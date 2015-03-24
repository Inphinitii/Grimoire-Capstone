using UnityEngine;
using System.Collections;

namespace PlayerStates
{
	public class JumpingState : IState
	{
		bool lastJump;
		bool getJump;

		public JumpingState()
		{
		}

		public override void OnSwitch()
		{
			//GetFSM().GetMovement().ApplyJump( true, Vector2.zero );
		}

		public override void ExecuteState()
		{
			/* Redundant Code */
			Vector2 _leftStick = GetFSM().GetInput().LeftStick();
			if ( _leftStick.x > 0 || _leftStick.x < 0 )
				GetFSM().GetMovement().MoveX( _leftStick );
			///////////////////
			lastJump = getJump;


			getJump = GetFSM().GetInput().A();
			if ( getJump )
			{
				GetFSM().GetMovement().ApplyJump( true, _leftStick );
			}
			else if ( getJump != lastJump )
			{
				GetFSM().GetMovement().ApplyJump( false, _leftStick );
			}

			if ( GetFSM().GetInput().Y() )
			{
				if ( _leftStick.y < 0 )
				{
					Debug.Log( "Down air" );
				}
				else if ( _leftStick.x > 0 && GetFSM().GetMovement().SignThisFrame() == -1 ||
						_leftStick.x < 0 && GetFSM().GetMovement().SignThisFrame() == 1 )
				{
					AttackList.AttackStruct _temp = GetFSM().GetAttackList( "Basic Attacks" ).GetAttack( "BackAir" );
					GetFSM().CurrentAttack = _temp.attackRef;
					GetFSM().SetCurrentState( PlayerFSM.States.ATTACKING, false );
				}
				else if ( _leftStick.x == 0 && _leftStick.y == 0 )
				{
					AttackList.AttackStruct _temp = GetFSM().GetAttackList( "Basic Attacks" ).GetAttack( "NeutralAir" );
					GetFSM().CurrentAttack = _temp.attackRef;
					GetFSM().SetCurrentState( PlayerFSM.States.ATTACKING, false );

				}
			}

			if ( !GetFSM().GetMovement().IsJumping() && GetFSM().GetPhysics().Velocity.x == 0 )
			{
				GetFSM().SetCurrentState( PlayerFSM.States.STANDING, false );
			}
			else if ( !GetFSM().GetMovement().IsJumping() )
			{
				GetFSM().SetCurrentState( PlayerFSM.States.MOVING, false );
			}
		}
	}
}
