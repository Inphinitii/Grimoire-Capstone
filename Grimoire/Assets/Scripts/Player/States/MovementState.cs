using UnityEngine;
using System.Collections;

namespace PlayerStates
{
	public class MovementState : IState
	{
		public MovementState()
		{
		}

		public override void ExecuteState()
		{
			if ( GetFSM().GetActorReference().GetInputHandler().Y() )
				Debug.Log( "Dash Attack" );
			if ( GetFSM().GetActorReference().GetInputHandler().A() )
				GetFSM().SetCurrentState( PlayerFSM.States.JUMPING, false );



			Vector2 _leftStick = GetFSM().GetActorReference().GetInputHandler().LeftStick();
			if ( _leftStick.x != 0 )
				GetFSM().GetActorReference().GetMovementController().MoveX( _leftStick );
			if ( _leftStick.y < 0 )
				GetFSM().SetCurrentState( PlayerFSM.States.CROUCHING, true );
			if( _leftStick.x == 0)
				GetFSM().SetCurrentState( PlayerFSM.States.STANDING, false );
			
		}
	}
}

