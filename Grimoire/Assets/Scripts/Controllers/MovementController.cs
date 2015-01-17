using UnityEngine;
using System.Collections;

[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(PhysicsController))]
public class MovementController : MonoBehaviour
{
    public float p_groundAccel;
    public float p_airAccel;
    public float p_jumpAccel;
    public float p_dashAccel;

    public float p_maxGroundAccel;
    public float p_maxAirAccel;

    public float p_groundTurningConstant;
    public float p_airTurningConstant;


	private PhysicsController 	m_physicsController;
	private Actor		   		m_actor;
	private InputHandler	   	m_inputHandler;
	
	private Vector2 m_tempVel;
    private Vector2 m_tempAccel;
	private Vector2 m_leftStickInput;

    private int sign;
    private int signLastFrame;
    private float turningMultiplier;
    private float turningSpeedType;

	void Start()
	{
		m_actor 		 			= GetComponent<Actor>();
		m_inputHandler 	 			= GetComponent<InputHandler>();
		m_physicsController			= GetComponent<PhysicsController>();
	}

	void Update()
	{
            m_tempVel = Vector2.zero;
            m_tempAccel             = m_physicsController.Acceleration;
			m_leftStickInput 		= m_inputHandler.LeftStick ();
		    
            // -- Ground Check Determinants -- //
            m_physicsController.p_applyGravity = !GroundCheck();
            turningSpeedType            = GroundCheck() ? p_groundTurningConstant : p_airTurningConstant;
            float _movementSpeedType    = GroundCheck() ? p_groundAccel : p_airAccel;

			// -- Scale Flipping -- //
            OrientationCheck();
            
            // -- Movement Speed Cap -- //
            CapAcceleration();

            // -- Turning -- //
            ApplyTurningSpeed(ref turningMultiplier);

            // -- Jumping -- //
            ApplyJump(p_jumpAccel);
            

            // -- Update Forces -- //
            m_tempVel.x += (m_leftStickInput.x * _movementSpeedType) * turningMultiplier;
            m_physicsController.Forces = m_tempVel;
			m_physicsController.Step();

            //Carry over to next frame
            signLastFrame = sign;
	}

	void ApplyJump(float _forceModifier)
	{
        if (GroundCheck() && m_inputHandler.DashDown) 
        m_tempVel.y -= _forceModifier * -m_physicsController.p_gravitationalForce;

        if (m_physicsController.Velocity.y > 0 && !m_inputHandler.DashDown)
            m_tempVel.y += 2.0f * -m_physicsController.p_gravitationalForce;
            //m_physicsController.Acceleration = new Vector2(m_physicsController.Acceleration.x, 0.0f);
	}

    void ApplyTurningSpeed(ref float _turningSpeed)
    {
        if (signLastFrame != sign && sign != 0)
        {
            _turningSpeed = turningSpeedType * sign;
        }
        else
            _turningSpeed = 1.0f;
    }
    
    bool GroundCheck()
    {
        Vector2 start = transform.position;
        start.y = (transform.position.y - transform.localScale.y / 2f - 0.001f);
        float skinWidth = 0.001f;

        if (Physics2D.Raycast(start, -Vector2.up, skinWidth).collider != null || Physics2D.Raycast(new Vector2(start.x + transform.localScale.x / 2, start.y), -Vector2.up, skinWidth).collider != null || Physics2D.Raycast(new Vector2(start.x - transform.localScale.x / 2, start.y), -Vector2.up, skinWidth).collider != null)
        {
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