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
	private BoxCollider2D	m_boxCollider;
	private Transform			m_parent;

	void Start()
	{
		m_boxCollider = GetComponent<BoxCollider2D>();
		m_boxCollider.isTrigger = true;
		m_parent = transform.parent.transform;
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
    public virtual void OnEnemyHit( Collider2D _collider )
    {
		SendMessageUpwards( "HitEnemy", _collider );			//Send a message to this object
		_collider.SendMessage( "OnHit" );								//Send a message to the enemy
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
    void OnTriggerEnter2D(Collider2D _collider)
    {
        if ( _collider.gameObject.tag == "Player" )
        {
            if ( _collider.gameObject.GetComponent<Actor>().Force == forceType )
                OnFriendlyHit( _collider );
            else
                OnEnemyHit(_collider);
        }
        else
            OnAnyHit();
    }

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.matrix = m_parent.transform.localToWorldMatrix;
		Gizmos.DrawCube((Vector3)m_boxCollider.offset, (Vector3)m_boxCollider.size);
		
	}
}
