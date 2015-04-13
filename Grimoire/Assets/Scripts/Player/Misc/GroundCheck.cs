using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class GroundCheck : MonoBehaviour
{
	public Transform			rayAnchor;
	public float					rayDistance;
	public bool					drawDebug;

	private int					m_colliderLayer;

	void Start()
	{
	}


	void Update()
	{

	}

	/// <summary>
	/// Cast a ray downwards.
	/// </summary>
	/// <param name="_collisionMask"> Mask that the ray is intersecting with. </param>
	/// <returns> True upon a collision with the given layermask.</returns>
	public bool CastRay( LayerMask _collisonMask )
	{
		Vector2 rayDirection = -Vector2.up;
		RaycastHit2D ray = Physics2D.Raycast( (Vector2)rayAnchor.position, rayDirection, rayDistance, _collisonMask );
		if ( ray.collider != null )
		{
			m_colliderLayer = ray.collider.gameObject.layer;
			return true;
		}
		else
		{
			return false;
		}
	}

	/// <summary>
	/// Cast a ray downwards and increase the distance of the ray relative to the Y velocity given as a parameter.
	/// </summary>
	/// <param name="_axisVelocity"> Y Velocity to accomodate for. </param>
	/// <param name="_collisionMask"> Mask that the ray is intersecting with. </param>
	/// <returns> True upon a collision with the given layermask.</returns>
	public bool CastRayVelocity(float _axisVelocity, LayerMask _collisonMask)
	{
		bool			goingUp		= _axisVelocity > 0;
		Vector2	rayDirection	= goingUp ? Vector2.up : -Vector2.up;
		float			rayDistance	= Mathf.Abs( _axisVelocity * Time.deltaTime );
		RaycastHit2D ray			= Physics2D.Raycast( (Vector2)rayAnchor.position, rayDirection, rayDistance, _collisonMask );

		if(drawDebug)
			Debug.DrawRay( rayAnchor.position, -rayDirection * rayDistance, Color.red );

		if ( ray.collider != null )
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public int GetCollisionLayer() { return m_colliderLayer; }



}
