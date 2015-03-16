using UnityEngine;
using System.Collections;

public class MovementState : IState {
    public MovementState(){
    }

    public override void ExecuteState()
    {
       if (GetFSM().GetActorReference().GetInputHandler().Y())
           Debug.Log("Dash Attack");
	   if ( GetFSM().GetActorReference().GetInputHandler().A() )
           GetFSM().SetCurrentState(PlayerFSM.States.JUMPING, false);



	   Vector2 _leftStick = GetFSM().GetActorReference().GetInputHandler().LeftStick();
	   if ( _leftStick.x != 0 )
		   GetFSM().GetActorReference().GetMovementController().MoveX( _leftStick );
	   else
		   GetFSM().SetCurrentState( PlayerFSM.States.STANDING, false );
    }
}

