using UnityEngine;
using System.Collections;

namespace PlayerStates
{
	public class CrouchingState : IState
	{
		Vector2 m_leftStick;

		public CrouchingState()
		{
		}

		public override void OnSwitch()
		{
			GetFSM().GetComponent<Animator>().SetBool( "Crouching", true );
			base.OnSwitch();
		}

		public override void OnExit()
		{
			GetFSM().GetComponent<Animator>().SetBool( "Crouching", false );
			base.OnExit();
		}
		public override void ExecuteState()
		{
			m_leftStick = GetFSM().GetInput().LeftStick();

			if ( GetFSM().GetInput().Attack().thisFrame  )
			{
				if ( m_leftStick.y < 0.0f )
				{
					m_playerFSM.CurrentAttack = GetFSM().GetAttackList().GetAttack( BasicAttacks.Attacks.CROUCHING_ATTACK );
					GetFSM().GetComponent<Animator>().SetBool( "CrouchingKick", true );
					GetFSM().SetCurrentState( PlayerFSM.States.ATTACKING, false );
				}

			}

		}

		public override void ExitConditions()
		{
			if ( GetFSM().GetInput().Jump().thisFrame )
				GetFSM().GetMovement().FallThrough();
			if ( m_leftStick.y >= 0 )
				GetFSM().GoToPreviousState( true );

		}
	}
}
