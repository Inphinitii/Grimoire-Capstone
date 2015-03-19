using UnityEngine;
using System.Collections;

public class AttackState : IState {

	private float m_time;
	private bool attackStart;

	public AttackState()
	{
	}

	public override void OnSwitch()
	{
		m_time = GetFSM().CurrentAttack.GetStateBlockTime();
		attackStart = true;
	}
	public override void ExecuteState()
	{
		if ( m_time > 0.0f )
		{
			if ( attackStart )
			{
				GetFSM().StartCoroutine( BlockStateSwitch( GetFSM().CurrentAttack.GetStateBlockTime() ) );
				GetFSM().StartCoroutine( GetFSM().CurrentAttack.StartAttack() );
				attackStart = false;
			}
			GetFSM().CurrentAttack.HandleInput( GetFSM().GetActorReference().GetInputHandler() );
			m_time -= Time.deltaTime;
		}
		else
		{
			GetFSM().GoToPreviousState( false );
		}
		
	}
}