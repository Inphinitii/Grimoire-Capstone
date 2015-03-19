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

	// Use this for initialization
	public override void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	public override void Update()
	{
		base.Update();
	}

	public override void HandleInput (InputHandler _input)
	{
		base.HandleInput( _input );
	}

	public override void HitEnemy(Collider2D _collider)
	{
		base.HitEnemy( _collider );
	}

	public override void BeforeAttack()
	{
		//throw new System.NotImplementedException();
	}

	public override void DuringAttack()
	{
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
}
