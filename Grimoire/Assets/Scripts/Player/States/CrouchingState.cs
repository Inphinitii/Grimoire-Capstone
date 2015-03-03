using UnityEngine;
using System.Collections;

public class CrouchingState : IState {
    public CrouchingState()
    {
    }

    public override void ExecuteState()
    {
       Vector2 _leftStick = GetFSM().GetInput().LeftStick();
       if (_leftStick.y >= 0)
           GetFSM().SetCurrentState(PlayerFSM.States.STANDING);

        //If Crouching + Attack button..

    }	
}
