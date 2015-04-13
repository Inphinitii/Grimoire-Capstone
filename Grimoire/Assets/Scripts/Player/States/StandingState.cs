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
			if ( GetFSM().GetInput().Special().thisFrame && !GetFSM().GetInput().Special().lastFrame )
			{
				float _recharge = GetFSM().GetActorReference().GetGrimoire().GetRefreshRate();
				if ( GetFSM().GetActorReference().GetSpellCharges().UseCharge( _recharge ) )
				{
					GetFSM().CurrentAttack = GetFSM().GetActorReference().GetGrimoire().UseCurrentPage( Page.Type.STANDING_NEUTRAL );
					GetFSM().SetCurrentState( PlayerFSM.States.ATTACKING, false );
				}

			}

			if ( GetFSM().GetInput().Attack().thisFrame  )
			{
				//Directional Attack - Standing
				if ( _leftStick.x != 0 )
					GetFSM().GetMovement().OrientationCheck( _leftStick );
				//Upwards Attack - Standing
				if ( _leftStick.y > 0 )
				{
					GetFSM().CurrentAttack = GetFSM().GetAttackList().GetAttack( BasicAttacks.Attacks.STANDING_AIR );
					GetFSM().SetCurrentState( PlayerFSM.States.ATTACKING, false );
				}
				//Neutral Attack - Standing
				else
				{
					GetFSM().CurrentAttack = GetFSM().GetAttackList().GetAttack( BasicAttacks.Attacks.STANDING_NEUTRAL );
					GetFSM().SetCurrentState( PlayerFSM.States.ATTACKING, false );
				}
			}
		}

		public override void ExitConditions()
		{
			if ( _leftStick.x != 0 )
			{
				if ( GetFSM().GetInput().LeftStick().y < 0.1f )
					if ( GetFSM().GetInput().Triggers().thisFrame > 0.5f && GetFSM().GetInput().Triggers().lastFrame < 0.5f)
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
