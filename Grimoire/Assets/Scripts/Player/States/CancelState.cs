using UnityEngine;
using System.Collections;

namespace PlayerStates
{
	public class CancelState : IState
	{
		bool _block;
		public CancelState()
		{
		}

		public override void OnSwitch()
		{
			GetFSM().StartChildCoroutine( Invulnerable() );
			GetFSM().GetActorReference().GetParticleManager().CancelParticle();
			Actor[] _actors = GameManager.GetAllActors();
			for ( int i = 0; i < _actors.Length; i++ )
			{
				if ( _actors[i].gameObject.name != GetFSM().gameObject.name )
				{
					Vector3 _direction	= _actors[i].transform.position - GetFSM().gameObject.transform.position;
					float _distance			= ( _actors[i].transform.position - GetFSM().gameObject.transform.position ).magnitude;

					if ( GetFSM().GetInput().LeftStick().y < 0 )
					{
						if ( _distance < 5.0f )
						{
							_actors[i].GetPhysicsController().ClearValues();
							_actors[i].GetPhysicsController().Velocity = _direction.normalized * 100.0f; //Push
						}
					}
				}
			}
		}

		public override void OnExit()
		{
		}

		public override void ExecuteState()
		{
		}

		public override void ExitConditions()
		{
            if ( !GetFSM().GetMovement().IsJumping() && GetFSM().GetInput().LeftStick().x == 0 )
                GetFSM().GoToPreviousState( true , 2);
            else if ( !GetFSM().GetMovement().IsJumping() && GetFSM().GetInput().LeftStick().x != 0 )
                GetFSM().GoToPreviousState( true , 2 );
            else if ( GetFSM().GetMovement().IsJumping() )
                GetFSM().GoToPreviousState( true , 2);
		}

		IEnumerator Invulnerable()
		{
			GetFSM().GetActorReference().SetInvulnerable( true );
			yield return new WaitForSeconds( 0.5f );
			GetFSM().GetActorReference().SetInvulnerable( false );
		}
	}
}

