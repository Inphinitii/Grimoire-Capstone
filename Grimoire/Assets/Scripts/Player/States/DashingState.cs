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
			Physics2D.IgnoreLayerCollision( LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Player"), true );
			if(m_dashComponent == null)
				m_dashComponent = m_playerFSM.gameObject.GetComponent<Dash>();


			m_dashComponent.StartDash( m_playerFSM.GetInput().LeftStick() );
			BlockStateSwitch( m_dashComponent.dashDuration );

		}

		public override void OnExit()
		{
			Physics2D.IgnoreLayerCollision( LayerMask.NameToLayer( "Player" ), LayerMask.NameToLayer( "Player" ), false );
			base.OnExit();
		}

		public override void ExecuteState()
		{
			Debug.Log( "Dashing" );
		}

		public override void ExitConditions()
		{
			if ( m_dashComponent.DashComplete() || m_playerFSM.GetInput().Triggers().thisFrame > 0.5f && m_playerFSM.GetInput().Triggers().lastFrame < 0.5f )
			{
				m_dashComponent.ForceCompletion = true;
				m_playerFSM.GoToPreviousState( true );
			}

			if ( GetFSM().GetActorReference().GetInputHandler().Jump().thisFrame )
				GetFSM().SetCurrentState( PlayerFSM.States.JUMPING, true );
		}
	}
}

