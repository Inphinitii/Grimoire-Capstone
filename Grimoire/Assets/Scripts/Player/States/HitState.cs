using UnityEngine;
using System.Collections;

namespace PlayerStates
{
	public class HitState : IState
	{
		private float oldDampening;

		public HitState()
		{
		}

		public override void OnSwitch()
		{
			oldDampening = GetFSM().GetActorReference().GetMovementController().groundDampeningConstant;
			GetFSM().GetActorReference().GetMovementController().groundDampeningConstant = 0.93f;
		}

		public override void OnExit()
		{
			GetFSM().GetActorReference().GetMovementController().groundDampeningConstant = oldDampening;
		}

		public override void ExecuteState()
		{
			if ( GetFSM().GetActorReference().GetPhysicsController().Velocity.y > 0 )
			{
				GetFSM().GetActorReference().GetMovementController().SetJumping( true );
			}
			Vector2 _leftStick = GetFSM().GetInput().LeftStick();
			if ( GetFSM().GetActorReference().GetMovementController().IsJumping() )
			{
				GetFSM().GetActorReference().GetMovementController().MoveX( _leftStick * 0.35f );
				GetFSM().GetActorReference().GetParticleManager().SetSmokeHitParticle( true );
			}
			else
			{
				GetFSM().GetActorReference().GetParticleManager().SetSmokeHitParticle( false );
			}
		}
	}
}
