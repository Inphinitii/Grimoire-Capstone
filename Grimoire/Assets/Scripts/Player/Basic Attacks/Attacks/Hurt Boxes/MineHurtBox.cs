using UnityEngine;
using System.Collections;

public class MineHurtBox : AbstractHurtBox
{
	public Vector2 hitDirection;
	public float		hitForce;
	public bool		hitAlongDistanceVector;

	private float lifetime = 5.0f;

	public override void Start()
	{
		transform.parent = null;
	}
	public override void Update()
	{
		this.transform.position = m_reference.transform.position;

		if ( m_reference.GetComponent<PhysicsController>().Velocity != Vector2.zero )
		{
			Vector3 temp = m_reference.GetComponent<PhysicsController>().Velocity;
			temp.x *= 0.95f;
			temp.y *= 0.95f;
			m_reference.GetComponent<PhysicsController>().Velocity = temp;
		}

		if ( lifetime > 0.0f )
			lifetime -= Time.deltaTime;
		else
			DestroyObject();
	}

	public override void OnHurtboxHit( Collider2D _collider )
	{
		Vector3 direction = (_collider.transform.position + new Vector3( 0.0f, 1.0f, 0.0f ) - m_reference.transform.position).normalized;
		m_reference.GetComponent<PhysicsController>().Velocity = -direction * 35.0f;
		base.OnHurtboxHit( _collider );
	}

	public override void OnFriendlyHit( Collider2D _collider )
	{
		base.OnFriendlyHit( _collider );
	}

	public override void OnEnemyHit( Collider2D _enemy )
	{
		Camera.main.GetComponent<CameraShake>().Shake();
		m_reference.GetComponent<PhysicsController>().ClearValues();
		m_reference.GetComponent<PhysicsController>().PausePhysics(true);
		StartCoroutine( FreezePlayers( _enemy ) );
	}

	public override void OnAnyHit()
	{
		base.OnAnyHit();
	}

	private void FloorHit(Collider2D _collider)
	{
		if ( m_reference.gameObject.GetComponent<PhysicsController>().Velocity != Vector2.zero )
			DestroyObject();
	}

	public override void OnTriggerEnter2D(Collider2D _collider)
	{
	     if ( _collider.gameObject.tag == "Player" )
        {
			if ( _collider.gameObject.GetComponent<Actor>().Force == forceType )
				OnFriendlyHit( _collider );
			else
				OnEnemyHit( _collider);
        }
		else if (_collider.gameObject.tag == "HurtBox")
		{
			if ( _collider.gameObject.GetComponent<AbstractHurtBox>().forceType == forceType )
				OnHurtboxHit( _collider );
		}
        else if(_collider.gameObject.tag == "Floor" || _collider.gameObject.tag == "Platform" )
			 FloorHit(_collider);
		else
            OnAnyHit();
	}

	public void ApplyForce( Collider2D _collider )
	{

		Vector2 direction = ( _collider.transform.position + new Vector3(0.0f, 1.0f, 0.0f) - m_reference.transform.position ).normalized;

		if ( !hitAlongDistanceVector )
		{
			direction.x *= hitDirection.x;
			direction.y = hitDirection.y;
		}

		_collider.gameObject.GetComponent<PhysicsController>().Velocity = direction * hitForce;
	}

	public IEnumerator FreezePlayers( Collider2D _collider )
	{
		PhysicsController _otherPhys = _collider.gameObject.GetComponent<PhysicsController>();
		SpellCharges _otherCharge = _collider.gameObject.GetComponent<SpellCharges>();
		Animator _otherAnimator = _collider.gameObject.GetComponent<Animator>();

		_otherPhys.PausePhysics( true );
		_otherCharge.SetFreezeTimer( true );
		_otherAnimator.speed = 0.0f;

		yield return new WaitForSeconds(0.2f );

		_otherPhys.PausePhysics( false );
		_otherCharge.SetFreezeTimer( false );
		_otherAnimator.speed = 1.0f;

		_collider.gameObject.GetComponent<PlayerFSM>().SetCurrentState( PlayerFSM.States.HIT, true );


		ApplyForce( _collider );

		DestroyObject();
	}

	public void DestroyObject()
	{
		Instantiate( m_reference.GetComponent<OnDestroyParticle>().particleToUse, m_reference.transform.position, Quaternion.identity );
		Destroy( m_reference );
		Destroy( this.gameObject );
	}
}
