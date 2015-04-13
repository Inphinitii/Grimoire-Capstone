using UnityEngine;
using System.Collections;

/*========================================================
 * Author: Tyler Remazki
 *
 * Class : Default Attack
 *
 * Description: Default attack that uses the virtual implementation within the AbstractAttack. 
 =========================================================*/

public class MineAttack : AbstractAttack
{

	public GameObject mineObject;
	public bool				allowMovement;
	private float				m_onHitFreezeDuration = 0.25f;


	public override void Start()
	{
		m_exitFlag			= false;
		m_parentActor	= this.transform.parent.gameObject.GetComponent<Actor>();
	}

	public override void Update()
	{
		base.Update();
	}

	public override IEnumerator StartAttack()
	{
		yield return new WaitForSeconds( Startup() );
		SpawnMineObject();
		yield return new WaitForSeconds( Cooldown() );
	}

	public override void HandleInput( InputHandler _input )
	{
		base.HandleInput( _input );
	}

	public override void AfterAttack()
	{
		throw new System.NotImplementedException();
	}

	public override void DuringAttack()
	{
		throw new System.NotImplementedException();
	}

	public override void BeforeAttack()
	{
		throw new System.NotImplementedException();
	}

	private void SpawnMineObject()
	{
		AbstractHurtBox _temp;
		GameObject _mine = (GameObject)Instantiate( mineObject, m_parentActor.gameObject.transform.position + new Vector3( 0.0f, 1.0f, 0.0f ), Quaternion.identity );

		for ( int i = 0; i < boxColliders.Length; i++ )
		{
			_temp = (AbstractHurtBox)Instantiate( boxColliders[i], this.transform.position, transform.parent.transform.rotation );
			_temp.transform.parent = _mine.transform;
			_temp.SetForce( forceType );
			_temp.EnableHurtBox();
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
		PhysicsController _thisPhys = transform.parent.gameObject.GetComponent<PhysicsController>();
		SpellCharges _thisCharge = transform.parent.gameObject.GetComponent<SpellCharges>();
		Animator _thisAnimator = transform.parent.gameObject.GetComponent<Animator>();

		PhysicsController _otherPhys = _collider.gameObject.GetComponent<PhysicsController>();
		SpellCharges _otherCharge = _collider.gameObject.GetComponent<SpellCharges>();
		Animator _otherAnimator = _collider.gameObject.GetComponent<Animator>();

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

		_thisPhys.PausePhysics( false );
		_otherPhys.PausePhysics( false );

		_thisCharge.SetFreezeTimer( false );
		_otherCharge.SetFreezeTimer( false );

		_thisAnimator.speed = 1.0f;
		_otherAnimator.speed = 1.0f;

		_collider.gameObject.GetComponent<PlayerFSM>().SetCurrentState( PlayerFSM.States.HIT, true );


		ApplyForce( _collider );


	}

}
