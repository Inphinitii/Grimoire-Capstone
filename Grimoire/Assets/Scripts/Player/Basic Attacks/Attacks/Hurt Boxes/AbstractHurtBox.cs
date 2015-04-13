using UnityEngine;
using System.Collections;

/*========================================================
 * Author: Tyler Remazki
 *
 * Class : Abstract Hurt Box
 *
 * Description: Defines the base behaviour of every HurtBox within the game.
 =========================================================*/

[RequireComponent(typeof(BoxCollider2D))]
[System.Serializable]
public abstract class AbstractHurtBox : MonoBehaviour {

    public	Properties.ForceType forceType;
	public bool drawGizmo = true;

	protected BoxCollider2D	m_boxCollider;
	protected Transform		m_parent;
	protected GameObject	m_reference;

	void Awake()
	{
		gameObject.tag = "HurtBox";
	}

	public virtual void Start()
	{
		m_boxCollider = GetComponent<BoxCollider2D>();
		m_boxCollider.isTrigger = true;
		m_parent = transform.parent.transform;
	}

	public virtual void Update()
	{

	}

    /// <summary>
    /// Called when any object is colliding with the hurt box.
    /// </summary>
    public virtual void OnAnyHit(){}

    /// <summary>
    /// Called when any object that shares the same force type is colliding with the hurt box.
    /// </summary>
    /// <param name="_collider"> Collider2D Object </param>
    public virtual void OnFriendlyHit( Collider2D _collider ){}

    /// <summary>
    /// Called when any object that has a different force type is colliding with the hurt box.
    /// </summary>
    /// <param name="_collider"> Collider2D Object </param>
    public virtual void OnEnemyHit( Collider2D _enemy )
    {
		SendMessageUpwards( "HitEnemy", _enemy );			//Send a message to this object
		_enemy.SendMessage( "OnHit" );								//Send a message to the enemy
    }

	/// <summary>
	/// Called when any two hurt boxes intersect.
	/// </summary>
	/// <param name="_collider"> Collider2D Object </param>
	public virtual void OnHurtboxHit( Collider2D _collider ) 
	{
		SendMessageUpwards( "HitHurtBox", _collider );
	}

    /// <summary>
    /// Set the force type of the current hurt box. Used in the Attack method that houses the HurtBoxes.
    /// </summary>
    /// <param name="_type"></param>
    public void SetForce( Properties.ForceType _type )
    {
        forceType = _type;
    }

	/// <summary>
	/// Enable this hurtbox object.
	/// </summary>
	public void EnableHurtBox()
	{
		gameObject.SetActive( true );
	}

	/// <summary>
	/// Disable this hurtbox object.
	/// </summary>
	public void DisableHurtBox()
	{
		gameObject.SetActive( false );
	}

    /// <summary>
    /// Handles the 2D collision using Unity's default physics system.
    /// </summary>
    /// <param name="_collider"> Collider2D Object </param>
    public virtual void OnTriggerEnter2D(Collider2D _collider)
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
			if ( _collider.gameObject.GetComponent<AbstractHurtBox>().forceType != forceType )
				OnHurtboxHit( _collider );
		}
        else
            OnAnyHit();
    }

	public void SetReference(GameObject _obj)
	{
		m_reference = _obj;
	}

	void OnDrawGizmos()
	{
		if ( drawGizmo )
		{
			int sign = m_parent.gameObject.transform.parent.GetComponent<MovementController>().IsFacingRight() ? 1 : -1;
			Vector3 pos;

			Gizmos.color = Color.red;
			pos = new Vector3( m_boxCollider.offset.x * sign, m_boxCollider.offset.y );
			pos += m_parent.gameObject.transform.position;

			Gizmos.DrawCube( pos, (Vector3)m_boxCollider.size );
		}
		
	}
}
