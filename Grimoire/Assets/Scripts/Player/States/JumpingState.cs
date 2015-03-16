using UnityEngine;
using System.Collections;

public class JumpingState : IState {
    public JumpingState()
    {
    }

    public override void OnSwitch(){
		GetFSM().GetMovement().ApplyJump();
    }

    public override void ExecuteState()
    {
        /* Redundant Code */
		Vector2 _leftStick = GetFSM().GetInput().LeftStick();
        if (_leftStick.x > 0 || _leftStick.x < 0)
            GetFSM().GetMovement().MoveX(_leftStick);
        ///////////////////

		if ( GetFSM().GetInput().A() )
			GetFSM().GetMovement().ApplyJump();
       else if(GetFSM ().GetInput().Y())

	    Debug.Log("Jumping Attack");

       if (!GetFSM().GetMovement().IsJumping() && GetFSM().GetPhysics().Velocity.x == 0)
       {
           GetFSM().SetCurrentState(PlayerFSM.States.STANDING, false);
       }
	   else if ( !GetFSM().GetMovement().IsJumping() )
       {
           GetFSM().SetCurrentState(PlayerFSM.States.MOVING, false);
       }
           
 
    }
}
