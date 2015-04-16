using UnityEngine;
using System.Collections;

/*========================================================
 * Author: Tyler Remazki
 *
 * Class : Default Attack
 *
 * Description: Default attack that uses the virtual implementation within the AbstractAttack. 
 =========================================================*/

public class DefaultAttack : AbstractAttack {

	public bool	allowMovement;
	private float m_onHitFreezeDuration = 0.25f;
	

	public override void Start () {
		base.Start();
	}
	
	public override void Update()
	{
		base.Update();
	}

	public override void HandleInput (InputHandler _input)
	{
		base.HandleInput( _input );
		if ( allowMovement )
		{
			m_parentActor.GetMovementController().MoveX( _input.LeftStick() );
		}

	}

	public override void HitEnemy( Collider2D _collider )
	{
		if ( !_collider.gameObject.GetComponent<Actor>().GetInvulnerable() )
		{
			transform.parent.gameObject.GetComponent<Actor>().StartChildCoroutine( FreezePlayers( _collider ) );
			Camera.main.GetComponent<CameraShake>().Shake();

			//Add the attack delay to the blocking timer. Do we want this?
		//	transform.parent.gameObject.GetComponent<PlayerFSM>().AddBlockingTime( OnHitCooldown() );
			base.HitEnemy( _collider );
		}
	}

	public override void BeforeAttack()
	{
		//throw new System.NotImplementedException();
	}

	public override void DuringAttack()
	{
		if ( freezeMovementOnUse )
			transform.parent.gameObject.GetComponent<PhysicsController>().ClearValues();

		for ( int i = 0; i < m_childHurtBoxes.Length; i++ )
		{
			m_childHurtBoxes[i].EnableHurtBox();
		}
	}

	public override void AfterAttack()
	{
		for ( int i = 0; i < m_childHurtBoxes.Length; i++ )
		{
			m_childHurtBoxes[i].DisableHurtBox();
		}
	}

	/// <summary>
	/// Freeze the players for a set amount of time in their animator before resuming it. This is going to contribute to the 
	/// feeling of force applied by an attack. 
	/// </summary>
	/// <param name="_collider"></param>
	/// <returns></returns>
	public IEnumerator FreezePlayers( Collider2D _collider )
	{
		PhysicsController _thisPhys		= transform.parent.gameObject.GetComponent<PhysicsController>();
		SpellCharges _thisCharge		= transform.parent.gameObject.GetComponent<SpellCharges>();
		Animator _thisAnimator			= transform.parent.gameObject.GetComponent<Animator>();

		PhysicsController _otherPhys	= _collider.gameObject.GetComponent<PhysicsController>();
		SpellCharges _otherCharge		= _collider.gameObject.GetComponent<SpellCharges>();
		Animator _otherAnimator		= _collider.gameObject.GetComponent<Animator>();

		_thisPhys.PausePhysics( true );
		_otherPhys.PausePhysics( true );

		_thisCharge.SetFreezeTimer( true );
		_otherCharge.SetFreezeTimer( true );

		_thisAnimator.speed = 0.0f;
		_otherAnimator.speed = 0.0f;

		//Instantiate Particle Systems
		Vector3 offSet = new Vector3( 0.0f, 1.0f, 0.0f );
		Instantiate( particleOnHit, _collider.transform.localPosition + offSet, Quaternion.identity ); 

		yield return new WaitForSeconds( m_onHitFreezeDuration );
		_collider.gameObject.GetComponent<PlayerFSM>().SetCurrentState( PlayerFSM.States.HIT, true );

		_thisPhys.PausePhysics( false );
		_otherPhys.PausePhysics( false );

		_thisCharge.SetFreezeTimer( false );
		_otherCharge.SetFreezeTimer( false );

		_thisAnimator.speed = 1.0f;
		_otherAnimator.speed = 1.0f;




		ApplyForce( _collider , true);


	}

}
