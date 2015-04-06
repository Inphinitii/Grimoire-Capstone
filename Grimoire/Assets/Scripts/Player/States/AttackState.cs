using UnityEngine;
using System.Collections;

namespace PlayerStates
{
	public class AttackState : IState
	{

		private float m_time;
		private bool attackStart;

		public AttackState()
		{
		}

		public override void OnSwitch()
		{
			if(GetFSM().CurrentAttack.freezeMovementOnUse)
				GetFSM().GetActorReference().GetPhysicsController().ClearValues();

			m_time = GetFSM().CurrentAttack.GetStateBlockTime();
			attackStart = true;
		}
		public override void ExecuteState()
		{
			BlockStateSwitch( GetFSM().CurrentAttack.GetStateBlockTime() );
			if ( m_time > 0.0f )
			{
				if ( attackStart )
				{
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
}