using UnityEngine;
using System.Collections;

namespace PlayerStates
{
	public class JumpingState : IState
	{
		bool	m_lastJump;
		bool	m_getJump;
		bool m_jumpStart;
		bool m_jumping;
		Vector2 m_leftStick;

		public JumpingState()
		{
		}

		public override void OnSwitch()
		{
			GetFSM().GetComponent<Animator>().SetBool( "Jumping", true );
            GetFSM().GetComponent<Animator>().SetBool( "Falling", true );

		}

        public override void OnPop()
        {
            //OnExit();

        }
		public override void OnExit()
		{
			GetFSM().GetComponent<Animator>().SetBool( "Jumping", false );
			GetFSM().GetComponent<Animator>().SetBool( "Falling", false );
			GetFSM().GetComponent<Animator>().SetBool( "HoldingForward", false );
			GetFSM().GetComponent<Animator>().SetBool( "HoldingBack", false );
		}

		public override void ExecuteState()
		{
			m_lastJump		= m_getJump;
			m_leftStick		= GetFSM().GetInput().LeftStick();
			m_getJump		= GetFSM().GetInput().Jump().thisFrame;

			if ( m_leftStick.x != 0.0f )
				GetFSM().GetMovement().MoveX( m_leftStick );

			if ( GetFSM().GetInput().Jump().thisFrame && !GetFSM().GetInput().Jump().lastFrame )
			{
				if ( GetFSM().GetMovement().JumpCount() < GetFSM().GetMovement().JumpTotal())
					GetFSM().GetComponent<AudioSource>().PlayOneShot( SFXManager.GetJump() );
			}
			//Jumping behaviour differs whether or not the button was let go or pressed.
			if ( GetFSM().GetMovement().JumpCount() <= GetFSM().GetMovement().JumpTotal() )
			{
				if ( m_getJump )
				{
					GetFSM().GetMovement().ApplyJump( true, m_leftStick );

					if(!m_jumping)
						m_jumpStart = true;

					m_jumping = true;
				}
				else if ( m_getJump != m_lastJump )
				{
					GetFSM().GetMovement().ApplyJump( false, m_leftStick );
					m_jumping = false;
				}

				if ( m_jumpStart )
				{
					SFXManager.PlayOneShot(GetFSM().GetComponent<AudioSource>(),  SFXManager.GetJump() );
					m_jumpStart = false;
				}

			}

			if ( GetFSM().GetPhysics().Velocity.y < 0 )
			{
				GetFSM().GetComponent<Animator>().SetBool( "Jumping", false );
				GetFSM().GetComponent<Animator>().SetBool( "Falling", true );
			}
			else if ( GetFSM().GetPhysics().Velocity.y == 0 )
			{
				GetFSM().GetComponent<Animator>().SetBool( "Falling", false );
			}


			if ( GetFSM().GetInput().Special().thisFrame && !GetFSM().GetInput().Special().lastFrame )
			{
				GetFSM().CurrentAttack = GetFSM().GetActorReference().GetGrimoire().UseCurrentPage( Page.Type.AIR_NEUTRAL );
                GetFSM().GetComponent<Animator>().SetBool( "Attacking", true );
				GetFSM().SetCurrentState( PlayerFSM.States.ATTACKING, false );
			}

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
                        GetFSM().GetComponent<Animator>().SetBool( "Attacking", true );
						GetFSM().GetComponent<Animator>().SetBool( "HoldingForward", true );
						GetFSM().SetCurrentState( PlayerFSM.States.ATTACKING, true );		
					}
					
					else if ( GetFSM().GetMovement().IsFacingRight() && m_leftStick.x < 0.0f ||
								!GetFSM().GetMovement().IsFacingRight() && m_leftStick.x > 0.0f )
					{
						GetFSM().CurrentAttack = GetFSM().GetAttackList().GetAttack( BasicAttacks.Attacks.AIR_BACK );
                        GetFSM().GetComponent<Animator>().SetBool( "Attacking", true );
						GetFSM().GetComponent<Animator>().SetBool( "HoldingBack", true );
						GetFSM().SetCurrentState( PlayerFSM.States.ATTACKING, true );		

					}
				}
				//Downward Attack - Jumping
				else if ( m_leftStick.y < 0 )
				{
					GetFSM().CurrentAttack = GetFSM().GetAttackList().GetAttack( BasicAttacks.Attacks.AIR_DOWN );
                    GetFSM().GetComponent<Animator>().SetBool( "Attacking", true );
					GetFSM().SetCurrentState( PlayerFSM.States.ATTACKING, true );
				}

				else if ( m_leftStick == Vector2.zero )
				{
					GetFSM().CurrentAttack = GetFSM().GetAttackList().GetAttack( BasicAttacks.Attacks.AIR_NEUTRAL );
                    GetFSM().GetComponent<Animator>().SetBool( "Attacking", true );
					GetFSM().SetCurrentState( PlayerFSM.States.ATTACKING, true );
				}
			}
			else if(m_leftStick.y < 0)
			{
				GetFSM().GetComponent<Animator>().SetBool( "HoldingDown", true );
				GetFSM().GetMovement().ApplyFastFall();
			}
			else
			{
				GetFSM().GetComponent<Animator>().SetBool( "HoldingDown", false );
			}
		}

		public override void ExitConditions()
		{
			if ( !GetFSM().GetMovement().IsJumping() )
				GetFSM().GoToPreviousState( true, 1 );

			if(GetFSM().GetInput().LeftStick().y < 0.1f)
				if ( GetFSM().GetInput().Triggers().thisFrame > 0.5f && GetFSM().GetInput().Triggers().lastFrame < 0.5f )
					if ( GetFSM().GetActorReference().GetSpellCharges().UseCharge())
							GetFSM().SetCurrentState( PlayerFSM.States.DASHING, true );

		}
	}
}
