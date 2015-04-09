using UnityEngine;
using System.Collections;

namespace PlayerStates
{
	public class MovementState : IState
	{
		Vector2 m_leftStick;
		public MovementState()
		{
		}

		public override void ExecuteState()
		{
			m_leftStick = GetFSM().GetActorReference().GetInputHandler().LeftStick();

			if ( m_leftStick.x != 0.0f )
				GetFSM().GetActorReference().GetMovementController().MoveX( m_leftStick );

			if ( GetFSM().GetInput().Attack().thisFrame )
			{
				Debug.Log( "Dash Attack" );
			}

			
		}

		public override void ExitConditions()
		{
			if ( GetFSM().GetInput().Triggers().thisFrame > 0.5f && GetFSM().GetInput().Triggers().lastFrame < 0.5f )
				if ( GetFSM().GetActorReference().GetSpellCharges().UseCharge() )
						GetFSM().SetCurrentState( PlayerFSM.States.DASHING, true );

			if ( GetFSM().GetActorReference().GetInputHandler().Jump().thisFrame )
				GetFSM().SetCurrentState( PlayerFSM.States.JUMPING, false );

			if ( GetFSM().GetActorReference().GetMovementController().IsJumping() )
				GetFSM().SetCurrentState( PlayerFSM.States.JUMPING, true );

			if ( m_leftStick.y < 0 )
				GetFSM().SetCurrentState( PlayerFSM.States.CROUCHING, true );
			if ( m_leftStick.x == 0 )
				GetFSM().SetCurrentState( PlayerFSM.States.STANDING, false );
		}
	}
}

