using UnityEngine;
using System.Collections;

namespace UI
{
	public class ButtonHover : MonoBehaviour
	{

		bool inHover;

		Vector2 m_originalPos;
		Vector2 m_currentPos;
		float _time;
		// Use this for initialization
		void Start()
		{
			_time = 0.0f;
			m_originalPos = GetComponent<RectTransform>().anchoredPosition;
		}

		// Update is called once per frame
		void Update()
		{
			if ( inHover )
			{
				if ( _time < 0.5 )
					_time += Time.deltaTime;

				m_currentPos = new Vector2( QuadEaseIn( _time, m_originalPos.x, m_originalPos.x + 5.0f, 0.5f ), m_originalPos.y );
				GetComponent<RectTransform>().anchoredPosition = m_currentPos;
			}
			else
			{
				if ( _time > 0.5f )
					_time -= Time.deltaTime;

				m_currentPos = new Vector2( QuadEaseIn( _time, m_currentPos.x, m_originalPos.x, 0.5f ), m_originalPos.y );
				GetComponent<RectTransform>().anchoredPosition = m_currentPos;
			}
		}

		public void OnHover()
		{
			inHover = true;
		}

		public void OnHoverOff()
		{
			inHover = false;
		}

		/// <summary>
		/// Easing equation function for a quadratic (t^2) easing in: 
		/// accelerating from zero velocity.
		/// </summary>
		/// <param name="t">Current time in seconds.</param>
		/// <param name="b">Starting value.</param>
		/// <param name="c">Final value.</param>
		/// <param name="d">Duration of animation.</param>
		/// <returns>The correct value.</returns>
		public static float QuadEaseIn( float t, float b, float c, float d )
		{
			float ret = c * ( t /= d ) * t + b;
			return ret;

			//Bounce in place - float ret = c * (1 - (t / d)) * t + b;

		}
	}
}
