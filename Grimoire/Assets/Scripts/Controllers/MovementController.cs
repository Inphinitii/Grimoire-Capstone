using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ControllerCollider))]
[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(PhysicsController))]
public class MovementController : MonoBehaviour
{
	/* MOVED TO PHYSICS CONTROLLER
	public float p_gravity 			= -25f;
	public float p_groundDampening 	= 20f; 
	public float p_airDampening 	= 5f;
	public float p_mass 			= 10f;
	*/
	
	[Range(-1.0f, 0.0f)]
	public float p_dragResistance 	= -0.15f;

	//TODO 
	// SEPARATE PARTS INTO A PHYSICS COMPONENT

	[HideInInspector]

	private ControllerCollider 	m_controllerCollider;
	private PhysicsController 	m_physicsController;
	private Actor		   		m_actor;
	private InputHandler	   	m_inputHandler;
	
	private Vector2 m_tempVel;

	/* BEING MOVED TO PHYSICSCONTROLLER
	private Vector3 m_velocity;
	private Vector2 m_acceleration;
	private Vector2 m_force;
	*/
	
	private Vector2 m_leftStickInput;

	void Start()
	{
		m_controllerCollider 	 	= GetComponent<ControllerCollider>();
		m_actor 		 			= GetComponent<Actor>();
		m_inputHandler 	 			= GetComponent<InputHandler>();
		m_physicsController			= GetComponent<PhysicsController>();
	}

	void Update()
	{
			m_tempVel 						= m_physicsController.Velocity;
			m_leftStickInput 				= m_inputHandler.LeftStick ();
		          
			m_physicsController.p_applyGravity = m_controllerCollider.isGrounded () ? false : true;

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
					m_tempVel.y += Mathf.Lerp (m_tempVel.y, (m_tempVel.y * p_dragResistance), 2.0f);
				}
			}


			// -- Direction changing speed -- //
			float _smoothedMovementFactor = m_controllerCollider.isGrounded () ? m_physicsController.p_groundFriction : m_physicsController.p_airFriction;
			float _movementSpeedType = m_controllerCollider.isGrounded () ? m_actor.AccessProperties.MovementSpeed : m_actor.AccessProperties.AirMovementSpeed;
			m_tempVel.x += Mathf.Lerp (m_tempVel.x, m_leftStickInput.x * _movementSpeedType, Time.deltaTime * _smoothedMovementFactor);
		
		// -- Lerp to new  X position using left stick input -- //
			m_physicsController.Velocity = m_controllerCollider.Move (m_tempVel * Time.deltaTime);
			m_physicsController.Step();
			
	
	}

	void ApplyJump(float _forceModifier)
	{
		m_tempVel.y += Mathf.Sqrt( _forceModifier * m_actor.AccessProperties.JumpHeight * -m_physicsController.p_gravitationalForce);
	}

	
}