using UnityEngine;
using GamepadInput;
using System.Collections;

public class ScrollMenuObject : MonoBehaviour
{

	public bool vertical;
	public bool active;
	public AbstractMenu parent;
	public GameObject[] scrollableObjects;

	protected GameObject m_currentlySelected;
	protected int m_selectionIndex;
	private float m_currentTime = 0.0f;
	private const float SCROLL_DELAY = 0.15f;

	void Start()
	{

	}

	public void SetActive(bool _active)
	{
		active = _active;
	}

	public virtual void Update()
	{
		if ( active )
		{
			if ( scrollableObjects.Length > 0 )
			{
				Vector2 input = GamePad.GetAxis( GamepadInput.GamePad.Axis.LeftStick, (GamePad.Index)parent.controllerNumber, true );
				if ( m_currentTime <= 0.0f )
				{
					if ( input != Vector2.zero )
					{
						Scroll( input, vertical );
						m_currentTime = SCROLL_DELAY;
					}
				}
				else if ( m_currentTime > 0.0f )
					m_currentTime -= Time.deltaTime;
			}
		}
	}

	public virtual void UpdateSelection()
	{

	}

	public void Scroll( Vector3 _stick, bool _vertical )
	{

		Debug.Log( "Scrolling" );
		float direction;
		direction = !_vertical ? -_stick.x : _stick.y;

		if ( direction < 0.0f )
			m_selectionIndex++;
		else if ( direction > 0.0f )
			m_selectionIndex--;

		if ( m_selectionIndex >= scrollableObjects.Length )
			m_selectionIndex = 0;
		else if ( m_selectionIndex < 0 )
			m_selectionIndex = scrollableObjects.Length - 1;

		m_currentlySelected = scrollableObjects[m_selectionIndex];
		UpdateSelection();
	}

}
