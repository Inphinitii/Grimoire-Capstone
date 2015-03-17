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

    public Properties.ForceType forceType;
    public Vector2                      hitDirection;
    public float                           hitForce;
    public int                              hitDamage;
    public bool                           staticForce;

	void Start()
	{
		GetComponent<BoxCollider2D>().isTrigger = true;
	}

    /// <summary>
    /// Called when any object is colliding with the hurt box.
    /// </summary>
    public abstract void OnAnyHit();

    /// <summary>
    /// Called when any object that shares the same force type is colliding with the hurt box.
    /// </summary>
    /// <param name="_collider"> Collider2D Object </param>
    public abstract void OnFriendlyHit( Collider2D _collider );

    /// <summary>
    /// Called when any object that has a different force type is colliding with the hurt box.
    /// </summary>
    /// <param name="_collider"> Collider2D Object </param>
    public virtual void OnEnemyHit( Collider2D _collider )
    {
		SendMessageUpwards( "HitEnemy" );			//Send a message to this object
		_collider.SendMessage( "OnHit" );	//Send a message to the enemy

        Vector2 direction = (_collider.transform.position - this.transform.position).normalized;
        direction.x *= hitDirection.x;
        direction.y = hitDirection.y;
        _collider.gameObject.GetComponent<PhysicsController>().Velocity = direction * hitForce;
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
}
