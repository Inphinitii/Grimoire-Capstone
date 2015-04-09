using UnityEngine;
using System.Collections;

namespace PlayerStates
{
	public class StandingState : IState
	{
		Vector2 _leftStick;
		public StandingState()
		{
		}

		public override void ExecuteState()
		{
			_leftStick = GetFSM().GetInput().LeftStick();

			if ( GetFSM().GetInput().Attack().thisFrame )
			{
				//Directional Attack - Standing
				if ( _leftStick.x != 0 )
					GetFSM().GetMovement().OrientationCheck( _leftStick );
				//Upwards Attack - Standing
				if ( _leftStick.y > 0 )
				{
					GetFSM().CurrentAttack = GetFSM().GetAttackList( "Basic Attacks" ).GetAttack( "StandingUp" );
					GetFSM().SetCurrentState( PlayerFSM.States.ATTACKING, false );
				}
				//Neutral Attack - Standing
				else
				{
					GetFSM().CurrentAttack = GetFSM().GetAttackList( "Basic Attacks" ).GetAttack( "StandingNeutral" );
					GetFSM().SetCurrentState( PlayerFSM.States.ATTACKING, false );
				}
			}
		}

		public override void ExitConditions()
		{
			if ( _leftStick.x > 0 || _leftStick.x < 0 )
			{
				if ( GetFSM().GetInput().Triggers().thisFrame > 0.0f && GetFSM().GetInput().Triggers().lastFrame <= 0.0f )
					GetFSM().SetCurrentState( PlayerFSM.States.DASHING, true );
				else
					GetFSM().SetCurrentState( PlayerFSM.States.MOVING, false );
			}

			if ( GetFSM().GetInput().Jump().thisFrame )
				GetFSM().SetCurrentState( PlayerFSM.States.JUMPING, false );
			if ( _leftStick.y < 0 )
				GetFSM().SetCurrentState( PlayerFSM.States.CROUCHING, false );
		}
	}
}
