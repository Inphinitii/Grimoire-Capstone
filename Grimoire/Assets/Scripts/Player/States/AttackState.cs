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

		public override void OnPop()
		{
            OnExit();
		}

        public override void OnExit()
        {
            GetFSM().GetComponent<Animator>().SetBool( "Attacking", false );
            GetFSM().GetComponent<Animator>().SetBool( "Casting", false );

            if(m_playerFSM.CurrentAttack != null)
                  m_playerFSM.CurrentAttack.SetExitFlag( false );

            m_playerFSM.CurrentAttack = null;
            m_time = 0.0f;
            m_noTimer = false;
            GetFSM().InterruptBlocking();
        }

		public override void OnForcedExit()
		{
			if ( m_playerFSM.CurrentAttack != null )
			{
				//m_playerFSM.CurrentAttack.ForcedExitCleanup();
                GetFSM().GetComponent<Animator>().SetBool( "Attacking", false );
                GetFSM().GetComponent<Animator>().SetBool( "Casting", false );


				m_playerFSM.CurrentAttack.SetExitFlag( false );
			}
		}


		public override void OnSwitch()
		{
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
				GetFSM().GoToPreviousState( true , 1);
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
				GetFSM().GoToPreviousState( true, 1 );
			}

			
		}
	}
}