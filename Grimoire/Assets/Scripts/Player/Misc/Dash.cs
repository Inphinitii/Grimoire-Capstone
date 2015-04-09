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
	private SpellCharges				m_spellCharges;
	private Vector2						m_direction;
	private float								m_cooldown;
	private float								m_dashTimer;
	private bool								m_dashComplete;
	private bool								m_onCooldown;
	private bool								m_orientSelf;
	private bool m_forceCompletion;
	

	// Use this for initialization
	void Start()
	{
		m_physicsController		= GetComponent<PhysicsController>();
		m_movementController	= GetComponent<MovementController>();
		m_spellCharges				= GetComponent<SpellCharges>();
		m_onCooldown				= false;
		m_dashComplete			= true;	
	}

	// Update is called once per frame
	void Update()
	{
		if(!m_dashComplete)
		{
			if ( m_dashTimer > 0.0f )
			{
				//if ( m_movementController.IsJumping() == false )
					m_physicsController.Velocity = m_direction * dashSpeed;

				m_dashTimer -= Time.deltaTime;

				if ( m_forceCompletion )
					m_dashTimer = 0.0f;
			}
			else
			{
				m_movementController.m_capAcceleration = true;
				m_dashComplete	= true;
				m_onCooldown		= true;
			}
		}
		else if ( m_onCooldown )
		{
			if(m_cooldown > 0.0f)
			m_cooldown -= Time.deltaTime;

			if(m_cooldown <= 0.0f)
				m_onCooldown = false;
		}
	}

	public void StartDash(Vector2 _direction)
	{
		if(m_dashComplete && m_spellCharges.UseCharge())
		{
			m_movementController.m_capAcceleration = false;
			m_direction					= _direction;
			m_forceCompletion		= false;
			m_dashComplete		= false;
			m_dashTimer				= dashDuration;
		}
	}
	void GetDiamondGateDirection(ref Vector2 _direction, ref bool _orientationCheck)
	{
		if ( _direction.x > 0.6f )
		{
			_direction.x = 1.0f;
			_direction.y = 0.0f;
			_orientationCheck = true;
		}
		else if ( _direction.x < -0.6f )
		{
			_direction.x = -1.0f;
			_direction.y = 0.0f;
			_orientationCheck = true;
		}
		//if ( _direction.y > 0.6f )
		//{
		//	_direction.x = 0.0f;
		//	_direction.y = 1.0f;
		//}
		if ( _direction.y < -0.6f )
		{
			_direction.x = 0.0f;
			_direction.y = -1.0f;
		}
	}

	public bool DashComplete() { return m_dashComplete; }
	public bool ForceCompletion { get { return m_forceCompletion; } set { m_forceCompletion = value; } }
}
