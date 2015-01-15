using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ControllerCollider))]
[RequireComponent(typeof(InputHandler))]
public class MovementController : MonoBehaviour
{
	public float p_gravity 			= -25f;
	public float p_groundDampening 	= 20f; 
	public float p_airDampening 	= 5f;
	public float p_mass 			= 10f;

	[Range(-1.0f, 0.0f)]
	public float p_dragResistance 	= -0.15f;

	//TODO 
	// SEPARATE PARTS INTO A PHYSICS COMPONENT

	[HideInInspector]

	private ControllerCollider 	m_controllerCollider;
	private Actor		   		m_actor;
	private InputHandler	   	m_inputHandler;


	private Vector3 m_velocity;
	private Vector2 m_acceleration;
	private Vector2 m_force;
	private Vector2 m_leftStickInput;

	void Start()
	{
		m_controllerCollider 	 	= GetComponent<ControllerCollider>();
		m_actor 		 			= GetComponent<Actor>();
		m_inputHandler 	 			= GetComponent<InputHandler>();
	}

	void Update()
	{
			m_force = Vector2.zero;
			m_velocity = m_controllerCollider.Velocity ();
			m_leftStickInput = m_inputHandler.LeftStick ();

			if (m_controllerCollider.isGrounded ())
					m_acceleration.y = 0;


			// -- Scale Flipping -- //
			if (m_leftStickInput.x > 0.0f) 
			{
				if (transform.localScale.x < 0.0f)
						transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			} 
			else if (m_leftStickInput.x < 0.0f) 
			{
				if (transform.localScale.x > 0.0f)
						transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			}

			// -- Jumping -- //
			if (m_leftStickInput.y > 0) 
			{
				if (m_controllerCollider.isGrounded ()) 
				{
					ApplyJump (2.0f);
				} 
				else 
				{
					// -- Wall Jumping -- //
					if (m_controllerCollider.CState ().Left || m_controllerCollider.CState ().Right) 
					{
							//ApplyJump(3.0f);
					}
				}
			}

			// -- Wall Sliding -- //
			if (!m_controllerCollider.isGrounded ()) 
			{
				if (m_controllerCollider.CState ().Left || m_controllerCollider.CState ().Right) 
				{
						m_velocity.y += Mathf.Lerp (m_velocity.y, (m_velocity.y * p_dragResistance), 2.0f);
				}
			}


			// -- Direction changing speed -- //
			float _smoothedMovementFactor = m_controllerCollider.isGrounded () ? p_groundDampening : p_airDampening;
			float _movementSpeedType = m_controllerCollider.isGrounded () ? m_actor.AccessProperties.MovementSpeed : m_actor.AccessProperties.AirMovementSpeed;

			// -- Lerp to new  X position using left stick input -- //
			m_velocity.x = Mathf.Lerp (m_velocity.x, m_leftStickInput.x * _movementSpeedType, Time.deltaTime * _smoothedMovementFactor);

			// -- Gravity Application -- //
			if (!m_controllerCollider.isGrounded ())
					ApplyGravity ();

			m_force = p_mass * m_acceleration;
			m_velocity += (Vector3)m_force * Time.deltaTime * Time.deltaTime;
			// -- Controller Callback -- //
			m_controllerCollider.Move ((Vector2)m_velocity * Time.deltaTime);
	
	}

	void ApplyJump(float _forceModifier)
	{
		m_velocity.y = Mathf.Sqrt( _forceModifier * m_actor.AccessProperties.JumpHeight * -p_gravity );
	}

	void ApplyGravity()
	{
		m_acceleration.y += p_gravity;
	}
	
	public Vector3 Velocity
	{
		get { return m_velocity; }
		set { m_velocity = value;}
	}
}