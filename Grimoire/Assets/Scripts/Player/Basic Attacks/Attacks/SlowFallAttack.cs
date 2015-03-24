using UnityEngine;
using System.Collections;

/*========================================================
 * Author: Tyler Remazki
 *
 * Class : Slow Fall Attack
 *
 * Description: Slow fall attack that uses the virtual implementation within the AbstractAttack. 
 =========================================================*/

//NEEDS FIXING
public class SlowFallAttack : AbstractAttack
{
	private float m_defaultGravity;
	private PhysicsController m_physicsController;
	private const float ATTACK_GRAVITY = 1.0f;

	// Use this for initialization
	public override void Start()
	{
		base.Start();
		m_defaultGravity = transform.parent.GetComponent<Actor>().GetPhysicsController().p_gravitationalForce;
		m_physicsController = transform.parent.GetComponent<Actor>().GetPhysicsController();
	}

	// Update is called once per frame
	public override void Update()
	{
		base.Update();
	}

	public override void HandleInput( InputHandler _input )
	{
		base.HandleInput( _input );
	}

	public override void HitEnemy( Collider2D _collider )
	{
		base.HitEnemy( _collider );
	}

	public override void BeforeAttack()
	{
		//throw new System.NotImplementedException();
		m_physicsController.ClearValues();
	}

	public override void DuringAttack()
	{
		for ( int i = 0; i < m_childHurtBoxes.Length; i++ )
		{
			m_childHurtBoxes[i].EnableHurtBox();
		}
		//transform.parent.GetComponent<Actor>().GetPhysicsController().PausePhysics( true );
		m_physicsController.p_gravitationalForce = ATTACK_GRAVITY;

	}

	public override void AfterAttack()
	{
		//transform.parent.GetComponent<Actor>().GetPhysicsController().PausePhysics( false );
		m_physicsController.p_gravitationalForce = m_defaultGravity;
		for ( int i = 0; i < m_childHurtBoxes.Length; i++ )
		{
			m_childHurtBoxes[i].DisableHurtBox();
		}
	}
}
