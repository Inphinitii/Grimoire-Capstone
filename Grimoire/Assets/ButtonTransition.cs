using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonTransition : MonoBehaviour {

	
    Vector2 m_origTransform, 
				 m_endTransform,
				 outVector;

	public Image hoverBackground;
	private Image m_hoverIcon;

	
	float time;
	float duration = 0.5f;
    bool pressed;

	// Use this for initialization
	void Start () {
        time = 0.0f;

		outVector = Vector2.zero;
        m_origTransform = GetComponent<RectTransform>().anchoredPosition;
        m_endTransform = new Vector2(m_origTransform.x + 1.0f ,m_origTransform.y);

		m_hoverIcon = (Image)Instantiate( hoverBackground, new Vector3(-520.0f, 0.0f, 0.0f), Quaternion.identity ) as Image;
		m_hoverIcon.transform.SetParent( this.gameObject.transform, false );
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (pressed)
        {
			if ( time < duration )
            {
                time += Time.deltaTime;

				float alpha = QuadEaseIn( time, 0.0f, 1.0f, duration );

				outVector.x = QuadEaseIn( time, m_origTransform.x, m_endTransform.x, duration );
				outVector.y = m_endTransform.y;
				GetComponent<RectTransform>().anchoredPosition = outVector;

				Color _color = m_hoverIcon.GetComponent<Image>().color;
				_color.a = alpha;
				m_hoverIcon.GetComponent<Image>().color = _color;

            }
        }
		else
		{
			if(time > 0.0f)
				time -= Time.deltaTime;

			float alpha = QuadEaseIn( time, 0.0f, 1.0f, duration );
			Color _color = m_hoverIcon.GetComponent<Image>().color;
			_color.a = alpha;
			m_hoverIcon.GetComponent<Image>().color = _color;

			outVector.x = QuadEaseIn( time, m_origTransform.x, m_endTransform.x, duration );
			outVector.y = m_endTransform.y;
			GetComponent<RectTransform>().anchoredPosition = outVector;
		}
	}

    public void OnPress()
    {
		//m_hoverIcon.gameObject.SetActive( true );
        pressed = true;
    }

	public void OnRelease()
	{
		//m_hoverIcon.gameObject.SetActive( false );
		pressed = false;
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
    public static float QuadEaseIn(float t, float b, float c, float d)
    {
		t /= d;
        float ret = (c * (t * t)) + b;
        return ret;

        //Bounce in place - float ret = c * (1 - (t / d)) * t + b;
        
    }
}
