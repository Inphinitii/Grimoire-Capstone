using UnityEngine;
using System.Collections;

public class JumpingState : IState {
	bool lastJump;
	bool getJump;

    public JumpingState()
    {
    }

    public override void OnSwitch(){
		GetFSM().GetMovement().ApplyJump(true, Vector2.zero);
    }

    public override void ExecuteState()
    {
        /* Redundant Code */
		Vector2 _leftStick = GetFSM().GetInput().LeftStick();
        if (_leftStick.x > 0 || _leftStick.x < 0)
            GetFSM().GetMovement().MoveX(_leftStick);
        ///////////////////
		lastJump = getJump;
		getJump = GetFSM().GetInput().A();
		if ( getJump )
		{
			GetFSM().GetMovement().ApplyJump( true, _leftStick );
		}
		else if (getJump != lastJump)
		{
			GetFSM().GetMovement().ApplyJump( false, _leftStick );
		}

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
