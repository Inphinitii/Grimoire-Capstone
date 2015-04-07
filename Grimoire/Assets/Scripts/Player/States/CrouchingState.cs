using UnityEngine;
using System.Collections;

namespace PlayerStates
{
	public class CrouchingState : IState
	{
		Vector2 m_leftStick;

		public CrouchingState()
		{
		}

		public override void ExecuteState()
		{
			m_leftStick = GetFSM().GetInput().LeftStick();

			if ( GetFSM().GetInput().Attack().thisFrame )
			{
				if ( m_leftStick.y < 0.0f )
					m_playerFSM.CurrentAttack = GetFSM().GetAttackList( "Basic Attacks" ).GetAttack( "CrouchingAttack" );

				GetFSM().SetCurrentState( PlayerFSM.States.ATTACKING, false );
			}

		}

		public override void ExitConditions()
		{
			if ( m_leftStick.y >= 0 )
				GetFSM().SetCurrentState( PlayerFSM.States.STANDING, false );

		}
	}
}
