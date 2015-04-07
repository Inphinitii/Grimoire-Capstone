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

	private PhysicsController	m_physicsController;
	private MovementController	m_movementController;
	private bool				m_dashComplete;
	private bool				m_orientSelf;
	

	// Use this for initialization
	void Start()
	{
		m_physicsController		= GetComponent<PhysicsController>();
		m_movementController	= GetComponent<MovementController>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public IEnumerator StartDash(Vector2 _direction)
	{
		m_dashComplete		= false;
		m_orientSelf		= false;

		GetDiamondGateDirection( ref _direction, ref m_orientSelf );

		if ( m_orientSelf )
			m_movementController.OrientationCheck( _direction );

		//m_physicsController.applyGravity = false;
		if(m_movementController.IsJumping() == false)
			m_physicsController.Velocity = _direction * dashSpeed;

		m_movementController.m_capAcceleration = false; //Remove the acceleration cap temporarily. 
		yield return new WaitForSeconds( dashDuration + dashCooldown );
		m_movementController.m_capAcceleration = true;

		//m_physicsController.applyGravity = true;
		m_dashComplete = true;
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
