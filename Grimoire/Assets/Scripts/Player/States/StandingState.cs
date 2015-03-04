using UnityEngine;
using System.Collections;

public class StandingState : IState {
    public StandingState()
    {
    }

    public override void ExecuteState()
    {
        Vector2 _leftStick = GetFSM().GetInput().LeftStick();

            //Forward Attack
            if (GetFSM().GetInput().Y())
            {
				if(_leftStick.x != 0)
				{
	                GetFSM().StartCoroutine(BlockStateSwitch(1.25f));
	                GetFSM().GetMovementController().OrientationCheck(_leftStick);
	                Debug.Log("Forward Standing Attack");
                }
                if(_leftStick.y > 0)
                {
					GetFSM().StartCoroutine(BlockStateSwitch(1.25f)); //Attack Delay
					Debug.Log("Up Standing Attack");
                }
                else
                {
					GetFSM().StartCoroutine(BlockStateSwitch(1.25f));
					Debug.Log("Neutral Standing Attack");
				}
            }
            else
            {
                GetFSM().StartCoroutine(SwitchStates(_leftStick));
            }
    }

    IEnumerator SwitchStates(Vector2 _leftStick)
    {
        yield return new WaitForSeconds(0.05f);
        if (_leftStick.x > 0 || _leftStick.x < 0)
            GetFSM().SetCurrentState(PlayerFSM.States.MOVING);
        if (_leftStick.y < 0)
            GetFSM().SetCurrentState(PlayerFSM.States.CROUCHING);
        if (GetFSM().GetInput().A())
            GetFSM().SetCurrentState(PlayerFSM.States.JUMPING);
    }
}
