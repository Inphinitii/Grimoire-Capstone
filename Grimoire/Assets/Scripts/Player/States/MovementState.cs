using UnityEngine;
using System.Collections;

public class MovementState : IState {
    public MovementState(){
    }

    public override void ExecuteState()
    {
       if (GetFSM().GetInput().Y())
           Debug.Log("Dash Attack");
       if (GetFSM().GetInput().A())
           GetFSM().SetCurrentState(PlayerFSM.States.JUMPING);



       Vector2 _leftStick = GetFSM().GetInput().LeftStick();
       if (_leftStick.x > 0 || _leftStick.x < 0)
           GetFSM().GetMovementController().MoveX(_leftStick);
       else
       {
           GetFSM().SetCurrentState(PlayerFSM.States.STANDING);
       }
    }
}


