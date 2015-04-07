using UnityEngine;
using System.Collections;

namespace PlayerStates
{
	public class JumpingState : IState
	{
		bool	m_lastJump;
		bool	m_getJump;
		Vector2 m_leftStick;

		public JumpingState()
		{
		}

		public override void OnSwitch()
		{
		}

		public override void ExecuteState()
		{


			m_lastJump		= m_getJump;
			m_leftStick		= GetFSM().GetInput().LeftStick();
			m_getJump		= GetFSM().GetInput().Jump().thisFrame;

			if ( m_leftStick.x != 0.0f )
				GetFSM().GetMovement().MoveX( m_leftStick );


			//Jumping behaviour differs whether or not the button was let go or pressed.
			if ( m_getJump )
				GetFSM().GetMovement().ApplyJump( true, m_leftStick );
			else if ( m_getJump != m_lastJump )
				GetFSM().GetMovement().ApplyJump( false, m_leftStick );


			if ( GetFSM().GetInput().Attack().thisFrame )
			{
				if( m_leftStick.x != 0)
				{
					//FIX DIS
					GetFSM().GetMovement().OrientationCheck( m_leftStick );
					if ( GetFSM().GetMovement().IsFacingRight() && m_leftStick.x > 0.0f ||
						!GetFSM().GetMovement().IsFacingRight() && m_leftStick.x < 0.0f)
					{
						GetFSM().CurrentAttack = GetFSM().GetAttackList( "Basic Attacks" ).GetAttack( "FrontAir" );
					}

					if ( GetFSM().GetMovement().IsFacingRight() && m_leftStick.x < 0.0f ||
						!GetFSM().GetMovement().IsFacingRight() && m_leftStick.x > 0.0f )
					{
						GetFSM().CurrentAttack = GetFSM().GetAttackList( "Basic Attacks" ).GetAttack( "BackAir" );
					}
				}
				//Downward Attack - Jumping
				if ( m_leftStick.y < 0 )
					GetFSM().CurrentAttack = GetFSM().GetAttackList( "Basic Attacks" ).GetAttack( "DownAir" ); 
				//Neutral Attack - Jumping
				else if ( m_leftStick.x == 0 && m_leftStick.y == 0 )
					GetFSM().CurrentAttack = GetFSM().GetAttackList( "Basic Attacks" ).GetAttack( "NeutralAir" );

				GetFSM().SetCurrentState( PlayerFSM.States.ATTACKING, false );		
			}
		}

		public override void ExitConditions()
		{
			if ( !GetFSM().GetMovement().IsJumping() && GetFSM().GetPhysics().Velocity.x == 0 )
				GetFSM().SetCurrentState( PlayerFSM.States.STANDING, false );
			else if ( !GetFSM().GetMovement().IsJumping() )
				GetFSM().SetCurrentState( PlayerFSM.States.MOVING, false );
		}
	}
}
