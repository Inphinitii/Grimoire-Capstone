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
			if(m_dashComponent == null)
				m_dashComponent = m_playerFSM.gameObject.GetComponent<Dash>();

			m_dashComponent.StartCoroutine( m_dashComponent.StartDash( m_playerFSM.GetInput().LeftStick() ) );
			BlockStateSwitch( m_dashComponent.dashDuration );
		}

		public override void OnExit()
		{
			base.OnExit();
		}

		public override void ExecuteState()
		{
			//Make sure this doesn't register the trigger if it's already down. 
			if ( m_dashComponent.DashComplete() || m_playerFSM.GetInput().Triggers().thisFrame < 1.0f && m_playerFSM.GetInput().Triggers().lastFrame > 0.0f)
				m_playerFSM.GoToPreviousState(true);
		}
	}
}

