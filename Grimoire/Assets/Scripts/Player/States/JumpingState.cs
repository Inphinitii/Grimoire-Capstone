using UnityEngine;
using System.Collections;

public class JumpingState : IState {
    public JumpingState()
    {
    }

    public override void OnSwitch()
    {
        GetFSM().GetMovementController().ApplyJump();
    }

    public override void ExecuteState()
    {
        /* Redundant Code */
        Vector2 _leftStick = GetFSM().GetInput().LeftStick();
        if (_leftStick.x > 0 || _leftStick.x < 0)
            GetFSM().GetMovementController().MoveX(_leftStick);
        ///////////////////

       if (GetFSM().GetInput().A())
           GetFSM().GetMovementController().ApplyJump();

       if (!GetFSM().GetMovementController().IsJumping() && GetFSM().GetPhysicsController().Velocity.x == 0)
       {
           GetFSM().SetCurrentState(PlayerFSM.States.STANDING);
       }
       else if (!GetFSM().GetMovementController().IsJumping())
       {
           GetFSM().SetCurrentState(PlayerFSM.States.MOVING);
       }
           
 
    }
}
