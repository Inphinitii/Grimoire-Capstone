using UnityEngine;
using System.Collections;

[RequireComponent( typeof( Actor ) )]
//Needs a major overhaul. Doesn't work all the time.
public class StageBouncing : MonoBehaviour
{

	public LayerMask	platformLayerMask;
	public LayerMask	groundLayerMask;
	public float			bounceThreshold = 10.0f;
	public float			bounceDampener = 1.0f;

	private Actor				m_actorReference;
	private PlayerFSM		m_fsmReference;

	private Vector2 _collisionNormal;
	private bool bounce;
	// Use this for initialization
	void Start()
	{
		m_actorReference	= GetComponent<Actor>();
		m_fsmReference		= GetComponent < PlayerFSM>();
	}

	// Update is called once per frame
	void LateUpdate()
	{
	}

	void Bounce(float _velocityy)
	{
		if ( bounce )
		{
			Vector2 reflectionVector;
			//m_movementController.groundCheck = false;
			_collisionNormal.Normalize();
			reflectionVector = (Vector2)Vector3.Reflect( (Vector3)m_actorReference.GetPhysicsController().LastVelocity, _collisionNormal ); // HACKY. We're not taking collision normals, we're using just Vector2.up

			m_actorReference.GetPhysicsController().Velocity = reflectionVector * bounceDampener;
			m_fsmReference.SetCurrentState( PlayerFSM.States.BOUNCE, true );
			bounce = false;
		}
	}

	void OnCollisionStay2D( Collision2D _collider )
	{
		if ( !bounce )
		{
			if ( _collider.collider.gameObject.layer == LayerMask.NameToLayer( "Platform" ) || _collider.collider.gameObject.layer == LayerMask.NameToLayer( "Floor" ) )
			{
				if ( m_fsmReference.currentState == m_fsmReference.GetState( PlayerFSM.States.HIT ) )
				{
					_collisionNormal = Vector2.zero;
					_collisionNormal += _collider.contacts[0].normal;

					float vel = m_actorReference.GetPhysicsController().Velocity.y;
					if ( vel < -bounceThreshold )
					{
						bounce = true;
						Bounce( vel );
					}
				}
			}
		}
	}
}
