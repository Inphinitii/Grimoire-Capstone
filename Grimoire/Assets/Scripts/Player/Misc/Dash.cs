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
	public ParticleSystem dashParticle;

	private PhysicsController			m_physicsController;
	private MovementController	m_movementController;
	private Vector2						m_direction;
	private float								m_cooldown;
	private float								m_dashTimer;
	private bool								m_dashComplete;
	private bool								m_onCooldown;
	private bool								m_orientSelf;
	private bool								m_forceCompletion;
	private ParticleSystem _temp;
	

	// Use this for initialization
	void Start()
	{
		m_physicsController		= GetComponent<PhysicsController>();
		m_movementController	= GetComponent<MovementController>();
		m_onCooldown				= false;
		m_dashComplete			= true;

		Vector3 pos = this.transform.position;
		pos.y += 1;
		// dashParticle.startColor = GetComponent<Actor>().actorColor;
		if ( dashParticle != null )
		{
			_temp = Instantiate( dashParticle, pos, Quaternion.identity ) as ParticleSystem;
			_temp.transform.parent = this.transform;
			_temp.enableEmission = false;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if(!m_dashComplete)
		{
			if ( m_dashTimer > 0.0f )
			{
				m_physicsController.applyGravity = false;
				m_physicsController.Forces = m_direction * dashSpeed;

				if ( dashParticle != null )
					_temp.enableEmission = true;

				m_dashTimer -= Time.deltaTime;

				if ( m_forceCompletion )
					m_dashTimer = 0.0f;
			}
			else
			{
				m_movementController.m_capAcceleration = true;
				m_dashComplete	= true;
				m_onCooldown		= true;
				m_physicsController.applyGravity = true;

				if ( dashParticle != null )
					_temp.enableEmission = false;
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
		m_direction					= _direction;
		GetDiamondGateDirection( ref m_direction, ref m_orientSelf);

		if(m_direction.y <= 0)
			if(m_dashComplete)
			{
				m_movementController.m_capAcceleration = false;
				m_forceCompletion		= false;
				m_dashComplete		= false;
				m_dashTimer				= dashDuration;
				m_physicsController.ClearValues();
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
		if ( _direction.y > 0.6f )
		{
			_direction.x = 0.0f;
			_direction.y = 1.0f;
		}
		if ( _direction.y < -0.6f )
		{
			_direction.x = 0.0f;
			_direction.y = -1.0f;
		}
	}

	public bool DashComplete() { return m_dashComplete; }
	public bool ForceCompletion { get { return m_forceCompletion; } set { m_forceCompletion = value; } }
}
