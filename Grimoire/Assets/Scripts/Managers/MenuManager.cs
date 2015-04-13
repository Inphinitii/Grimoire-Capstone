using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour
{
	public enum MenuName
	{
		MAINMENU,
		OPTIONS,
		CHARACTERSELECT
	}

	public class MenuGroup
	{
		public AbstractMenu[] menus;
		public MenuGroup( AbstractMenu[] _menus )
		{
			menus = _menus;
		}
	}

	public AbstractMenu mainMenu;
	public AbstractMenu optionsMenu;

	public AbstractMenu characterSelectionMenu1;
	public AbstractMenu characterSelectionMenu2;

	private MenuGroup _characterSelection;
	private MenuGroup _mainMenu;
	private MenuGroup _options;



	MenuGroup currentMenu;
	MenuGroup previousMenu;

	private bool transitioning;


	// Use this for initialization
	void Start()
	{

		_mainMenu = new MenuGroup( new AbstractMenu[1] { mainMenu } );
		_options = new MenuGroup( new AbstractMenu[1] { optionsMenu } );

		_characterSelection = new MenuGroup(new AbstractMenu[2] {characterSelectionMenu1, characterSelectionMenu2});

		currentMenu = _mainMenu;
		transitioning = false;
	}

	// Update is called once per frame
	void Update()
	{
	}

	public void SwitchPlayerCustomization()
	{
		if ( !transitioning )
		{
			previousMenu = currentMenu;
			StartCoroutine( FadeInOut( currentMenu, _characterSelection, 1.0f, 0.0f ) );
		}
	}

	public void SwitchOptions()
	{
		if ( !transitioning )
		{
			previousMenu = currentMenu;
			StartCoroutine( FadeInOut( currentMenu, _options, 1.0f, 0.0f ) );
		}
	}

	public void GoBack()
	{
		if ( previousMenu != null && !transitioning )
			StartCoroutine( FadeInOut( currentMenu, previousMenu, 1.0f, 0.0f ) );
	}

	IEnumerator FadeIn( AbstractMenu _menuObject, float _speed, float delay)
	{
		yield return new WaitForSeconds( delay );
		float increment;
		CanvasGroup _canvasGroup = _menuObject.GetComponent<CanvasGroup>();
		while ( _canvasGroup.alpha < 1 )
		{
			increment = _speed * Time.deltaTime;
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
			increment = _speed * Time.deltaTime;
			if ( _canvasGroup.alpha - increment < 0 ) _canvasGroup.alpha = 0;
			else _canvasGroup.alpha -= increment;
			yield return null;
		}
		_menuObject.Active( false );
	}

	IEnumerator FadeInOut( MenuGroup _first, MenuGroup _second, float _speed, float _delay )
	{
		transitioning = true;
		float increment;

		CanvasGroup _canvasGroup = _first.menus[0].GetComponent<CanvasGroup>();
		while ( _canvasGroup.alpha > 0 )
		{
			increment = _speed * Time.deltaTime;
			if ( _canvasGroup.alpha - increment < 0 ) _canvasGroup.alpha = 0;
			else _canvasGroup.alpha -= increment;

			for ( int i = 0; i < _first.menus.Length; i++ )
				_first.menus[i].GetComponent<CanvasGroup>().alpha = _canvasGroup.alpha;

				yield return null;
		}
		_first.menus[0].Active( false );


		yield return new WaitForSeconds( _delay );

		currentMenu = _second;
		_canvasGroup = _second.menus[0].GetComponent<CanvasGroup>();
		while ( _canvasGroup.alpha < 1 )
		{
			increment = _speed * Time.deltaTime;
			if ( _canvasGroup.alpha + increment > 1 ) _canvasGroup.alpha = 1;
			else _canvasGroup.alpha += increment;

			for ( int i = 0; i < _second.menus.Length; i++ )
				_second.menus[i].GetComponent<CanvasGroup>().alpha = _canvasGroup.alpha;

			yield return null;
		}
		for ( int i = 0; i < _second.menus.Length; i++ )
			currentMenu.menus[i].Active( true );

		transitioning = false;


	}
}
