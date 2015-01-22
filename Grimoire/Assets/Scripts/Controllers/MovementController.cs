using UnityEngine;
using System.Collections;

[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(PhysicsController))]
public class MovementController : MonoBehaviour
{
    public float p_groundAccel = 150.0f;
    public float p_airAccel = 150.0f;
    public float p_jumpAccel = 50.0f;
    public float p_dashAccel = 1000.0f;

    public float p_maxGroundAccel = 600.0f;
    public float p_maxAirAccel = 600.0f;

    public float p_groundTurningConstant = 10.0f;
    public float p_airTurningConstant = 5.0f;

    public float p_fastFallRate = 100.0f;

    public bool  p_multiJump = true;
    public int   p_totalJumps = 2;
    public float p_jumpCooldown = 0.25f;

    public LayerMask GroundLayerMask;
    public LayerMask PlatformLayerMask;

	private PhysicsController 	m_physicsController;
	private Actor		   		m_actor;
	private InputHandler	   	m_inputHandler;
	
	private Vector2 m_tempVel;
	private Vector2 m_leftStickInput;

    private int sign;
    private int signLastFrame;
    private int jumpCount;

    private float turningMultiplier;
    private float turningSpeedType;
    private float skinWidth = 0.001f;
    private float jumpTimer = 0.0f;

    private LayerMask mCurrentMask;
    private LayerMask mCurrentlyHitting;

    //TODO DOUBLE JUMPING
    //TODO WALL JUMPING
    //TODO WALL SLIDING
    //TODO FALLING THROUGH PLATFORMS

    //ISSUE SOMETIMES FALL THROUGH THIN PLATFORM.
    //ISSUE X POSITION DOES NOT HALT 

	void Start()
	{
		m_actor 		 			        = GetComponent<Actor>();
		m_inputHandler 	 			= GetComponent<InputHandler>();
		m_physicsController		= GetComponent<PhysicsController>();
	}

	void Update()
	{
            m_tempVel           = Vector2.zero;
			m_leftStickInput    = m_inputHandler.LeftStick ();
            turningMultiplier   = 1.0f;
		    
            // -- Ground Check Determinants -- //
            m_physicsController.p_applyGravity    = !GroundCheck();
            turningSpeedType                                = GroundCheck() ? p_groundTurningConstant : p_airTurningConstant;
            float _movementSpeedType                = GroundCheck() ? p_groundAccel : p_airAccel;

			// -- Scale Flipping -- //
            OrientationCheck();
            
            // -- Movement Speed Cap -- //
            CapAcceleration();

            // -- Turning -- //
            ApplyTurningSpeed(ref turningMultiplier);
        
            // -- Jumping -- //
            //if(jumpCount <= p_totalJumps && jumpTimer < p_jumpCooldown)
            ApplyJump(p_jumpAccel);

            if (jumpTimer > 0.0f)
                jumpTimer -= Time.deltaTime;

            // -- Update Forces -- //
            m_tempVel.x += (m_leftStickInput.x * _movementSpeedType);
            m_tempVel.y -= m_leftStickInput.y < 0 && !GroundCheck() ?  p_fastFallRate : 0.0f; // Fast Falling

            // -- Update Forces and Step through Physics -- //
            m_physicsController.AddToVelocity(m_tempVel);
			m_physicsController.Step();

            signLastFrame = sign;
	}

    void ApplyJump(float _forceModifier)
    {
        if (GroundCheck() && m_inputHandler.Jump().Held)
        {
            m_physicsController.p_applyGravity = true;
            m_tempVel.y += _forceModifier * 2.0f;
        }
    }

    void ApplyTurningSpeed(ref float _turningSpeed)
    {
        if (signLastFrame != sign && sign != 0)
        {
            _turningSpeed = turningSpeedType * sign;
            m_physicsController.AddToVelocity(new Vector2(m_physicsController.Velocity.x * _turningSpeed, 0.0f));
        }
    }
    
    bool GroundCheck()
    {
        //Determine whether we're moving down or up.
        bool goingUp = m_physicsController.Velocity.y > 0;
        Vector2 rayDirection = goingUp ? Vector2.up : -Vector2.up;

        //Ray starting position
        Vector2 start = transform.position;
        start.y = (transform.position.y+ skinWidth);

        float rayDistance = Mathf.Abs(m_physicsController.Velocity.y * Time.deltaTime);
        Debug.DrawRay(start, rayDistance * rayDirection, Color.red);

        if (goingUp)
            mCurrentMask &= ~PlatformLayerMask; //Ignore the Platform Layer with the ray cast
        else
            mCurrentMask = (1 << LayerMask.NameToLayer("Floor")) | (1 << LayerMask.NameToLayer("Platform")); //Include both the floor and platform layer

        if(Physics2D.Raycast(start, rayDirection, rayDistance, mCurrentMask).collider != null)
        {
            //Find what object we're on. If it's a platform, let our character move through it if they wish. 
            return true;
        }
        else
        {
            return false;
        }
    }

    void OrientationCheck()
    {
        //CURRENTLY BROKEN 
        //TODO CHANGE THE MODEL ORIENTATION WITH THE BOOK STAYING IN THE PROPER HAND. SCALING THE X BY -1 DOESNT WORK
        if (m_leftStickInput.x > 0.0f) 
		{
            sign = 1;
            if(transform.localRotation.y != 0.0f)
            {

                transform.Rotate(0, 180, 0, Space.Self);
            }
            //if (transform.localScale.z < 0.0f)
            //        transform.localScale = new Vector3 (transform.localScale.x, transform.localScale.y, -transform.localScale.z);
		} 
		else if (m_leftStickInput.x < 0.0f) 
		{
            sign = -1;
            if (transform.localRotation.y == 0.0f)
            {
                transform.Rotate(0, -180, 0, Space.Self);
            }
            //if (transform.localScale.z > 0.0f)
            //        transform.localScale = new Vector3 (transform.localScale.x, transform.localScale.y, -transform.localScale.z);
		}
        else
        {
            sign = signLastFrame;
            m_physicsController.Velocity = new Vector2(0.0f, m_physicsController.Velocity.y);
        }
    }

    void CapAcceleration()
    {
        float value = GroundCheck() ? p_maxGroundAccel : p_maxAirAccel;
        if (m_physicsController.Velocity.x > value)
            m_physicsController.Velocity = new Vector2(value, m_physicsController.Velocity.y);
        if (m_physicsController.Velocity.x < -value)
            m_physicsController.Velocity = new Vector2(-value, m_physicsController.Velocity.y);
    }
    
    void OnTriggerEnter2D(Collider2D _collision)
    {
        Debug.Log("Collision");
    }
	
}