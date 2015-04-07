using UnityEngine;
using System.Collections;

namespace PlayerStates
{
	public class BounceState : IState
	{
		public BounceState()
		{
		}

		public override void OnSwitch()
		{
			if ( m_playerFSM.GetInput().Triggers().thisFrame > 0.5f )
			{
				m_playerFSM.StartChildCoroutine(InterruptBounce());
			}
		}

		public override void ExecuteState()
		{
			m_playerFSM.GetActorReference().GetMovementController().groundCheck = true;
		}

		public override void ExitConditions()
		{

		}

		IEnumerator InterruptBounce()
		{
			m_playerFSM.SetCurrentState( PlayerFSM.States.STANDING, true );
			m_playerFSM.GetActorReference().GetPhysicsController().ClearValues();

			m_playerFSM.GetActorReference().GetRenderer().material.color = Color.white;
			yield return new WaitForSeconds( 0.1f );
			m_playerFSM.GetActorReference().GetRenderer().material.color = m_playerFSM.GetActorReference().actorColor;
		}
	}
}

