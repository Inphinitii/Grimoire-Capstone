using UnityEngine;
using System.Collections;

/*========================================================
 * Author: Tyler Remazki
 *
 * Class : Default Attack
 *
 * Description: Default attack that uses the virtual implementation within the AbstractAttack. 
 =========================================================*/

public class StraightFallAttack : AbstractAttack
{
	public float fallSpeed = 5000f;
	public GameObject spellAnimation;
	private float m_onHitFreezeDuration = 0.25f;
	GameObject _temp;

	public override IEnumerator StartAttack()
	{
		BeforeAttack();
		yield return new WaitForSeconds( Startup() );
		m_duringAttack = true;
	}

	public override void Start()
	{
		base.Start();
	}

	public override void Update()
	{
		base.Update();
	}

	public override void HandleInput( InputHandler _input )
	{
		base.HandleInput( _input );
	}

	public override void HitEnemy( Collider2D _collider)
	{
		transform.parent.gameObject.GetComponent<Actor>().StartChildCoroutine( FreezePlayers( _collider ) );
		Camera.main.GetComponent<CameraShake>().Shake();

		//Add the attack delay to the blocking timer. Do we want this?
		transform.parent.gameObject.GetComponent<PlayerFSM>().AddBlockingTime( OnHitCooldown() );
		base.HitEnemy( _collider );
	}

	public override void BeforeAttack()
	{
		transform.parent.gameObject.GetComponent<PhysicsController>().PausePhysics( true );
		m_parentActor.GetPhysicsController().applyGravity = false;
		m_parentActor.GetPhysicsController().ClearValues();
	}

	public override void DuringAttack()
	{
		transform.parent.gameObject.GetComponent<PhysicsController>().PausePhysics( false );
		if ( m_parentActor.GetMovementController().IsJumping() )
		{
			m_parentActor.GetMovementController().m_capAcceleration = false;
			m_parentActor.GetPhysicsController().Forces = -Vector2.up * fallSpeed;
		}
		else
		{
			Camera.main.GetComponent<CameraShake>().Shake();
			if(m_parentActor.GetMovementController().IsOnGround())
				_temp = (GameObject)Instantiate( spellAnimation, m_parentActor.gameObject.transform.position, Quaternion.identity );
			m_parentActor.GetMovementController().m_capAcceleration = true;
			m_parentActor.GetPhysicsController().applyGravity = true;
			m_duringAttack = false;

			for ( int i = 0; i < m_childHurtBoxes.Length; i++ )
			{
				m_childHurtBoxes[i].EnableHurtBox();
			}

			StartCoroutine( DelayAfterAttack() );
		}
	}

	public override void AfterAttack()
	{
		for ( int i = 0; i < m_childHurtBoxes.Length; i++ )
		{
			m_childHurtBoxes[i].DisableHurtBox();
		}
		m_exitFlag = true;
	}

	IEnumerator DelayAfterAttack()
	{
		if ( _temp != null )
		{
			_temp.GetComponent<Animator>().SetBool( "StartSpell", true );
			_temp.GetComponent<Animator>().SetBool( "StartSpell", false );
		}
		yield return new WaitForSeconds( 0.10f );
		if ( _temp != null )
		{
			_temp.GetComponent<Animator>().SetBool( "Explode", true );
			_temp.GetComponent<Animator>().SetBool( "Explode", false );
		}
		
		AfterAttack();
	}
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

		//Freeze Animations
		transform.parent.gameObject.GetComponent<Animator>().speed = 0.0f;
		_collider.GetComponent<Animator>().speed = 0.0f;

		//Freeze Physics

		//_collider.gameObject.GetComponent<PlayerFSM>().SetCurrentState( PlayerFSM.States.HIT, true );

		//Instantiate Particle Systems -- Separate this.
		Vector3 offSet = new Vector3( 0.0f, 1.0f, 0.0f );
		Instantiate( particleOnHit, _collider.transform.localPosition + offSet, Quaternion.identity );

		yield return new WaitForSeconds( m_onHitFreezeDuration );
		_collider.gameObject.GetComponent<PlayerFSM>().SetCurrentState( PlayerFSM.States.HIT, true );
		ApplyForce( _collider );

		_collider.GetComponent<Animator>().speed = 1.0f;
		transform.parent.gameObject.GetComponent<Animator>().speed = 1.0f;

	}

}
