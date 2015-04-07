using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhysicsController))]
[RequireComponent(typeof(MovementController))]
//Needs refactoring. Works for now. 
public class Dash : MonoBehaviour
{
	public float dashDuration;
	public float dashSpeed;
	public float dashCooldown;

	private PhysicsController			m_physicsController;
	private MovementController	m_movementController;
	private bool								m_dashing;
	private bool								m_dashComplete;
	

	// Use this for initialization
	void Start()
	{
		m_physicsController = GetComponent<PhysicsController>();
		m_movementController = GetComponent<MovementController>();
		m_dashing = false;
	}

	// Update is called once per frame
	void Update()
	{
		//if(GetComponent<InputHandler>().SpellSwap())
		//{
		//	if(!m_dashing)
		//		StartCoroutine(StartDash( Vector2.right ));
		//}
	}

	public IEnumerator StartDash(Vector2 _direction)
	{
		m_dashComplete = false;
		bool orientation = false;
		GetDiamondGateDirection( ref _direction , ref orientation);

		if ( orientation )
			m_movementController.OrientationCheck( _direction );

		m_dashing = true;
		//m_physicsController.applyGravity = false;
		if(m_movementController.IsJumping() == false)
		m_physicsController.Velocity = _direction * dashSpeed;

		m_movementController.m_capAcceleration = false;
		yield return new WaitForSeconds( dashDuration + dashCooldown );
		m_movementController.m_capAcceleration = true;

		//m_physicsController.applyGravity = true;
		m_dashComplete = true;
		m_dashing = false;
	}

	void GetDiamondGateDirection(ref Vector2 _direction, ref bool _orientationCheck)
	{
		if ( _direction.x > 0.85f )
		{
			_direction.x = 1.0f;
			_direction.y = 0.0f;
			_orientationCheck = true;
		}
		if ( _direction.x < -0.85f )
		{
			_direction.x = -1.0f;
			_direction.y = 0.0f;
			_orientationCheck = true;
		}
		if ( _direction.y > 0.85f )
		{
			_direction.x = 0.0f;
			_direction.y = 1.0f;
		}
		if ( _direction.y < -0.85f )
		{
			_direction.x = 0.0f;
			_direction.y = -1.0f;
		}
	}

	public bool DashComplete() { return m_dashComplete; }
}
