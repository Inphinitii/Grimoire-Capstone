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

	public override void HitEnemy(Collider2D _collider)
	{
		transform.parent.gameObject.GetComponent<Actor>().StartChildCoroutine( FreezePlayers( _collider ) );
		Camera.main.GetComponent<CameraShake>().Shake();

		//Add the attack delay to the blocking timer. Do we want this?
		transform.parent.gameObject.GetComponent<PlayerFSM>().currentState.AddBlockingTime( OnHitCooldown() );

		//StartCoroutine( DashWindow( dashWindow ) );
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
	/// 
	//REFACTOR
	public IEnumerator FreezePlayers( Collider2D _collider )
	{

		//Freeze Animations
		transform.parent.gameObject.GetComponent<Animator>().speed = 0.0f;
		_collider.GetComponent<Animator>().speed									= 0.0f;

		//Freeze Physics
		transform.parent.gameObject.GetComponent<PhysicsController>().PausePhysics( true );

		_collider.gameObject.GetComponent<PhysicsController>().PausePhysics( true );
		_collider.gameObject.GetComponent<PlayerFSM>().SetCurrentState( PlayerFSM.States.HIT, true );

		//Instantiate Particle Systems -- Separate this.
		Vector3 offSet = new Vector3( 0.0f, 1.0f, 0.0f );
		Instantiate( particleOnHit, _collider.transform.localPosition + offSet, Quaternion.identity ); 

		yield return new WaitForSeconds( m_onHitFreezeDuration );

		ApplyForce( _collider );

		transform.parent.gameObject.GetComponent<PhysicsController>().PausePhysics( false );
		_collider.gameObject.GetComponent<PhysicsController>().PausePhysics( false );

		_collider.GetComponent<Animator>().speed = 1.0f;
		transform.parent.gameObject.GetComponent<Animator>().speed = 1.0f;

	}

	/*
	 * 	/// <summary>
	/// The window of time that, when activated, allows the player to perform a dash. 
	/// </summary>
	/// <param name="_time">The window of opportunity in seconds.</param>
	public IEnumerator DashWindow( float _time )
	{
		m_dashAvailable = true;
		yield return new WaitForSeconds( _time );
		m_dashAvailable = false;
	}
	 */
}
