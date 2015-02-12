using UnityEngine;
using System.Collections;

[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(PhysicsController))]
public class MovementController : MonoBehaviour {
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
    private int signLastFrame;
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

    //TODO WALL JUMPING
    //TODO WALL SLIDING
    //TODO FALLING THROUGH PLATFORMS 

    void Start() {
        m_actor = GetComponent<Actor>();
        m_inputHandler = GetComponent<InputHandler>();
        m_physicsController = GetComponent<PhysicsController>();
    }

    void FixedUpdate() {
        m_leftStickInput = m_inputHandler.LeftStick();
        turningMultiplier = 1.0f;

        if (Mathf.Abs(m_physicsController.Velocity.x) > 10.0f) {
            m_inputHandler.ComboCheck = false;
        }
        else{
            m_inputHandler.ComboCheck = true;
        }
        // -- Ground Check Determinants -- //
        GroundCheck();
        m_physicsController.p_applyGravity = m_isJumping;
        turningSpeedType = !m_isJumping ? p_groundTurningConstant : p_airTurningConstant;
        float _movementSpeedType = !m_isJumping ? p_groundAccel : p_airAccel;

        // -- Scale Flipping -- //
        OrientationCheck();

        // -- Movement Speed Cap -- //
        CapAcceleration();

        // -- Turning -- //
        ApplyTurningSpeed(ref turningMultiplier);

        // -- Jumping -- //
        //if (jumpTimer <= 0)
           // ApplyJump(p_jumpAccel);

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

            

        // -- Update Forces -- //
        if(m_isMoving)
            m_tempForce.x += (m_leftStickInput.x * _movementSpeedType);

        m_tempForce.y -= m_leftStickInput.y < 0 && m_isJumping ? p_fastFallRate : 0.0f; // Fast Falling -- NEEDS TWEAKING -- 

        // -- Update Forces and Step through Physics -- //
        //Dashing is void of physics.
        if (!m_isDashing) {
            m_physicsController.AddToForce(m_tempForce); //Apply Forces
            m_physicsController.AddToVelocity(m_tempVel); //Apply to Velocity if necessary. 
        }

        m_tempForce = Vector2.zero;
        m_tempVel   = Vector2.zero;   

        signLastFrame = sign;
    }

    public void ApplyJump() {
        if (jumpCount < p_totalJumps && jumpTimer <= 0.0f) {
            if (jumpCount > 0) {
                m_physicsController.Velocity    = new Vector2(Mathf.Abs(m_physicsController.Velocity.x) * sign, m_physicsController.Velocity.y);
                m_physicsController.Forces      = new Vector2(0.0f, 0.0f);
                m_tempForce.y += (p_jumpAccel * 2.0f);

                if (m_physicsController.Velocity.y < 0.0f)
                    m_tempForce.y += m_physicsController.Velocity.y;
            }
            else {
                m_tempForce.y += (p_jumpAccel * 2.0f);
            }
            m_tempForce.y += Mathf.Abs(m_physicsController.Velocity.y) * 100.0f; //Compensate for the y velocity when falling. Works but idk why. Witchcraft?

            m_physicsController.p_applyGravity = true;
            m_isJumping = true;
            jumpTimer = p_jumpCooldown;
            jumpCount++;
        }
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

    void GroundCheck() {
        //Determine whether we're moving down or up.
        bool goingUp = m_physicsController.Velocity.y > 0;
        Vector2 rayDirection = goingUp ? Vector2.up : -Vector2.up;

        //Ray starting position
        Vector2 start = transform.position;
        start.y = (transform.position.y + skinWidth);

        float rayDistance = Mathf.Abs(m_physicsController.Velocity.y * Time.deltaTime);
        Debug.DrawRay(start, rayDistance * rayDirection, Color.red);

        if (goingUp) {
            mCurrentMask &= ~PlatformLayerMask; //Ignore the Platform Layer with the ray cast
        }
        else {
            mCurrentMask = (1 << LayerMask.NameToLayer("Floor")) | (1 << LayerMask.NameToLayer("Platform")); //Include both the floor and platform layer
        }

        RaycastHit2D ray = Physics2D.Raycast(start, rayDirection, rayDistance, mCurrentMask);
        if (ray.collider != null) {
            //Find what object we're on. If it's a platform, let our character move through it if they wish. 
            jumpCount = 0;
            m_isJumping = false;
            if (ray.collider.tag == "Platform") {
                if (m_inputHandler.FallThrough())
                    m_isJumping = true;
            }
        }
        else {
            m_isJumping = true;
        }
    }

    void OrientationCheck() {
        //Left Stick Right
        if (m_leftStickInput.x > 0.0f) {
            m_isMoving = true;
            sign = 1;
            if (!m_isJumping)
                if (transform.localRotation.y != 0.0f) {
                    transform.Rotate(0, 180, 0, Space.Self);
                }
        }
        //Left Stick Left
        else if (m_leftStickInput.x < 0.0f) {
            m_isMoving = true;
            sign = -1;
            if (!m_isJumping)
                if (transform.localRotation.y == 0.0f) {
                    transform.Rotate(0, -180, 0, Space.Self);
                }
        }
        //Left Stick Null
        else {
            if (m_physicsController.Velocity.x != 0 && !m_isJumping) {
                m_physicsController.Velocity = new Vector2(m_physicsController.Velocity.x * p_groundDampeningConstant, m_physicsController.Velocity.y);
                m_isMoving = false;
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

    void OnTriggerEnter2D(Collider2D _collision) {
        Debug.Log("Collision");
    }

}