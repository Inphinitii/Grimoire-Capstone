using UnityEngine;
using System.Collections;

[RequireComponent( typeof( Actor ) )]
//Needs a major overhaul. Doesn't work all the time.
public class StageBouncing : MonoBehaviour
{

	public LayerMask platformLayerMask;
	public LayerMask groundLayerMask;
	public float bounceDampener = 0.1f;

	private Actor m_actorReference;
	private PlayerFSM m_fsmReference;
	private MovementController m_movementController;

	private Vector2 _collisionNormal;
	private bool bounce;
	// Use this for initialization
	void Start()
	{
		m_actorReference	= GetComponent<Actor>();
		m_fsmReference		= GetComponent < PlayerFSM>();
		m_movementController = GetComponent<MovementController>();
	}

	// Update is called once per frame
	void LateUpdate()
	{
		if ( bounce )
		{
			Vector2 reflectionVector;
			if ( m_actorReference.GetPhysicsController().Velocity.y < -25.0f )
			{
				m_movementController.groundCheck = false;
				_collisionNormal.Normalize();
				reflectionVector = (Vector2)Vector3.Reflect( (Vector3)m_actorReference.GetPhysicsController().Velocity, (Vector3)_collisionNormal );
				m_actorReference.GetPhysicsController().Velocity = reflectionVector * bounceDampener;
				m_fsmReference.SetCurrentState( PlayerFSM.States.BOUNCE, true );
				bounce = false;
			}
		}
	}

	void OnCollisionEnter2D( Collision2D _collider )
	{
		if ( _collider.collider.gameObject.layer == LayerMask.NameToLayer( "Platform" ) || _collider.collider.gameObject.layer == LayerMask.NameToLayer( "Floor" ) )
		{
			if(m_fsmReference.GetState( PlayerFSM.States.HIT) == m_fsmReference.currentState)
			{
				_collisionNormal = _collider.contacts[0].normal;
				bounce = true;
				LateUpdate();
			}
		}
	}
}
