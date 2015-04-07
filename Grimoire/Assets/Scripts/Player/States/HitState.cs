﻿using UnityEngine;
using System.Collections;

namespace PlayerStates
{
	//TODO MAKE THE EXIT STATES
	//TODO LANDING STATE
	public class HitState : IState
	{
		private float		m_oldDampening;
		private Vector2		m_leftStick;
		private const float MOVEMENT_DAMPENER		= 0.35f;
		private const float GROUND_ACCEL_DAMPENER	= 0.93f;
		private const float SMOKE_THRESHOLD			= 10.0f;


		public HitState()
		{
		}

		public override void OnSwitch()
		{
			m_oldDampening = GetFSM().GetActorReference().GetMovementController().groundDampeningConstant;
			GetFSM().GetActorReference().GetMovementController().groundDampeningConstant = GROUND_ACCEL_DAMPENER;
			GetFSM().GetActorReference().GetMovementController().m_capAcceleration = false;
			GetFSM().StartChildCoroutine( Flash() );

		}

		public override void OnExit()
		{
			GetFSM().GetActorReference().GetMovementController().groundDampeningConstant = m_oldDampening;
			GetFSM().GetActorReference().GetMovementController().m_capAcceleration = true;
		}

		public override void ExecuteState()
		{
			m_leftStick = GetFSM().GetInput().LeftStick();

			if(m_leftStick.x != 0.0f)
				GetFSM().GetActorReference().GetMovementController().MoveX( m_leftStick * MOVEMENT_DAMPENER );

			if ( GetFSM().GetActorReference().GetPhysicsController().Velocity.y > 0.0f )
				GetFSM().GetActorReference().GetMovementController().SetJumping( true );

			DisplayParticles();


		}

		public override void ExitConditions()
		{
			if ( GetFSM().GetActorReference().GetPhysicsController().LastVelocity.y > 0.0f && GetFSM().GetActorReference().GetPhysicsController().Velocity.y < 0.0f )
				GetFSM().SetCurrentState( PlayerFSM.States.JUMPING, true );
		}

		private void DisplayParticles()
		{
			if ( Mathf.Abs( GetFSM().GetActorReference().GetPhysicsController().Velocity.y ) > SMOKE_THRESHOLD )
				GetFSM().GetActorReference().GetParticleManager().SetSmokeHitParticle( true );
			else
				GetFSM().GetActorReference().GetParticleManager().SetSmokeHitParticle( false );
		}

		//Currently debugging. Maybe turn this into something interesting and visually appealing later. 
		private IEnumerator Flash()
		{
			GetFSM().GetActorReference().GetRenderer().material.color = Color.white;
			yield return new WaitForSeconds( 0.25f );
			GetFSM().GetActorReference().GetRenderer().material.color = GetFSM().GetActorReference().actorColor;

		}
	}
}
