using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PageUI : MonoBehaviour
{

	private Image				m_imageReference;
	private AudioSource	m_audioSource;
	private Vector3			m_localPosition;
	private Vector3			m_origPosition;


	// Use this for initialization
	void Start()
	{
		m_imageReference		= GetComponent<Image>();
		m_audioSource			= GetComponentInParent<AudioSource>();
		m_localPosition			= GetComponent<RectTransform>().localPosition;
		m_origPosition				= m_localPosition;
	}

	// Update is called once per frame
	void Update()
	{
	}

	public void Select()
	{
		m_imageReference.color = Color.white;
		m_localPosition = new Vector3( m_origPosition.x, m_origPosition.y + 10.0f, m_origPosition.z ) ;
		GetComponent<RectTransform>().localPosition = m_localPosition;
		SFXManager.PlayOneShot( m_audioSource, SFXManager.GetPageFlip());
	}

	public void Exit()
	{
		m_imageReference.color = Color.gray;
		m_localPosition = m_origPosition;
		GetComponent<RectTransform>().localPosition = m_localPosition;
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
	public float QuadEaseIn( float t, float b, float c, float d )
	{
		t /= d;
		float ret = ( c * ( t * t ) ) + b;
		return ret;

		//Bounce in place - float ret = c * (1 - (t / d)) * t + b;
	}
}
