using UnityEngine;

/*========================================================
 * Author: Tyler Remazki
 *
 * Class : Hit Cirlce
 *
 * Description: 
 =========================================================*/

public class HitCircle
{
	public enum HitBoxType { HURT, HIT }

	public float   		p_hitBoxRadius;
	public Vector2 		p_hitBoxTransform;
	public HitBoxType 	p_hitBoxType;

	private Vector2 	m_hitBoxOrigin;
	private Vector2     m_hitDirection;

	public HitCircle(float _radius, Vector2 _origin, Vector2 _transform, HitBoxType _type)
	{
		p_hitBoxRadius 		= _radius;
		p_hitBoxTransform 	= _transform;
		p_hitBoxType 		= _type;

		m_hitBoxOrigin 		= _origin;
		m_hitDirection 		= new Vector2(0.0f, 0.0f);
	}

	public static bool CheckCollision(HitCircle _box1, HitCircle _box2)
	{
		float _distanceBetween = (_box1.p_hitBoxTransform - _box2.p_hitBoxTransform).magnitude;
		float _sumOfRadii 	   = (_box1.p_hitBoxRadius 	  + _box2.p_hitBoxRadius);

		if(_distanceBetween < _sumOfRadii )
			return true;

		return false;
	}
}