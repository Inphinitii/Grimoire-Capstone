using UnityEngine;
using System.Collections;

namespace PlayerStates
{
	public class StandingState : IState
	{
		public StandingState()
		{
		}

		public override void ExecuteState()
		{
			Vector2 _leftStick = GetFSM().GetInput().LeftStick();

			//Forward Attack
			if ( GetFSM().GetInput().Attack() )
			{
				if ( _leftStick.x != 0 )
				{
					GetFSM().GetMovement().OrientationCheck( _leftStick );
					Debug.Log( "Forward Standing Attack" );
				}
				if ( _leftStick.y > 0 )
				{
					AttackList.AttackStruct _temp = GetFSM().GetAttackList( "Basic Attacks" ).GetAttack( "StandingUp" );
					GetFSM().CurrentAttack = _temp.attackRef;
					GetFSM().SetCurrentState( PlayerFSM.States.ATTACKING, false );
				}
				else
				{
					AttackList.AttackStruct _temp = GetFSM().GetAttackList( "Basic Attacks" ).GetAttack( "StandingNeutral" );
					GetFSM().CurrentAttack = _temp.attackRef;
					GetFSM().SetCurrentState( PlayerFSM.States.ATTACKING, false );
				}
			}
			else
			{
				GetFSM().StartCoroutine( SwitchStates( _leftStick ) );
			}
		} 

		/// <summary>
		/// Check for state switching after a set period of time.
		/// </summary>
		/// <param name="_leftStick">Input method.</param>
		/// <returns></returns>
		IEnumerator SwitchStates( Vector2 _leftStick )
		{
			yield return new WaitForSeconds( 0.00f );
			if ( GetFSM().GetInput().Jump() )
				GetFSM().SetCurrentState( PlayerFSM.States.JUMPING, false );
			if ( _leftStick.x > 0 || _leftStick.x < 0 )
				GetFSM().SetCurrentState( PlayerFSM.States.MOVING, false );
			if ( _leftStick.y < 0 )
				GetFSM().SetCurrentState( PlayerFSM.States.CROUCHING, false );
		}
	}
}
