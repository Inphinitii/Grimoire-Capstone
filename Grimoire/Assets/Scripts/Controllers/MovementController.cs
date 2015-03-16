using UnityEngine;
using System.Collections;

[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(PhysicsController))]
public class MovementController : MonoBehaviour {

	//TODO NEEDS REFACTORING 
    //Acceleration
    public float p_groundAccel = 100.0f;
    public float p_airAccel = 100.0f;
    public float p_jumpAccel = 400.0f;

    //Dash Variables
    public float p_dashAccel = 200.0f;
    public float p_dashStartupWindow;
    public float p_dashDuration;

    //Maximum Accelerations
    public float p_maxGroundAccel = 10.0f;
    public float p_maxAirAccel = 9.0f;

    //Turning Modifiers
    public float p_groundTurningConstant = 1.2f;
    public float p_airTurningConstant = 0.5f;

    //Dampeners
    public float p_groundDampeningConstant = 0.9f;
    // public float p_airDampeningConstant = 0.5f;

    public float p_fastFallRate = 100.0f;

    public bool p_multiJump = true;
    public int p_totalJumps = 2;
    public float p_jumpCooldown = 0.4f;

    public LayerMask GroundLayerMask;
    public LayerMask PlatformLayerMask;

    private PhysicsController m_physicsController;
    private InputHandler m_inputHandler;
    private bool m_isJumping;
    private bool m_dashWaitForInput;
    private bool m_isDashing;
    public bool m_isMoving;
    private Actor m_actor;

    private Vector2 m_leftStickInput;
    private Vector2 m_tempForce;
    private Vector2 m_tempVel;

    private int jumpCount = 0;
    public int signLastFrame;
    private int sign;

    private float turningMultiplier;
    private float turningSpeedType;
    private float skinWidth = 0.001f;
    private float jumpTimer = 0.0f;

    private float dashTimer = 0.0f;
    //private float runTimer = 0.05f;

    private LayerMask mCurrentMask;
    private LayerMask mCurrentlyHitting;


    private Vector2 dashDirection;
    float movementSpeedType;
    //TODO WALL JUMPING
    //TODO WALL SLIDING
    //TODO FALLING THROUGH PLATFORMS 
    //TODO FIX MOVING THROUGH PLATFORMS BOTTOM UP

    void Start() {
        m_actor = GetComponent<Actor>();
        m_inputHandler = GetComponent<InputHandler>();
        m_physicsController = GetComponent<PhysicsController>();
        m_isJumping = true;
    }

    void FixedUpdate() {
        m_leftStickInput = m_inputHandler.LeftStick();
        turningMultiplier = 1.0f;
       // m_isMoving = false;

        // -- Ground Check Determinants -- //
        //m_physicsController.p_applyGravity = m_isJumping;
        turningSpeedType = !m_isJumping ? p_groundTurningConstant : p_airTurningConstant;
        movementSpeedType = !m_isJumping ? p_groundAccel : p_airAccel;

        // -- Movement Speed Cap -- //
        CapAcceleration();

        //Move this later! Works for now as a patch job. 
        //Stops the player from moving continuously. Encapsulate this in a state eventually. 
        DampenMovement();

        // -- Turning -- //
        ApplyTurningSpeed(ref turningMultiplier);

        // -- Jumping -- //
        if (jumpTimer > 0.0f)
            jumpTimer -= Time.deltaTime;

        //Clean this up once it's working properly.
        //Pause the player for X amount of seconds in the air, let them choose a direction, and shoot them towards it. 
        if (m_dashWaitForInput) {          
            dashTimer -= Time.deltaTime;

            //Pause the player and wait for X Seconds before returning control to the physics controller.
            if (dashTimer <= 0.0f) {
                m_physicsController.Velocity = Vector2.zero;
                m_physicsController.PausePhysics(false);
                m_dashWaitForInput = false;
                m_isDashing = false;    
            }

            //If paused and the directional stick is pressed with the dash button, move in that direction
            //Y Direction is current screwed up for some reason?
            if (m_isDashing) {
                m_physicsController.PausePhysics(false);
                m_physicsController.Velocity = dashDirection * p_dashAccel;
                m_physicsController.ClearValues();
            }
        }


         m_tempForce.y -= m_leftStickInput.y < 0 && m_isJumping ? p_fastFallRate : 0.0f; // Fast Falling -- NEEDS TWEAKING -- 
         
        // -- Update Forces and Step through Physics -- //
        m_physicsController.AddToForce(m_tempForce);
        m_physicsController.AddToVelocity(m_tempVel);

        m_tempForce = Vector2.zero;
        m_tempVel   = Vector2.zero;

        if(m_physicsController.Velocity.y <= 0)
            GroundCheck();

        OneWayPlatform();
        signLastFrame = sign;
    }

    public void ApplyJump() {
        if (jumpCount < p_totalJumps && jumpTimer <= 0.0f) {
            m_isJumping = true;
            m_physicsController.Velocity = new Vector2(m_physicsController.Velocity.x, p_jumpAccel);
            m_physicsController.p_applyGravity = true;
            jumpTimer = p_jumpCooldown;
            jumpCount++;
        }
    }

    public void MoveX(Vector2 m_direction)
    {
        OrientationCheck(m_direction);
        if (m_isMoving)
        {
            m_tempForce.x += (m_direction.x * movementSpeedType);
        }
    }

    public void DampenMovement()
    {
        if (m_physicsController.Velocity.x != 0 && !m_isJumping)
        {
            m_physicsController.Velocity = new Vector2(m_physicsController.Velocity.x * p_groundDampeningConstant, m_physicsController.Velocity.y);
        }
		m_isMoving = m_leftStickInput.x > 0 || m_leftStickInput.x < 0 ? true : false;
    }
    //NEEDS FIXING
    public void ApplyDash() {
        if (!m_dashWaitForInput) {
            dashTimer = p_dashStartupWindow;
            m_dashWaitForInput = true;
            m_physicsController.PausePhysics(true);
        }
        else if(m_inputHandler.LeftStick() != Vector2.zero){
            dashDirection = m_inputHandler.LeftStick();
            dashTimer = p_dashDuration;
            m_isDashing = true;
        }

    }

    void ApplyTurningSpeed(ref float _turningSpeed) {
        if (signLastFrame != sign && sign != 0) {
            _turningSpeed = turningSpeedType * sign;
            m_tempForce.x += Mathf.Abs(m_physicsController.Velocity.x) * _turningSpeed;
        }
    }


    void OneWayPlatform()
    {
        bool goingUp = m_physicsController.Velocity.y > 0;
        if (goingUp)
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Platform"));
        }
        else
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Platform"), false);
        }

    }
    void GroundCheck() {
        bool goingUp = m_physicsController.Velocity.y > 0;
        Vector2 rayDirection = goingUp ? Vector2.up : -Vector2.up;
        //Ray starting position
        Vector2 start = transform.position;
        start.y = (transform.position.y + skinWidth);

        float rayDistance = Mathf.Abs(m_physicsController.Velocity.y * Time.deltaTime);
        Debug.DrawRay(start, rayDistance * rayDirection, Color.red);
        mCurrentMask = (1 << LayerMask.NameToLayer("Floor")) | (1 << LayerMask.NameToLayer("Platform")); //Include both the floor and platform layer
        
        RaycastHit2D ray = Physics2D.Raycast(start, rayDirection, rayDistance, mCurrentMask);
        if (ray.collider != null) {
            m_physicsController.Velocity = new Vector2(m_physicsController.Velocity.x, 0);
            jumpCount = 0;
            //m_physicsController.Position = m_physicsController.Position + (Vector3.up * (ray.distance + skinWidth));
            //m_physicsController.ClearValues();
            m_isJumping = false;
        }
        else {
			if(m_isJumping == false)
        	jumpCount++;
        	
            m_isJumping = true;
        }
    }

    public void OrientationCheck(Vector2 _stick) {
        //Left Stick Right
        if (_stick.x > 0.0f)
        {
            m_isMoving = true;
            sign = 1;
            if (!m_isJumping)
                if (transform.localRotation.y != 0.0f) {
                    transform.Rotate(0, 180, 0, Space.Self);
                }
        }
        //Left Stick Left
        else if (_stick.x < 0.0f)
        {
            m_isMoving = true;
            sign = -1;
            if (!m_isJumping)
                if (transform.localRotation.y == 0.0f) {
                    transform.Rotate(0, -180, 0, Space.Self);
                }
        }
    }

    void CapAcceleration() {
        //Grounded Movement Speed Cap
        float value = !m_isJumping ? p_maxGroundAccel : p_maxAirAccel;
        if (m_physicsController.Velocity.x > value)
            m_physicsController.Velocity = new Vector2(value, m_physicsController.Velocity.y);
        if (m_physicsController.Velocity.x < -value)
            m_physicsController.Velocity = new Vector2(-value, m_physicsController.Velocity.y);
    }

    public bool IsJumping() {  return m_isJumping; }
}