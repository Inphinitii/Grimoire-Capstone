﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class AbstractAttack : MonoBehaviour {


    public AbstractHurtBox[]		boxColliders;
    public Properties.ForceType forceType;
	public ParticleSystem			particleOnHit;

	public float duration;
	public float startupTime;
	public float cooldownTime;

	public float dashWindow;

	public Vector2	hitDirection;
	public float		hitForce;
	public int			hitDamage;
	public bool		staticForce;

	private bool m_dashAvailable;



    public virtual void Start()
    {
        AbstractHurtBox _temp;
        for (int i = 0; i < boxColliders.Length; i++)
        {
            _temp = (AbstractHurtBox)Instantiate( boxColliders[i], this.transform.position, transform.parent.transform.rotation );
            _temp.transform.parent = this.transform;
            _temp.SetForce( forceType );
        }
	}
	
	public virtual void Update () 
	{
	}

	/// <summary>
	/// Called when the attack object is enabled.
	/// </summary>
	public virtual void OnEnable()
	{

	}

	/// <summary>
	/// Called when the attack object is disabled.
	/// </summary>
	public virtual void OnDisable()
	{

	}

	/// <summary>
	/// Sets the 'force' of the attack. This means what color it has, or what team it belongs to.
	/// </summary>
	/// <param name="_force">The force value.</param>
   public void SetForce( Properties.ForceType _force )
    {
        forceType = _force;
        for ( int i = 0; i < boxColliders.Length; i++ )
        {
            boxColliders[i].SetForce( _force );
        }
    }

	/// <summary>
	/// Get a float for use in the FSM.
	/// </summary>
   /// <returns>Return a float that is used in the FSM to block state switching.</returns>
   public float GetStateBlockTime()
   {
	   return startupTime + duration + cooldownTime;
   }

	/// <summary>
	/// Handle the input appropriately for this particular attack. 
	/// </summary>
	/// <param name="_input">The input handler reference of the actor.</param>
	public virtual void HandleInput (InputHandler _input)
   {
		if(m_dashAvailable)
		{
			if(_input.LeftStick().x != 0.0f)
			{
				Debug.Log( "Dash!" );
			}
		}
   }

	/// <summary>
	/// Called when an AbstractHurtBox makes contact with an enemy. 
	/// </summary>
	public virtual void HitEnemy( Collider2D _collider )
	{
		transform.parent.gameObject.GetComponent<Actor>().StartChildCoroutine( FreezePlayers(_collider) );
		//StartCoroutine( DashWindow( dashWindow ) );
	}

	/// <summary>
	/// The window of time that, when activated, allows the player to perform a dash. 
	/// </summary>
	/// <param name="_time">The window of opportunity in seconds.</param>
	public IEnumerator DashWindow(float _time)
	{
		m_dashAvailable = true;
		yield return new WaitForSeconds( _time );
		m_dashAvailable = false;
	}

	/// <summary>
	/// Called to start the specific attack.
	/// </summary>
	public IEnumerator StartAttack()
	{
		yield return new WaitForSeconds( startupTime );

		if(gameObject.active == false)
			gameObject.SetActive( true );

		yield return new WaitForSeconds(  duration );
		if ( gameObject.active == true )
			gameObject.SetActive( false );

		yield return new WaitForSeconds( cooldownTime );
	}

	/// <summary>
	/// Freeze the players for a set amount of time in their animator before resuming it. This is going to contribute to the 
	/// feeling of force applied by an attack. 
	/// </summary>
	/// <param name="_collider"></param>
	/// <returns></returns>
	/// 
	//REFACTOR
	public IEnumerator FreezePlayers( Collider2D _collider )
	{
		transform.parent.gameObject.GetComponent<Animator>().speed = 0.0f;
		_collider.GetComponent<Animator>().speed = 0.0f;
		Debug.Log( "Freeze" );
		Vector3 offSet = new Vector3( 0.0f, 1.0f, 0.0f );
		Instantiate( particleOnHit, _collider.transform.localPosition + offSet, Quaternion.identity ); //Fix this to one shot. Firing off multiple times.
		yield return new WaitForSeconds( 0.4f );
		ApplyForce( _collider );
		_collider.GetComponent<Animator>().speed = 1.0f;
		transform.parent.gameObject.GetComponent<Animator>().speed = 1.0f;

	}

	/// <summary>
	/// Apply a force to the given collider object along the given direction with the given force.
	/// </summary>
	/// <param name="_collider"> Collider object </param>
	public void ApplyForce(Collider2D _collider)
	{
		Vector2 direction = ( _collider.transform.position - this.transform.position ).normalized;
		direction.x *= hitDirection.x;
		direction.y = hitDirection.y;
		_collider.gameObject.GetComponent<PhysicsController>().Velocity = direction * hitForce;
	}
}
