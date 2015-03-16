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
           GetFSM().SetCurrentState(PlayerFSM.States.STANDING, false);

       if (GetFSM().GetInput().Y())
       {
           AttackList.AttackStruct _temp = GetFSM().GetAttackList( "Basic Attacks" ).GetAttack( "CrouchingAttack" );
           GetFSM().StartCoroutine( BlockStateSwitch( _temp.attackTime ) );
           GetFSM().StartCoroutine( Attack( _temp.attackTime, _temp.attackRef ) );
       }

    }	
}
