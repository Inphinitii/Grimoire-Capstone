﻿using UnityEngine;
using System.Collections;

/*========================================================
 * Author: Tyler Remazki
 *
 * Class : Abstract Attack
 *
 * Description: Defines the base behaviour of every Attack within the game.
 * Stores the multiple hitboxes that are activated when the attack is started. 
 =========================================================*/

[System.Serializable]
public abstract class AbstractAttack : MonoBehaviour
{


	public AbstractHurtBox[]		boxColliders;
	public Properties.ForceType forceType;

	public ParticleSystem			particleOnHit;
	public ParticleSystem			particleOnUse;

	public float duration;
	public float startupTime;
	public float cooldownTime;

	public float	dashWindow;
	public bool	dashOnCD;

	public bool ableToCancel;

	public Vector2	hitDirection;
	public bool		hitAlongDistanceVector;
	public float		hitForce;
	public int			hitDamage;
	public bool		staticForce;

	private float m_onHitFreezeDuration = 0.35f;

	protected AbstractHurtBox[]	m_childHurtBoxes;
	protected bool							m_duringAttack;
	private bool								m_dashAvailable;



	public virtual void Start()
	{
		m_childHurtBoxes = new AbstractHurtBox[boxColliders.Length];
		AbstractHurtBox _temp;
		for ( int i = 0; i < boxColliders.Length; i++ )
		{
			_temp = (AbstractHurtBox)Instantiate( boxColliders[i], this.transform.position, transform.parent.transform.rotation );
			_temp.transform.parent = this.transform;
			_temp.SetForce( forceType );
			_temp.DisableHurtBox();
			m_childHurtBoxes[i] = _temp;
		}
	}

	public virtual void Update()
	{
		if ( m_duringAttack )
		{
			DuringAttack();
		}
	}

	/// <summary>
	/// Called when the attack object is enabled.
	/// </summary>
	public virtual void OnEnable()
	{

	}

	/// <summary>
	/// Called when the attack object is disabled.
	/// </summary>
	public virtual void OnDisable()
	{

	}

	/// <summary>
	/// Sets the 'force' of the attack. This means what color it has, or what team it belongs to.
	/// </summary>
	/// <param name="_force">The force value.</param>
	public void SetForce( Properties.ForceType _force )
	{
		forceType = _force;
		for ( int i = 0; i < boxColliders.Length; i++ )
		{
			boxColliders[i].SetForce( _force );
		}
	}

	/// <summary>
	/// Get a float for use in the FSM.
	/// </summary>
	/// <returns>Return a float that is used in the FSM to block state switching.</returns>
	public float GetStateBlockTime()
	{
		return startupTime + duration + cooldownTime;
	}

	/// <summary>
	/// Handle the input appropriately for each particular attack if necessary.
	/// </summary>
	/// <param name="_input">The input handler reference of the actor.</param>
	public virtual void HandleInput( InputHandler _input )
	{
		if ( m_dashAvailable )
		{
			if ( _input.LeftStick().x != 0.0f )
			{
				Debug.Log( "Dash!" );
			}
		}
	}

	/// <summary>
	/// Called when an AbstractHurtBox makes contact with an enemy. 
	/// </summary>
	public virtual void HitEnemy( Collider2D _collider )
	{
		transform.parent.gameObject.GetComponent<Actor>().StartChildCoroutine( FreezePlayers( _collider ) );

		//Add the attack delay to the blocking timer. Do we want this?
		//transform.parent.gameObject.GetComponent<PlayerFSM>().currentState.AddBlockingTime( m_onHitFreezeDuration );

		Camera.main.GetComponent<CameraShake>().Shake();
		//StartCoroutine( DashWindow( dashWindow ) );
	}

	/// <summary>
	/// The window of time that, when activated, allows the player to perform a dash. 
	/// </summary>
	/// <param name="_time">The window of opportunity in seconds.</param>
	public IEnumerator DashWindow( float _time )
	{
		m_dashAvailable = true;
		yield return new WaitForSeconds( _time );
		m_dashAvailable = false;
	}

	/// <summary>
	/// Called to start the specific attack.
	/// </summary>
	public IEnumerator StartAttack()
	{
		BeforeAttack();
		yield return new WaitForSeconds( startupTime );
		m_duringAttack = true;
		yield return new WaitForSeconds( duration );
		AfterAttack();
		m_duringAttack = false;
		yield return new WaitForSeconds( cooldownTime );
	}

	/// <summary>
	/// Called before the attack starts, during the startup period.
	/// </summary>
	public abstract void BeforeAttack();

	/// <summary>
	/// Called during the attack, after the startup period and before the
	/// cooldown period. 
	/// </summary>
	public abstract void DuringAttack();

	/// <summary>
	/// Called after the attack, during the cooldown period.
	/// </summary>
	public abstract void AfterAttack();

	/// <summary>
	/// Freeze the players for a set amount of time in their animator before resuming it. This is going to contribute to the 
	/// feeling of force applied by an attack. 
	/// </summary>
	/// <param name="_collider"></param>
	/// <returns></returns>
	/// 
	//REFACTOR
	public IEnumerator FreezePlayers( Collider2D _collider )
	{
		_collider.gameObject.GetComponent<PlayerFSM>().SetCurrentState( PlayerFSM.States.HIT, true );

		//Freeze Animations
		transform.parent.gameObject.GetComponent<Animator>().speed = 0.0f;
		_collider.GetComponent<Animator>().speed = 0.0f;

		//Freeze Physics
		transform.parent.gameObject.GetComponent<PhysicsController>().PausePhysics( true );
		_collider.gameObject.GetComponent<PhysicsController>().PausePhysics( true );

		//Instantiate Particle Systems -- Separate this.
		Vector3 offSet = new Vector3( 0.0f, 1.0f, 0.0f );
		Instantiate( particleOnHit, _collider.transform.localPosition + offSet, Quaternion.identity ); //Fix this to one shot. Firing off multiple times.

		yield return new WaitForSeconds( m_onHitFreezeDuration );
	
		ApplyForce( _collider );

		transform.parent.gameObject.GetComponent<PhysicsController>().PausePhysics( false );
		_collider.gameObject.GetComponent<PhysicsController>().PausePhysics( false );

		_collider.GetComponent<Animator>().speed = 1.0f;
		transform.parent.gameObject.GetComponent<Animator>().speed = 1.0f;

	}

	/// <summary>
	/// Apply a force to the given collider object along the given direction with the given force.
	/// </summary>
	/// <param name="_collider"> Collider object </param>
	public void ApplyForce( Collider2D _collider )
	{
	
		Vector2 direction = ( _collider.transform.position - this.transform.position ).normalized;

		if ( !hitAlongDistanceVector )
		{
			direction.x *= hitDirection.x;
			direction.y = hitDirection.y;
		}

		_collider.gameObject.GetComponent<PhysicsController>().Velocity = direction * hitForce;
	}
}