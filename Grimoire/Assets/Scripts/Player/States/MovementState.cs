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

			if ( GetFSM().GetInput().Attack() )
			{
				Debug.Log( "Dash Attack" );
			}

			
		}

		public override void ExitConditions()
		{
			if ( GetFSM().GetActorReference().GetInputHandler().Jump() )
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

