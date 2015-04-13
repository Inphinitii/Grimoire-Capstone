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
				if( m_leftStick.x > 0.5f || m_leftStick.x < -0.5f)
				{
					//FIX DIS
					GetFSM().GetMovement().OrientationCheck( m_leftStick );
					if ( GetFSM().GetMovement().IsFacingRight() && m_leftStick.x > 0.0f ||
						!GetFSM().GetMovement().IsFacingRight() && m_leftStick.x < 0.0f)
					{
						GetFSM().CurrentAttack = GetFSM().GetAttackList().GetAttack( BasicAttacks.Attacks.AIR_FRONT );
						GetFSM().SetCurrentState( PlayerFSM.States.ATTACKING, true );		
					}
					
					else if ( GetFSM().GetMovement().IsFacingRight() && m_leftStick.x < 0.0f ||
								!GetFSM().GetMovement().IsFacingRight() && m_leftStick.x > 0.0f )
					{
						GetFSM().CurrentAttack = GetFSM().GetAttackList().GetAttack( BasicAttacks.Attacks.AIR_BACK );
						GetFSM().SetCurrentState( PlayerFSM.States.ATTACKING, true );		

					}
				}
				//Downward Attack - Jumping
				else if ( m_leftStick.y < 0 )
				{
					GetFSM().CurrentAttack = GetFSM().GetAttackList().GetAttack( BasicAttacks.Attacks.AIR_DOWN );
					GetFSM().SetCurrentState( PlayerFSM.States.ATTACKING, true );
				}

				else if ( m_leftStick == Vector2.zero )
				{
					GetFSM().CurrentAttack = GetFSM().GetAttackList().GetAttack( BasicAttacks.Attacks.AIR_NEUTRAL );
					GetFSM().SetCurrentState( PlayerFSM.States.ATTACKING, true );
				}
			}
			else if(m_leftStick.y < 0)
			{
				GetFSM().GetMovement().ApplyFastFall();
			}
		}

		public override void ExitConditions()
		{
			if ( !GetFSM().GetMovement().IsJumping() && GetFSM().GetPhysics().Velocity.x == 0 )
				GetFSM().SetCurrentState( PlayerFSM.States.STANDING, false );
			else if ( !GetFSM().GetMovement().IsJumping() )
				GetFSM().SetCurrentState( PlayerFSM.States.MOVING, false );

			if(GetFSM().GetInput().LeftStick().y < 0.1f)
				if ( GetFSM().GetInput().Triggers().thisFrame > 0.5f && GetFSM().GetInput().Triggers().lastFrame < 0.5f )
					if ( GetFSM().GetActorReference().GetSpellCharges().UseCharge())
							GetFSM().SetCurrentState( PlayerFSM.States.DASHING, true );

		}
	}
}
