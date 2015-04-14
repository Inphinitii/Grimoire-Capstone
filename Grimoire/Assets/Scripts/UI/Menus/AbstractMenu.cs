using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GamepadInput;
using System.Collections;

/*==================================================================================
 * Author: Tyler Remazki
 *
 * Class : Abstract Menu
 *
 * Description: Base class that handles buttons within custom menus using the custom 
 * input handler, since Unity wants to be a bitch. Attach this to a canvas. 
 =================================================================================*/

//TODO FIX ALPHA PROBLEM 
//MENU STACK FOR THE MENUS AND GOING BACK TO WORK PROPERLY 

[RequireComponent(typeof(CanvasGroup))]
public class AbstractMenu : MonoBehaviour
{
	public int			controllerNumber;	//Which controller number will effect this menu 
	public bool		verticalMovement;
	public bool		isActive;

	private Button[]			m_buttonList;
	private Button				m_currentSelection;
	private EventSystem	m_panelEventSystem;
	private CanvasGroup	m_canvasGroup;

	private int					m_selectionIndex	= 0;
	private float					m_currentTime		= 0.0f;
	private const float		SCROLL_DELAY		= 0.15f;

	void Start()
	{
		m_canvasGroup		= GetComponent<CanvasGroup>();
		m_buttonList		= GetComponentsInChildren<Button>();
		m_panelEventSystem	= GameObject.FindObjectOfType<EventSystem>();

		if ( m_buttonList.Length != 0 )
		{
			m_currentSelection = m_buttonList[m_selectionIndex];

			//Set the currently selected object to be the first within the menu. 
			m_panelEventSystem.SetSelectedGameObject( m_currentSelection.gameObject );
		}

		//Set the default state of the menu 
		if ( !isActive )
			m_canvasGroup.alpha = 0.0f;
	}

	
	void Update()
	{
		if ( GamepadInput.GamePad.GetButtonDown( GamepadInput.GamePad.Button.B, GamepadInput.GamePad.Index.Any ) )
		{
			FindObjectOfType<MenuManager>().GoBack();
		}

		if ( isActive )
		{
			if(GamepadInput.GamePad.GetButtonDown(GamepadInput.GamePad.Button.A, (GamepadInput.GamePad.Index)controllerNumber))
			{
				Debug.Log( "Ding" );
				//GOLDEN PIECE OF UI CODE
				ExecuteEvents.Execute<ISubmitHandler>( m_currentSelection.gameObject, new BaseEventData( EventSystem.current ), ExecuteEvents.submitHandler);
			}
			if ( m_buttonList.Length > 0 )
			{
				Vector2 input = GamePad.GetAxis( GamepadInput.GamePad.Axis.LeftStick, (GamePad.Index)controllerNumber, true );
				if ( m_currentTime <= 0.0f )
				{
					if ( input != Vector2.zero )
					{
						Scroll( input, verticalMovement );
						m_currentTime = SCROLL_DELAY;
					}
				}
				else if ( m_currentTime > 0.0f )
					m_currentTime -= Time.deltaTime;
			}
		}
	}

	public virtual void Scroll( Vector3 _stick, bool _vertical )
	{

		float direction;
		direction = !_vertical ? -_stick.x : _stick.y;

		if ( direction < 0.0f)
			m_selectionIndex++;
		else if(direction > 0.0f)
			m_selectionIndex--;

		if ( m_selectionIndex >= m_buttonList.Length)		
			m_selectionIndex = 0;
		else if ( m_selectionIndex < 0 ) 
			m_selectionIndex = m_buttonList.Length - 1;

		m_currentSelection = m_buttonList[m_selectionIndex];
		m_panelEventSystem.SetSelectedGameObject( m_currentSelection.gameObject );
	}

	public virtual void Active(bool _active)
	{
		isActive = _active;
		if ( _active && m_buttonList.Length > 0 )
		{
			m_currentSelection = m_buttonList[0];
			m_panelEventSystem.SetSelectedGameObject( m_currentSelection.gameObject );
		}
		else if ( !_active )
			m_panelEventSystem.SetSelectedGameObject( null );
		
	}

	public virtual void OnExit()
	{

	}
}
