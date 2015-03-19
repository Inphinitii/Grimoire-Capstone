using UnityEngine;
using System.Collections;

public class AnimatedAttack : AbstractAttack {
	public float[] hurtBoxTimes;

	private float m_timer;

	// Use this for initialization
	public override void Start()
	{
		base.Start();
	}
	
	// Update is called once per frame
	public override void Update () 
	{
		if ( m_duringAttack )
		{
			DuringAttack();
		}
	}

	public override void OnEnable() 
	{
		base.OnEnable();
	}

	public override void OnDisable() 
	{
		base.OnDisable();
	}

	public override void BeforeAttack()
	{
		m_timer = 0.0f;
		base.BeforeAttack();
	}

	/// <summary>
	/// FIX ME
	/// </summary>
	public override void DuringAttack()
	{
		Debug.Log( "Ding" );
		m_timer += Time.deltaTime;
		for ( int i = 0; i < hurtBoxTimes.Length; i++ )
		{
			if ( m_timer >= hurtBoxTimes[i] )
			{
				if ( i - 1 >= 0 )
				{
					m_childHurtBoxes[i - 1].DisableHurtBox();
				}
				m_childHurtBoxes[i].EnableHurtBox();
				break;
			}
		}
	}

	public override void AfterAttack()
	{
		//m_childHurtBoxes[hurtBoxTimes.Length - 1].DisableHurtBox(); //Ensure the final hurtbox is disabled.
		base.AfterAttack();
	}

	
}
