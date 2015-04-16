using UnityEngine;
using System.Collections;

/*========================================================
 * Author: Tyler Remazki
 *
 * Class : Fire ball Attack
 *
 * Description: 
 =========================================================*/

public class FireballAttack : AbstractAttack
{

	public float fireballLifetime;
	public GameObject fireballObject;

	private bool	m_begin;
	private float m_holdTime;
	private float m_attackSpeed;
	private Vector2 m_direction;
	private Vector2 m_shotDirection;

	private GameObject m_fireballReference;



	public override void Start()
	{
		m_exitFlag = false;
	}

	public override void Update()
	{
	}

	public override IEnumerator StartAttack()
	{

		m_forceExit = false;
		m_parentActor = this.transform.parent.gameObject.GetComponent<Actor>();
		m_begin = true;
		m_fireballReference = (GameObject)Instantiate( fireballObject, m_parentActor.gameObject.transform.position, Quaternion.identity );

		Color _col = m_parentActor.actorColor;
		//_col.g -= 5.0f;

		foreach(Renderer obj in m_fireballReference.GetComponentsInChildren<Renderer>())
			obj.material.SetColor( "_TintColor", _col );
		yield return m_begin = true;
	}

	public override void HandleInput( InputHandler _input )
	{
		if ( m_begin )
		{
			if ( _input.Special().thisFrame || _input.Special().lastFrame )
			{
				m_direction = _input.LeftStick();
				DuringAttack();
				m_holdTime += Time.deltaTime;
			}
			else
			{
				AfterAttack();
				m_holdTime = 0.0f;
			}
		}
	}

	/// <summary>
	/// Throw the fireball in the direction supplied during 'DuringAttack()'
	/// </summary>
	public override void AfterAttack()
	{
		SpawnFireball();

		m_begin		= false;
		m_exitFlag	= true;
		//throw new System.NotImplementedException();
	}

	public override void ForcedExitCleanup()
	{
		Destroy( m_fireballReference );
	}
	/// <summary>
	/// Rotate around the character while the button is being held in the direction of the left stick.
	/// </summary>
	public override void DuringAttack()
	{
		float angle = Mathf.Atan2( m_direction.x, m_direction.y );
		bool facingRight = m_parentActor.GetMovementController().IsFacingRight();


		if ( !facingRight )
			angle = Mathf.Clamp( angle, -3.14f, 0.0f );
		else
			angle = Mathf.Clamp( angle, 0, 3.14f );


		AdjustAttackPower();
		m_fireballReference.transform.position = m_parentActor.transform.position +  new Vector3(0.0f, 1.0f, 0.0f) +   new Vector3
			(
				1.5f * Mathf.Sin(  angle ),
				1.5f * Mathf.Cos( angle ),
				0.0f
			);
		m_shotDirection = ( m_fireballReference.transform.position - (m_parentActor.transform.position + new Vector3( 0.0f, 1.0f, 0.0f )) ).normalized;
	}

	public override void BeforeAttack()
	{
		//throw new System.NotImplementedException();
	}

	public override void HitHurtBox( Collider2D _collider )
	{
		base.HitHurtBox( _collider );
	}

	private void SpawnFireball()
	{
		//Shoot the ball in the direction of the Left Stick. 
		m_fireballReference.GetComponent<PhysicsController>().Velocity = (Vector3)m_shotDirection * m_attackSpeed;

		//Enable the lifetime
		DestroyOverTime dotRef	= m_fireballReference.GetComponent<DestroyOverTime>();
		dotRef.timer						= fireballLifetime;
		dotRef.enabled					= true;


		ParticleSystem _particles = (ParticleSystem)Instantiate( particleOnUse, m_fireballReference.transform.position, Quaternion.identity );
		_particles.transform.parent = m_fireballReference.transform;

		AbstractHurtBox _temp;
		for ( int i = 0; i < boxColliders.Length; i++ )
		{
			_temp = (AbstractHurtBox)Instantiate( boxColliders[i], m_fireballReference.transform.position, transform.parent.transform.rotation );
			_temp.transform.parent = m_fireballReference.transform;
			_temp.SetGetAttackParent = this;
			_temp.SetReference( m_fireballReference );
			_temp.SetForce( forceType );
			_temp.EnableHurtBox();
		}
	}

	private void AdjustAttackPower()
	{
		if ( m_holdTime >= 0.0f  && m_holdTime < 1.5f )
		{
			m_attackSpeed = 75.0f;
			m_fireballReference.transform.localScale = new Vector3 (0.25f, 0.25f, 0.25f);
		}
		else if ( m_holdTime > 1.5f && m_holdTime < 3.0f )
		{
			m_attackSpeed = 50.0f;
			m_fireballReference.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
		}
		else if ( m_holdTime > 3.0 && m_holdTime < 4.5f )
		{
			m_attackSpeed = 25.0f;
			m_fireballReference.transform.localScale = new Vector3( 1.0f, 1.0f, 1.0f );
		}
		else if(m_holdTime > 5.0f)
		{
			m_attackSpeed = 15.0f;
			m_fireballReference.transform.localScale = new Vector3( 1.5f, 1.5f, 1.5f );
		}
	}
}