using UnityEngine;
using System.Collections;

namespace PlayerStates
{
	public class DashingState : IState
	{
		private Dash m_dashComponent;

		public DashingState()
		{
			
		}

		public override void OnSwitch()
		{
			GetFSM().StartChildCoroutine( DashInvulnerability() );
			Physics2D.IgnoreLayerCollision( LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Player"), true );
			if(m_dashComponent == null)
				m_dashComponent = m_playerFSM.gameObject.GetComponent<Dash>();

			m_dashComponent.StartDash( m_playerFSM.GetInput().LeftStick() );
			GetFSM().BlockStateSwitch( m_dashComponent.dashDuration );

		}

		public override void OnExit()
		{
			Physics2D.IgnoreLayerCollision( LayerMask.NameToLayer( "Player" ), LayerMask.NameToLayer( "Player" ), false );
			base.OnExit();
		}

		public override void ExecuteState()
		{
		}

		public override void ExitConditions()
		{
			if ( GetFSM().GetActorReference().GetInputHandler().Jump().thisFrame )
				GetFSM().SetCurrentState( PlayerFSM.States.JUMPING, true );

			if ( m_dashComponent.DashComplete() || m_playerFSM.GetInput().Triggers().thisFrame > 0.5f && m_playerFSM.GetInput().Triggers().lastFrame < 0.5f )
			{
				m_dashComponent.ForceCompletion = true;
				GetFSM().GoToPreviousState( true , 1);
			}

		}

		IEnumerator DashInvulnerability()
		{
			GetFSM().GetActorReference().SetInvulnerable(true);
			yield return new WaitForSeconds( 0.16f );
			GetFSM().GetActorReference().SetInvulnerable(false);
		}
	}
}

