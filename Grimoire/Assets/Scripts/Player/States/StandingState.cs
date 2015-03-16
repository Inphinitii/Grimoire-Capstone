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
					GetFSM().GetMovement().OrientationCheck( _leftStick );
	                Debug.Log("Forward Standing Attack");
                }
                if(_leftStick.y > 0)
                {
					GetFSM().StartCoroutine(BlockStateSwitch(1.25f)); //Attack Delay
					Debug.Log("Up Standing Attack");
                }
                else
                {
                    AttackList.AttackStruct _temp = GetFSM().GetAttackList( "Basic Attacks" ).GetAttack( "CrouchingAttack" );
					GetFSM().StartCoroutine(BlockStateSwitch(_temp.duration + _temp.cooldown));
                    GetFSM().StartCoroutine(Attack(_temp.attackRef));
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
            GetFSM().SetCurrentState(PlayerFSM.States.MOVING, false);
        if (_leftStick.y < 0)
			GetFSM().SetCurrentState( PlayerFSM.States.CROUCHING, false );
        if (GetFSM().GetInput().A())
			GetFSM().SetCurrentState( PlayerFSM.States.JUMPING, false );
    }
}
