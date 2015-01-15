using UnityEngine;
using System.Collections.Generic;



/// <summary>
/// An internal class that holds all of the necessary collision information for the frame
/// </summary>
public class CollisionState
{
	private bool 	_collisionRight;
	private bool 	_collisionRightLast;

	private bool 	_collisionLeft;
	private bool 	_collisionLeftLast;

	private bool 	_collisionDown;
	private bool 	_collisionDownLast;

	private bool 	_collisionUp;
	private bool 	_collisionUpLast;
	
	private bool 	_groundedThisFrame;
	private bool 	_groundedLastFrame;
	
	public CollisionState(){}
	
	public void SetLastFrameValues()
	{
		_collisionRightLast = _collisionRight;
		_collisionLeftLast  = _collisionLeft;
		_collisionUpLast	= _collisionUp;
		_collisionDownLast	= _collisionDown;
		_groundedLastFrame  = _collisionDown;
	}

	public void ResetValues() 
	{ 
		_collisionRight		= 
		_collisionLeft  	= 
		_collisionDown  	= 
		_collisionUp 		= 
		_groundedThisFrame  = false;
	}

	public bool Right 		{ get{return _collisionRight;	} 	set{_collisionRight   	= value;	}}
	public bool Left 		{ get{return _collisionLeft;	} 	set{_collisionLeft   	= value;	}}
	public bool Down 		{ get{return _collisionDown;	} 	set{_collisionDown 	    = value;	}}
	public bool Up 			{ get{return _collisionUp;	    } 	set{_collisionUp	   	= value;	}}

	public bool RightLast	{ get{return _collisionRightLast;	} 	set{_collisionRightLast = value;	}}
	public bool LeftLast 	{ get{return _collisionLeftLast;	} 	set{_collisionLeftLast 	= value;	}}
	public bool DownLast 	{ get{return _collisionDownLast;	} 	set{_collisionDownLast	= value;	}}
	public bool UpLast 		{ get{return _collisionUpLast;	    } 	set{_collisionUpLast 	= value;	}}

	public bool GroundedThisFrame 	{ get{return _groundedThisFrame;} 	set{_groundedThisFrame = value;	}}
	public bool GroundedLastFrame 	{ get{return _groundedLastFrame;} 	set{_groundedLastFrame = value;	}}

	public bool IsColliding() 		{ return _collisionRight || _collisionLeft || _collisionUp || _collisionDown; }
}

public class ControllerCollider : MonoBehaviour
{
	#region Internal Types
	public struct RaycastOrigins 
	{ 
		public Vector2 _topLeft; 
		public Vector2 _topRight;
		public Vector2 _bottomLeft;
		public Vector2 _bottomRight;
	}
	
	#endregion
	
	#region Properties
	private float m_skinWidth = 0.02f;
	
	public LayerMask m_platformMask 		= 0; //The mask of the platforms that we're going to collide with, used by our Rays.
	public LayerMask m_oneWayPlatformMask   = 0; //The mask of the platforms that we're going to collide with other than the bottom.
		
	public int _horizontalRayCount 			= 6;
	public int _verticalRayCount 			= 4;

	
	private CollisionState 		m_ColliderState;  	//Instance of our CollisionState defined above
	private BoxCollider2D 		m_Collider;      		//Use the Box Collider to find the corner of our body and cast rays.
	private Vector2 			m_Velocity;
	
	#endregion
	
	#region Raycast Properties
	private RaycastOrigins 		_raycastOrigins; 
	private RaycastHit2D		_raycastHit;	
	private float 			_horiDistanceBetweenRays;
	private float 			_vertDistanceBetweenRays;
	#endregion

		//Public Accessors
	public CollisionState CState() 		{ return m_ColliderState; 		}
	public bool 	isGrounded() 		{ return m_ColliderState.DownLast; 	}
	
	void Awake()
	{
		m_platformMask     |= m_oneWayPlatformMask;
		m_Collider 			= GetComponent<BoxCollider2D>();
		m_ColliderState 	= new CollisionState();
		CalculateDistanceBetweenRays();
	}
	
	/// <summary>
	/// Checks and changes the future velocity in accordance to whether or not it has collided with anything.
	/// <param name="_nextFrameDelta">The velocity of the object for the next frame</param>
	/// </summary>	
	public Vector2 Move(Vector2 _nextFrameDelta)
	{
		m_ColliderState.SetLastFrameValues();
		m_ColliderState.ResetValues();
		
		Vector2 _desiredPosition = (Vector2)transform.position + _nextFrameDelta;
		Vector2 _temp = _nextFrameDelta;
		SetRayOrigins(_desiredPosition);
		
		if(_nextFrameDelta.y != 0)
		{
			VerticalMovement(ref _temp);
		}
		
		if(_nextFrameDelta.x != 0)
		{
			HorizontalMovement(ref _temp);
		}
		
		
		//Alter the velocity in accordance to time
		if(Time.deltaTime > 0)
			m_Velocity = _temp;
			
		
		//Check if you touched the ground on this frame
		if(!m_ColliderState.GroundedLastFrame && m_ColliderState.Down)
			m_ColliderState.GroundedThisFrame = true;
			
		return m_Velocity;
	}
	
	/// <summary>
	/// If the x in the future velocity is not zero, enter this method.
	/// Check whether or not any of the rays to the left or right of the object collide with anything
	/// If they do, move the object accordingly and return the velocity
	/// </summary>
	public void HorizontalMovement(ref Vector2 _nextFrameDelta)
	{
		bool 	_goingRight 	= _nextFrameDelta.x > 0;
		float 	_rayDistance 	=  Mathf.Abs(_nextFrameDelta.x) + m_skinWidth;
		Vector2 _rayDirection 	= _goingRight ? Vector2.right : -Vector2.right;
		Vector2 _rayOrigin 		= _goingRight ? _raycastOrigins._bottomRight : _raycastOrigins._bottomLeft;
		Vector2 _ray;
		
		//Offset the origin by the velocity on the Y axis to ensure that the rays do not fall behind.
		_rayOrigin.y += _nextFrameDelta.y;

		for(int i = 0; i < _horizontalRayCount; i++)
		{
			_ray = new Vector2(_rayOrigin.x, _rayOrigin.y + (i * _horiDistanceBetweenRays));
			Debug.DrawRay(_ray, _rayDirection * _rayDistance, Color.red);
			
			if(i == 0 && m_ColliderState.GroundedLastFrame)
				_raycastHit = Physics2D.Raycast(_ray, _rayDirection, _rayDistance, m_platformMask);
			else
				_raycastHit = Physics2D.Raycast(_ray, _rayDirection, _rayDistance, m_platformMask & ~m_oneWayPlatformMask);
				
			if(_raycastHit)
			{
				_nextFrameDelta.x = _raycastHit.point.x - _ray.x;				
				if( _goingRight)
				{
					_nextFrameDelta.x -= m_skinWidth;
					m_ColliderState.Right = true;
				}
				else
				{
					_nextFrameDelta.x += m_skinWidth;
					m_ColliderState.Left = true;
				}	
		
				if( _rayDistance < m_skinWidth +  0.001f)
					break;
			}
		}
	}

	/// <summary>
	/// If the y in the future velocity is not zero, enter this method.
	/// Check whether or not any of the rays under or above the object collide with anything
	/// If they do, move the object accordingly and return the velocity
	/// </summary>
	public void VerticalMovement(ref Vector2 _nextFrameDelta)
	{
		bool 	_goingUp 		= _nextFrameDelta.y > 0;
		float 	_rayDistance 	=  Mathf.Abs(_nextFrameDelta.y) + m_skinWidth;
		Vector2 _rayDirection 	= _goingUp ? Vector2.up : -Vector2.up;
		Vector2 _rayOrigin 		= _goingUp ? _raycastOrigins._topLeft : _raycastOrigins._bottomLeft;
		Vector2 _ray;
		//Offset the origin by the velocity on the X axis to ensure that the rays do not fall behind.
		_rayOrigin.x += _nextFrameDelta.x;

		LayerMask mask = m_platformMask;
		
		if(_goingUp && !m_ColliderState.GroundedLastFrame)
			mask &= ~m_oneWayPlatformMask;

		for(int i = 0; i < _verticalRayCount; i++)
		{
			_ray 		= new Vector2(_rayOrigin.x + i * _vertDistanceBetweenRays, _rayOrigin.y);
			_raycastHit = Physics2D.Raycast(_ray, _rayDirection, _rayDistance * 3.0f, mask);

			Debug.DrawRay(_ray, _rayDirection * _rayDistance, Color.red);

			if(_raycastHit)
			{
				_nextFrameDelta.y 	= _raycastHit.point.y - _ray.y;
				_rayDistance 		= Mathf.Abs(_nextFrameDelta.y);

				if( _goingUp)
				{
					_nextFrameDelta.y -= m_skinWidth;
					m_ColliderState.Up = true;
				}
				else
				{
					_nextFrameDelta.y += m_skinWidth;
					m_ColliderState.Down = true;
				}	


				if( _rayDistance < m_skinWidth + 0.001f)
					return;
			}
		}
	}

	/// <summary>
	/// Calculate the collision box in accordance to the scale of object, and then set the origin of the rays
	/// </summary>
	public void SetRayOrigins(Vector2 _nextFramePosition)
	{
		Vector2 _scaledColliderSize = new Vector2( m_Collider.size.x * Mathf.Abs( transform.localScale.x ), m_Collider.size.y * Mathf.Abs( transform.localScale.y ) ) / 2;
		Vector2 _scaledCenter 		= new Vector2( m_Collider.center.x * transform.localScale.x, m_Collider.center.y * transform.localScale.y );
		
		_raycastOrigins._topRight = transform.position + 	new Vector3( _scaledCenter.x + _scaledColliderSize.x, _scaledCenter.y + _scaledColliderSize.y );
		_raycastOrigins._topRight.x -= m_skinWidth;
		_raycastOrigins._topRight.y -= m_skinWidth;
		
		_raycastOrigins._topLeft = transform.position +  	new Vector3( _scaledCenter.x - _scaledColliderSize.x, _scaledCenter.y + _scaledColliderSize.y );
		_raycastOrigins._topLeft.x += m_skinWidth;
		_raycastOrigins._topLeft.y -= m_skinWidth;
		
		_raycastOrigins._bottomRight = transform.position + new Vector3( _scaledCenter.x + _scaledColliderSize.x, _scaledCenter.y -_scaledColliderSize.y );
		_raycastOrigins._bottomRight.x -= m_skinWidth;
		_raycastOrigins._bottomRight.y += m_skinWidth;
		
		_raycastOrigins._bottomLeft = transform.position + 	new Vector3( _scaledCenter.x - _scaledColliderSize.x, _scaledCenter.y -_scaledColliderSize.y );
		_raycastOrigins._bottomLeft.x += m_skinWidth;
		_raycastOrigins._bottomLeft.y += m_skinWidth;
	}
	
	/// <summary>
	/// Calculate the width and height of the collider and calculate an appropriate spacing for the number of vertical and horizontal rays specified above
	/// </summary>
	public void CalculateDistanceBetweenRays()
	{

		float colliderHeight = m_Collider.size.y * Mathf.Abs(transform.localScale.y) - (2.0f * m_skinWidth);
		_vertDistanceBetweenRays = colliderHeight / (_verticalRayCount - 1);

		float colliderWidth = m_Collider.size.x * Mathf.Abs(transform.localScale.x) - (2.0f * m_skinWidth);
		_horiDistanceBetweenRays = colliderWidth / (_horizontalRayCount - 1);
	}
}