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

	public AbstractMenu mainMenu;
	public AbstractMenu optionsMenu;
	public AbstractMenu characterSelectionMenu;

	AbstractMenu currentMenu;
	AbstractMenu previousMenu;

	private bool transitioning;


	// Use this for initialization
	void Start()
	{
		currentMenu = mainMenu;
		transitioning = false;
	}

	// Update is called once per frame
	void Update()
	{
	}

	public void SwitchOptions()
	{
		if ( !transitioning )
		{
			previousMenu = currentMenu;
			StartCoroutine( FadeInOut( currentMenu, optionsMenu, 1.0f, 0.0f ) );
		}
	}

	public void GoBack()
	{
		if ( previousMenu != null && !transitioning )
		StartCoroutine( FadeInOut( currentMenu, previousMenu, 1.0f, 0.0f ) );
	}

	IEnumerator FadeIn( AbstractMenu _menuObject, float _speed )
	{
		float increment;
		_menuObject.gameObject.SetActive( true );
		CanvasGroup _canvasGroup = _menuObject.GetComponent<CanvasGroup>();
		while ( _canvasGroup.alpha < 1 )
		{
			increment = _speed * Time.deltaTime;
			if ( _canvasGroup.alpha + increment > 1 ) _canvasGroup.alpha = 1;
			else _canvasGroup.alpha += increment;
			yield return null;
		}
	}

	IEnumerator FadeOut( AbstractMenu _menuObject, float _speed )
	{
		float increment;
		CanvasGroup _canvasGroup = _menuObject.GetComponent<CanvasGroup>();
		while ( _canvasGroup.alpha > 0 )
		{
			increment = _speed * Time.deltaTime;
			if ( _canvasGroup.alpha - increment < 0 ) _canvasGroup.alpha = 0;
			else _canvasGroup.alpha -= increment;
			yield return null;
		}
		_menuObject.gameObject.SetActive( false );
	}

	IEnumerator FadeInOut( AbstractMenu _first, AbstractMenu _second, float _speed, float _delay)
	{
		transitioning = true;
		float increment;
		CanvasGroup _canvasGroup = _first.GetComponent<CanvasGroup>();
		while ( _canvasGroup.alpha > 0 )
		{
			increment = _speed * Time.deltaTime;
			if ( _canvasGroup.alpha - increment < 0 ) _canvasGroup.alpha = 0;
			else _canvasGroup.alpha -= increment;
			yield return null;
		}
		_first.Active( false );

		yield return new WaitForSeconds( _delay );


		_canvasGroup = _second.GetComponent<CanvasGroup>();
		while ( _canvasGroup.alpha < 1 )
		{
			increment = _speed * Time.deltaTime;
			if ( _canvasGroup.alpha + increment > 1 ) _canvasGroup.alpha = 1;
			else _canvasGroup.alpha += increment;
			yield return null;
		}
		currentMenu = _second;
		currentMenu.Active( true );
		transitioning = false;

	}
}
