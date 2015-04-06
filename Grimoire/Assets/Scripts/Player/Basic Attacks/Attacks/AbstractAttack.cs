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
	//Add a way of altering the way that the player reacts when they're hit
	//Interface of an OnHit would work well. Call it from within this class.
	public AbstractHurtBox[]		boxColliders;
	public Properties.ForceType	forceType;

	public ParticleSystem			particleOnHit;
	public ParticleSystem			particleOnUse;

	public float duration;
	public float startupTime;
	public float cooldownTime;

	public bool ableToCancel;

	public Vector2	hitDirection;
	public bool		hitAlongDistanceVector;
	public float		hitForce;
	public int			hitDamage;
	public bool		staticForce;
	public bool		freezeMovementOnUse;

	public float	groundKnockBack;
	public float	airKnockBack;

	protected AbstractHurtBox[]	m_childHurtBoxes;
	protected bool							m_duringAttack;

	/// <summary>
	/// Called to start the specific attack.
	/// </summary>
	public virtual IEnumerator StartAttack()
	{
		BeforeAttack();
		yield return new WaitForSeconds( startupTime );
		m_duringAttack = true;
		yield return new WaitForSeconds( duration );
		m_duringAttack = false;
		AfterAttack();
		yield return new WaitForSeconds( cooldownTime );
	}

	/// <summary>
	/// Called when the object with this Monobehaviour attached to it
	/// is initially created
	/// </summary>
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

	/// <summary>
	/// Called when the object with this Monobehaviour is being updated.
	/// </summary>
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
	/// Handle the input appropriately for each particular attack if necessary.
	/// </summary>
	/// <param name="_input">The input handler reference of the actor.</param>
	public virtual void HandleInput( InputHandler _input )
	{
	}

	/// <summary>
	/// Called when an AbstractHurtBox makes contact with an enemy. 
	/// </summary>
	public virtual void HitEnemy( Collider2D _collider )
	{
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
	/// Apply a force to the given collider object along the given direction with the given force.
	/// </summary>
	/// <param name="_collider"> Collider object </param>
	public void ApplyForce( Collider2D _collider )
	{
	
		Vector2 direction = ( _collider.transform.position - this.transform.position ).normalized;

		if ( !hitAlongDistanceVector )
		{
			direction.x *= hitDirection.x;
			direction.y   = hitDirection.y;
		}

		_collider.gameObject.GetComponent<PhysicsController>().Velocity = direction * hitForce;
		transform.parent.gameObject.GetComponent<PhysicsController>().Velocity = -direction * groundKnockBack;
	}

	/// <summary>
	/// Sets the 'force' of the attack. This means what color it has, or what team it belongs to.
	/// </summary>
	/// <param name="_force">The force value.</param>
	public void SetForce( Properties.ForceType _force )
	{
		forceType = _force;
		for ( int i = 0; i < m_childHurtBoxes.Length; i++ )
		{
			m_childHurtBoxes[i].SetForce( _force );
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
}