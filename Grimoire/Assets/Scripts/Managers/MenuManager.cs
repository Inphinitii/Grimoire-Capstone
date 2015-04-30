using UnityEngine;
using System.Collections;
using GamepadInput;

public class MenuManager : MonoBehaviour
{
	public enum MenuName
	{
		MAINMENU,
		OPTIONS,
		CHARACTERSELECT,
		GAMEOPTIONS,
		STAGESELECT
	}

	public class MenuGroup
	{
		public AbstractMenu[] menus;
		public MenuGroup( AbstractMenu[] _menus ){menus = _menus;	}
	}

	public AbstractMenu[] mainMenus;
	public AbstractMenu[] optionsMenus;
	public AbstractMenu[] characterSelectionMenus;
	public AbstractMenu[] pauseMenus;
	public GameObject start;
	public bool					isPaused = false;

	private MenuGroup m_characterSelection;
	private MenuGroup m_mainMenu;
	private MenuGroup m_options;
	private MenuGroup m_pauseMenu;

	//REWORK THIS TOMORROW
	private MenuGroup m_currentMenu;
	private MenuGroup m_previousMenu;
	private float				m_menuDeltaTime,
									m_timeThisFrame,
									m_timeLastFrame,
									m_backTimer;
	private bool				m_transitioning;
	private bool active = true;



	void Awake()
	{
		DontDestroyOnLoad( this );
	}

	void Start()
	{

		m_mainMenu				= new MenuGroup( mainMenus );
		m_options					= new MenuGroup( optionsMenus );
		m_characterSelection = new MenuGroup( characterSelectionMenus );
		m_pauseMenu				= new MenuGroup( pauseMenus );
		m_currentMenu			= m_mainMenu;
		m_transitioning			= false;
	}

	void Update()
	{
		if ( active )
		{
			m_timeLastFrame = m_timeThisFrame;
			m_timeThisFrame = Time.realtimeSinceStartup;
			m_menuDeltaTime = m_timeThisFrame - m_timeLastFrame;

			if ( m_currentMenu == m_characterSelection )
			{
				if ( m_currentMenu.menus[0].gameObject.GetComponent<CustomizationMenu>().lockedIn &&
					m_currentMenu.menus[1].gameObject.GetComponent<CustomizationMenu>().lockedIn )
				{

					//Display GameStart message
					start.SetActive( true );

					if ( GamepadInput.GamePad.GetButtonDown( GamepadInput.GamePad.Button.Start, GamepadInput.GamePad.Index.Any ) )
					{
						m_currentMenu = null;
						GameObject.Find( "GameManager" ).GetComponent<GameManager>().OnGameScene();
						active = false;
					}
				}
				else
				{
					start.SetActive( false );
				}
			}
		}
	}

	public void SwitchPlayerCustomization()
	{
		if ( !m_transitioning )
		{
			m_previousMenu = m_currentMenu;
			StartCoroutine( FadeInOut( m_previousMenu, m_characterSelection, 1.0f, 0.0f ) );
		}
	}

	public void SwitchOptions()
	{
		if ( !m_transitioning )
		{
			m_previousMenu = m_currentMenu;
			StartCoroutine( FadeInOut( m_currentMenu, m_options, 1.0f, 0.0f ) );
		}
	}

	public void PauseMenu(bool _open)
	{
		isPaused = _open;
		if(_open)
		{
			for ( int i = 0; i < m_currentMenu.menus.Length; i++ )
				m_currentMenu.menus[i].Active( false );

			Time.timeScale = 0.0f;
			m_pauseMenu.menus[0].GetComponent<CanvasGroup>().alpha = 1.0f;
			m_pauseMenu.menus[0].Active( true );
		}
		else
		{
			for ( int i = 0; i < m_currentMenu.menus.Length; i++ )
				m_currentMenu.menus[i].Active( true );

			Time.timeScale = 1.0f;
			m_pauseMenu.menus[0].GetComponent<CanvasGroup>().alpha = 0.0f;
			m_pauseMenu.menus[0].Active( false );
		}
	}

	public void GoBack()
	{
		if ( m_previousMenu != null && !m_transitioning )
			StartCoroutine( FadeInOut( m_currentMenu, m_previousMenu, 1.0f, 0.0f ) );
	}

	#region Transitions
	IEnumerator FadeIn( AbstractMenu _menuObject, float _speed, float delay)
	{
		yield return new WaitForSeconds( delay );
		float increment;
		CanvasGroup _canvasGroup = _menuObject.GetComponent<CanvasGroup>();
		while ( _canvasGroup.alpha < 1 )
		{
			increment = _speed *	m_menuDeltaTime;
			if ( _canvasGroup.alpha + increment > 1 ) _canvasGroup.alpha = 1;
			else _canvasGroup.alpha += increment;
			yield return null;
		}
		_menuObject.Active( true );
	}

	IEnumerator FadeOut( AbstractMenu _menuObject, float _speed, float delay )
	{
		yield return new WaitForSeconds( delay );
		float increment;
		CanvasGroup _canvasGroup = _menuObject.GetComponent<CanvasGroup>();
		while ( _canvasGroup.alpha > 0 )
		{
			increment = _speed * m_menuDeltaTime;
			if ( _canvasGroup.alpha - increment < 0 ) _canvasGroup.alpha = 0;
			else _canvasGroup.alpha -= increment;
			yield return null;
		}
		_menuObject.Active( false );
	}

	IEnumerator FadeInOut( MenuGroup _first, MenuGroup _second, float _speed, float _delay )
	{
		m_transitioning		= true;
		float increment		= _speed * m_menuDeltaTime;

		CanvasGroup _canvasGroup = _first.menus[0].GetComponent<CanvasGroup>();
		while ( _canvasGroup.alpha > 0 )
		{
			if ( _canvasGroup.alpha - increment < 0 ) _canvasGroup.alpha = 0;
			else _canvasGroup.alpha -= increment;

			for ( int i = 0; i < _first.menus.Length; i++ )
				_first.menus[i].GetComponent<CanvasGroup>().alpha = _canvasGroup.alpha;

				yield return null;
		}
		_first.menus[0].Active( false );


		yield return new WaitForSeconds( _delay );

		m_currentMenu = _second;
		_canvasGroup = _second.menus[0].GetComponent<CanvasGroup>();
		while ( _canvasGroup.alpha < 1 )
		{
			if ( _canvasGroup.alpha + increment > 1 ) _canvasGroup.alpha = 1;
			else _canvasGroup.alpha += increment;

			for ( int i = 0; i < _second.menus.Length; i++ )
				_second.menus[i].GetComponent<CanvasGroup>().alpha = _canvasGroup.alpha;

			yield return null;
		}
		for ( int i = 0; i < m_currentMenu.menus.Length; i++ )
			m_currentMenu.menus[i].Active( true );

		m_transitioning = false;


	}
	#endregion
}
