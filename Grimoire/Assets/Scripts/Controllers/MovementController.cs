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

    //TODO DOUBLE JUMPING
    //TODO WALL JUMPING
    //TODO WALL SLIDING

	void Start()
	{
		m_actor 		 			= GetComponent<Actor>();
		m_inputHandler 	 			= GetComponent<InputHandler>();
		m_physicsController			= GetComponent<PhysicsController>();
	}

	void Update()
	{
            m_tempVel           = Vector2.zero;
			m_leftStickInput    = m_inputHandler.LeftStick ();
            turningMultiplier   = 1.0f;
		    
            // -- Ground Check Determinants -- //
            m_physicsController.p_applyGravity  = !GroundCheck();
            turningSpeedType                    = GroundCheck() ? p_groundTurningConstant : p_airTurningConstant;
            float _movementSpeedType            = GroundCheck() ? p_groundAccel : p_airAccel;

			// -- Scale Flipping -- //
            OrientationCheck();
            
            // -- Movement Speed Cap -- //
            CapAcceleration();

            // -- Turning -- //
            ApplyTurningSpeed(ref turningMultiplier);
        
            // -- Jumping -- //
            if(jumpCount <= p_totalJumps && jumpTimer < p_jumpCooldown)
            ApplyJump(p_jumpAccel);

            if (jumpTimer > 0.0f)
                jumpTimer -= Time.deltaTime;

            // -- Update Forces -- //
            m_tempVel.x += (m_leftStickInput.x * _movementSpeedType);
            m_tempVel.y -= m_leftStickInput.y < 0 && !GroundCheck() ?  p_fastFallRate : 0.0f; // Fast Falling

            // -- Update Forces and Step through Phyics -- //
            m_physicsController.Forces = m_tempVel;
			m_physicsController.Step();

            signLastFrame = sign;
	}

	void ApplyJump(float _forceModifier)
	{
        if(m_inputHandler.Jump().Down )
        {
            jumpCount++;
            jumpTimer = p_jumpCooldown;
            Debug.Log("Pressed");
        }

        if (m_inputHandler.Jump().Held)
        {
            m_tempVel.y -= _forceModifier * -m_physicsController.p_gravitationalForce;
        }


        if (m_physicsController.Velocity.y > 0 && m_inputHandler.Jump().Up)
            m_tempVel.y += 2.0f * -m_physicsController.p_gravitationalForce;
	}

    void ApplyTurningSpeed(ref float _turningSpeed)
    {
        //Not working correctly. Acceleration is being capped and preventing the turn from happening and X rate
        if (signLastFrame != sign && sign != 0)
        {
            _turningSpeed = turningSpeedType * sign;
            m_tempVel.x *= _turningSpeed;
        }
    }
    
    bool GroundCheck()
    {
        Vector2 start = transform.position;
        start.y = (transform.position.y - transform.localScale.y / 2f + skinWidth);

        float rayDistance = Mathf.Abs(m_physicsController.Velocity.y * Time.deltaTime);
        Debug.DrawRay(start, rayDistance * -Vector2.up, Color.red);

        if (Physics2D.Raycast(start, -Vector2.up, rayDistance, GroundLayerMask).collider != null ||
            Physics2D.Raycast(new Vector2(start.x + transform.localScale.x / 2, start.y), -Vector2.up, rayDistance, GroundLayerMask).collider != null ||
            Physics2D.Raycast(new Vector2(start.x - transform.localScale.x / 2, start.y), -Vector2.up, rayDistance, GroundLayerMask).collider != null)
        {
           jumpCount = 0;
           jumpTimer = 0.0f;
           return true;
           
        }
        else
        {
           return false;
        }
    }

    void OrientationCheck()
    {
        if (m_leftStickInput.x > 0.0f) 
		{
            sign = 1;
			if (transform.localScale.x < 0.0f)
					transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
		} 
		else if (m_leftStickInput.x < 0.0f) 
		{
            sign = -1;
			if (transform.localScale.x > 0.0f)
					transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}
    }

    void CapAcceleration()
    {
        float value = GroundCheck() ? p_maxGroundAccel : p_maxAirAccel;
        if (m_physicsController.Acceleration.x > value)
            m_physicsController.Acceleration = new Vector2(value, m_physicsController.Acceleration.y);
        if (m_physicsController.Acceleration.x < -value)
            m_physicsController.Acceleration = new Vector2(-value, m_physicsController.Acceleration.y);
    }
	
}