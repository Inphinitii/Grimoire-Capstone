using UnityEngine;
using System.Collections;

namespace PlayerStates
{
	public class CrouchingState : IState
	{
		public CrouchingState()
		{
		}

		public override void ExecuteState()
		{
			Vector2 _leftStick = GetFSM().GetInput().LeftStick();

			if ( _leftStick.y >= 0 )
				GetFSM().SetCurrentState( PlayerFSM.States.STANDING, false );

			if ( GetFSM().GetInput().Attack() && _leftStick.y < 0)
			{
				AttackList.AttackStruct _temp = GetFSM().GetAttackList( "Basic Attacks" ).GetAttack( "CrouchingAttack" );
				GetFSM().CurrentAttack = _temp.attackRef;
				GetFSM().SetCurrentState( PlayerFSM.States.ATTACKING, false );
			}

		}
	}
}
