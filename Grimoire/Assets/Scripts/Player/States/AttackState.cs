using UnityEngine;
using System.Collections;

namespace PlayerStates
{
	public class AttackState : IState
	{

		private AbstractAttack		m_attackReference;
		private float						m_time;
		private bool						m_attackStart;
		private bool						m_noTimer;

		public AttackState()
		{
		}

		public override void OnExit()
		{

			if ( m_playerFSM.CurrentAttack != null )
				m_playerFSM.CurrentAttack.SetExitFlag( false );

			m_playerFSM.CurrentAttack = null;
			m_time		= 0.0f;
			m_noTimer	= false;

			GetFSM().GetComponent<Animator>().SetBool( "Attacking", false );
			GetFSM().GetComponent<Animator>().SetBool( "Casting", false );


		}

		public override void OnForcedExit()
		{
			if ( m_playerFSM.CurrentAttack != null )
			{
				//m_playerFSM.CurrentAttack.ForcedExitCleanup();
				m_playerFSM.CurrentAttack.SetExitFlag( false );
			}
		}


		public override void OnSwitch()
		{
			GetFSM().GetComponent<Animator>().SetBool( "Attacking", true );
			if ( GetFSM().CurrentAttack != null )
			{
				m_time = GetFSM().CurrentAttack.GetStateBlockTime();
				m_attackReference = GetFSM().CurrentAttack;
				m_attackStart = true;

				if ( m_time == 0.0f )
					m_noTimer = true;
				else
					GetFSM().BlockStateSwitch( m_time );

			}
			else
				GetFSM().GoToPreviousState( true );
		}
		public override void ExecuteState()
		{
			if ( m_time > 0.0f || m_noTimer)
			{
				if ( m_attackStart )
				{
					GetFSM().StartCoroutine( m_attackReference.StartAttack() );
					m_attackStart = false;
				}
				m_attackReference.HandleInput( GetFSM().GetActorReference().GetInputHandler() );

				if(!m_noTimer)
					m_time -= Time.deltaTime;
			}
		}

		public override void ExitConditions()
		{
			if ( m_attackReference.GetExitFlag() || m_time < 0.0f )
			{
				GetFSM().GoToPreviousState( false );


				//if ( GetFSM().GetMovement().IsJumping() )
				//{
				//	GetFSM().SetCurrentState( PlayerFSM.States.JUMPING, false );
				//}
				//if ( !GetFSM().GetMovement().IsJumping() && GetFSM().GetInput().LeftStick().x == 0.0f )
				//{
				//	GetFSM().SetCurrentState( PlayerFSM.States.STANDING, false );
				//}
				//if ( !GetFSM().GetMovement().IsJumping() && GetFSM().GetInput().LeftStick().x != 0.0f )
				//{
				//	GetFSM().SetCurrentState( PlayerFSM.States.MOVING, false );
				//}

			}

			
		}
	}
}