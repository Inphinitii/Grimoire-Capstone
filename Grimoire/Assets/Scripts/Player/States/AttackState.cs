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

		public override void OnExit()
		{
			m_playerFSM.CurrentAttack = null;
		}

		public override void OnSwitch()
		{
			m_time					= GetFSM().CurrentAttack.GetStateBlockTime();
			m_attackReference	= GetFSM().CurrentAttack;
			m_attackStart			= true;
		}
		public override void ExecuteState()
		{
			GetFSM().BlockStateSwitch( m_attackReference.GetStateBlockTime() );
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
			if ( m_attackReference.GetExitFlag() || m_time < 0.0f )
			{
				m_attackReference.SetExitFlag( false );

				if(GetFSM().GetMovement().IsJumping())
					GetFSM().SetCurrentState( PlayerFSM.States.JUMPING, true );
				if (!GetFSM().GetMovement().IsJumping() && GetFSM().GetInput().LeftStick().x == 0.0f )
					GetFSM().SetCurrentState( PlayerFSM.States.STANDING, true );
				if ( !GetFSM().GetMovement().IsJumping() && GetFSM().GetInput().LeftStick().x != 0.0f )
					GetFSM().SetCurrentState( PlayerFSM.States.MOVING, true );
				//GetFSM().GoToPreviousState( true );
			}

			
		}
	}
}