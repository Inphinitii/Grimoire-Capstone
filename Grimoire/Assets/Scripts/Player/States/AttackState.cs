using UnityEngine;
using System.Collections;

namespace PlayerStates
{
	public class AttackState : IState
	{

		private bool			m_attackStart;
		private float			m_time;
		private AbstractAttack	m_attackReference;

		public AttackState()
		{
		}

		public override void OnSwitch()
		{
			m_time					= GetFSM().CurrentAttack.attackRef.GetStateBlockTime();
			m_attackReference	= GetFSM().CurrentAttack.attackRef;
			m_attackStart			= true;
		}
		public override void ExecuteState()
		{
			BlockStateSwitch( m_attackReference.GetStateBlockTime() );
			if ( m_time > 0.0f )
			{
				if ( m_attackStart )
				{
					GetFSM().StartCoroutine( m_attackReference.StartAttack() );
					m_attackStart = false;
				}
				m_attackReference.HandleInput( GetFSM().GetActorReference().GetInputHandler() );
				m_time -= Time.deltaTime;
			}
		}

		public override void ExitConditions()
		{
			if(m_time <= 0.0f)
				GetFSM().GoToPreviousState( false );
		}
	}
}