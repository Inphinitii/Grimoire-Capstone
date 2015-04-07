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
			m_dashComponent = m_playerFSM.gameObject.GetComponent<Dash>();
			m_dashComponent.StartCoroutine( m_dashComponent.StartDash( m_playerFSM.GetInput().LeftStick() ) );
			BlockStateSwitch( m_dashComponent.dashDuration / 2.0f );
			base.OnSwitch();
		}

		public override void OnExit()
		{
			base.OnExit();
		}

		public override void ExecuteState()
		{
			if(m_dashComponent.DashComplete() || m_playerFSM.GetInput().Triggers() > 0.0f)
			{
				m_playerFSM.GoToPreviousState(true);
				//Change to the appropriate state. 
			}
		}
	}
}

