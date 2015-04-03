using UnityEngine;
using System.Collections;

/*========================================================
 * Author: Tyler Remazki
 *
 * Class : Incantation Attack
 *
 * Description: Defines the behaviour of typical incantation spells.
 * Extends from the Abstract Attack class. 
 =========================================================*/

[RequireComponent(typeof(SpellCharges))]
public class IncantationAttack : AbstractAttack {

	private SpellCharges m_spellChargeReference;

	/// <summary>
	/// Start the incantation. Requires a spell charge.
	/// </summary>
	public override IEnumerator StartAttack()
	{
		if ( m_spellChargeReference.UseCharge() )
			yield return StartCoroutine( base.StartAttack() );
		else
			yield return null;
	}

	public override void Start()
	{
		m_spellChargeReference = transform.parent.gameObject.GetComponent<SpellCharges>();
		base.Start();
	}

	public override void Update()
	{
		base.Update();
	}

	public override void OnEnable()
	{
		base.OnEnable();
	}

	public override void OnDisable()
	{
		base.OnDisable();
	}

	public override void HandleInput( InputHandler _input )
	{
		base.HandleInput( _input );
	}

	public override void BeforeAttack()
	{
		throw new System.NotImplementedException();
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

	public override void HitEnemy( Collider2D _collider )
	{
		base.HitEnemy( _collider );
	}


}
