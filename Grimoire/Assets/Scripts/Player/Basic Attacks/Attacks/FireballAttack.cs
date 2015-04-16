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

	public float Level1Threshhold;
	public float Level1Force;

	public float Level2Threshhold;
	public float Level2Force;

	public float Level3Threshhold;
	public float Level3Force;

	public float Level4Threshhold;
	public float Level4Force;

	public bool ignoreMovementCap = false;


	private bool	m_begin;
	private float m_holdTime;
	private float m_attackSpeed;
	private Vector2 m_direction;
	private Vector2 m_shotDirection;
	private bool m_facingRight;

	private GameObject m_fireballReference;



	public override void Start()
	{
		m_parentActor = this.transform.parent.gameObject.GetComponent<Actor>();
		m_exitFlag = false;
	}

	public override void Update()
	{
	}

	public override IEnumerator StartAttack()
	{

		m_forceExit = false;
		m_begin = true;
		m_fireballReference = (GameObject)Instantiate( fireballObject, m_parentActor.gameObject.transform.position, Quaternion.identity );

		Color _col = m_parentActor.actorColor;
		_col.g -= 0.55f;

		foreach(Renderer obj in m_fireballReference.GetComponentsInChildren<Renderer>())
			obj.material.SetColor( "_TintColor", _col );
		yield return m_begin = true;
	}

	public override void HandleInput( InputHandler _input )
	{
		if ( m_begin )
		{
			m_facingRight = m_parentActor.GetMovementController().IsFacingRight();
			if ( _input.Special().thisFrame || _input.Special().lastFrame )
			{
				if ( _input.LeftStick() != Vector2.zero )
					m_direction = _input.LeftStick();
				else
				{
					if ( m_facingRight )
						m_direction = new Vector2( 1.0f, 0.0f );
					else
					{
						Debug.Log( "Left" );
						m_direction = new Vector2( -1.0f, 0.0f );
					}
				}
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
		bool facingRight = m_parentActor.GetMovementController().IsFacingRight();
		float angle = Mathf.Atan2( m_direction.x, m_direction.y );


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

	public override void HitEnemy( Collider2D _collider )
	{
		Camera.main.GetComponent<CameraShake>().Shake();
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
		if ( m_holdTime >= 0.0f && m_holdTime < Level1Threshhold )
		{
			m_attackSpeed = 45.0f;
			m_fireballReference.transform.localScale = Vector3.Slerp( m_fireballReference.transform.localScale, new Vector3( 0.25f, 0.25f, 0.25f ), Level1Threshhold );

			hitForce = -Level1Force;
		}
		else if ( m_holdTime > Level1Threshhold && m_holdTime < Level2Threshhold )
		{
			m_attackSpeed = 35.0f;
			m_fireballReference.transform.localScale = Vector3.Slerp( m_fireballReference.transform.localScale, new Vector3( 0.5f, 0.5f, 0.5f ), Level2Threshhold );
			hitForce = -Level2Force;
		}
		else if ( m_holdTime > Level2Threshhold && m_holdTime < Level3Threshhold )
		{
			m_attackSpeed = 25.0f;
			m_fireballReference.transform.localScale = new Vector3( 1.0f, 1.0f, 1.0f );
			hitForce = -Level3Force;
		}
		else if ( m_holdTime > Level3Threshhold )
		{
			m_attackSpeed = 15.0f;
			m_fireballReference.transform.localScale = new Vector3( 1.5f, 1.5f, 1.5f );
			hitForce = -Level4Force;
		}
	}
}