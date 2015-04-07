using UnityEngine;
using System.Collections;

namespace PlayerStates
{
	//TODO MAKE THE EXIT STATES
	//TODO LANDING STATE
	public class HitState : IState
	{
		private float oldDampening;
		private const float hitMovementDampener = 0.35f;
		private const float hitGroundDampener = 0.93f;
		private const float smokeParticleThreshold = 10.0f;

		public HitState()
		{
		}

		public override void OnSwitch()
		{
			oldDampening = GetFSM().GetActorReference().GetMovementController().groundDampeningConstant;
			GetFSM().GetActorReference().GetMovementController().groundDampeningConstant = hitGroundDampener;

		}

		public override void OnExit()
		{
			GetFSM().GetActorReference().GetMovementController().groundDampeningConstant = oldDampening;
		}

		public override void ExecuteState()
		{
			Vector2 _leftStick = GetFSM().GetInput().LeftStick();
			GetFSM().GetActorReference().GetMovementController().MoveX( _leftStick * hitMovementDampener );
			if ( GetFSM().GetActorReference().GetPhysicsController().Velocity.y > 0.0f )
			{
				GetFSM().GetActorReference().GetMovementController().SetJumping( true );
			}

			//Particles
			if ( Mathf.Abs( GetFSM().GetActorReference().GetPhysicsController().Velocity.y ) > smokeParticleThreshold ) 
			{
				GetFSM().GetActorReference().GetParticleManager().SetSmokeHitParticle( true );
			}
			else
			{
				GetFSM().GetActorReference().GetParticleManager().SetSmokeHitParticle( false );
			}

			ApexOfHit();


		}

		void ApexOfHit()
		{

			if ( GetFSM().GetActorReference().GetPhysicsController().LastVelocity.y > 0.0f && GetFSM().GetActorReference().GetPhysicsController().Velocity.y < 0.0f )
			{
				GetFSM().SetCurrentState( PlayerFSM.States.JUMPING, true );
			}
		}
	}
}
