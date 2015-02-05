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
    public float p_dashDuration;
    public float p_dashCooldown;

    //Maximum Accelerations
    public float p_maxGroundAccel = 12.0f;
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
    private Actor m_actor;
    private InputHandler m_inputHandler;
    private bool m_isJumping;

    private Vector2 m_tempForce;
    private Vector2 m_tempVel;
    private Vector2 m_leftStickInput;

    private int sign;
    private int signLastFrame;
    private int jumpCount = 0;

    private float turningMultiplier;
    private float turningSpeedType;
    private float skinWidth = 0.001f;
    private float jumpTimer = 0.0f;

    private float dashTimer = 0.0f;
    private float dashCD;
    private bool startDashTimer;

    private LayerMask mCurrentMask;
    private LayerMask mCurrentlyHitting;

    //TODO WALL JUMPING
    //TODO WALL SLIDING
    //TODO FALLING THROUGH PLATFORMS

    //ISSUE SOMETIMES FALL THROUGH THIN PLATFORM.
    //ISSUE X POSITION DOES NOT HALT 

    void Start() {
        m_actor = GetComponent<Actor>();
        m_inputHandler = GetComponent<InputHandler>();
        m_physicsController = GetComponent<PhysicsController>();
    }

    void FixedUpdate() {
        m_leftStickInput = m_inputHandler.LeftStick();
        turningMultiplier = 1.0f;

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

        if(startDashTimer){
            dashTimer += Time.deltaTime;
            if(dashTimer >= p_dashCooldown)
                startDashTimer = false;
        }
            

        // -- Update Forces -- //
        m_tempForce.x += (m_leftStickInput.x * _movementSpeedType);
        //m_tempForce.y -= m_leftStickInput.y < 0 && m_isJumping ? p_fastFallRate : 0.0f; // Fast Falling -- NEEDS TWEAKING -- 

        // -- Update Forces and Step through Physics -- //
        m_physicsController.AddToForce(m_tempForce); //Apply Forces
        m_physicsController.AddToVelocity(m_tempVel); //Apply to Velocity if necessary. 
        signLastFrame = sign;
        m_tempForce = Vector2.zero;
        m_tempVel   = Vector2.zero;   
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
        if (dashTimer > p_dashCooldown) {
            m_tempVel += m_leftStickInput * p_dashAccel;
        }
        else {
            startDashTimer = true;
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

        if (goingUp)
            mCurrentMask &= ~PlatformLayerMask; //Ignore the Platform Layer with the ray cast
        else
            mCurrentMask = (1 << LayerMask.NameToLayer("Floor")) | (1 << LayerMask.NameToLayer("Platform")); //Include both the floor and platform layer

        if (Physics2D.Raycast(start, rayDirection, rayDistance, mCurrentMask).collider != null) {
            //Find what object we're on. If it's a platform, let our character move through it if they wish. 
            jumpCount = 0;
            m_isJumping = false;
        }
        else {
            m_isJumping = true;
        }
    }

    void OrientationCheck() {
        //Left Stick Right
        if (m_leftStickInput.x > 0.0f) {
            sign = 1;
            if (!m_isJumping)
                if (transform.localRotation.y != 0.0f) {
                    transform.Rotate(0, 180, 0, Space.Self);
                }
        }
        //Left Stick Left
        else if (m_leftStickInput.x < 0.0f) {
            sign = -1;
            if (!m_isJumping)
                if (transform.localRotation.y == 0.0f) {
                    transform.Rotate(0, -180, 0, Space.Self);
                }
        }
        //Left Stick Null
        else {
            if (m_physicsController.Velocity.x != 0 && !m_isJumping)
                m_physicsController.Velocity = new Vector2(m_physicsController.Velocity.x * p_groundDampeningConstant, m_physicsController.Velocity.y);
        }
    }

    void CapAcceleration() {
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