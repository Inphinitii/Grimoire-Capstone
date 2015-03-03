using UnityEngine;
using System.Collections;

public class StandingState : IState {
    public StandingState()
    {
    }

    public override void ExecuteState()
    {
        Vector2 _leftStick = GetFSM().GetInput().LeftStick();

            //Neutral Attack
            if (GetFSM().GetInput().Y() && _leftStick.x != 0)
            {
                GetFSM().StartCoroutine(BlockStateSwitch(1.25f));
                GetFSM().GetMovementController().OrientationCheck(_leftStick);
                Debug.Log("Forward Standing Attack");
                //Start attack
            }
            else if (GetFSM().GetInput().Y())
            {
                GetFSM().StartCoroutine(BlockStateSwitch(1.25f));
                Debug.Log("Neutral Standing Attack");
                //Start attack
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
