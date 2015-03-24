using UnityEngine;
using System.Collections;

public class ButtonTransition : MonoBehaviour {

    Vector2 m_origTransform, m_endTransform, outVector;
    float time;
    bool pressed;

	// Use this for initialization
	void Start () {
        time = 0.0f;
        m_origTransform = this.GetComponent<RectTransform>().anchoredPosition;
        m_endTransform = new Vector2(m_origTransform.x + 10.0f, m_origTransform.y);
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (pressed)
        {
            if (time < 1.0f)
            {
                time += Time.deltaTime;

                outVector.x = QuadEaseIn(time, m_origTransform.x, m_endTransform.x, 1.0f);
                outVector.y = QuadEaseIn(time, m_origTransform.y, m_endTransform.y, 1.0f);

                GetComponent<RectTransform>().anchoredPosition = outVector;
            }
            else
            {
                pressed = false;
                time = 0.0f;
            }
        }
	}

    public void OnPress()
    {
        pressed = true;
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
        float ret = c * (t /= d) * t + b;
        return ret;

        //Bounce in place - float ret = c * (1 - (t / d)) * t + b;
        
    }
}
