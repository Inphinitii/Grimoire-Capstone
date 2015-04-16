using UnityEngine;
using System.Collections;

/*==================================================================================
 * Author: Tyler Remazki
 *
 * Class : Game Manager
 *
 * Description: Used to carry necessary data across scenes.
 =================================================================================*/

public class GameManager : MonoBehaviour
{
	public enum GameState { Menu, InGame, Pause }

	public			AbstractStage stage;
	public static GameManager instance = null;
	public static AbstractStage stageObject;
	public static Actor[]				playerActors;

	private GameState _state = GameState.InGame;
	//Audio Options
	public static float sfxVolume = 0.1f;
	public static int musicVolume;

	private bool start;

	void Awake()
	{
		stageObject = stage;
		if ( instance == null )
			instance = this;
		else if ( instance != this )
			Destroy( gameObject );

		DontDestroyOnLoad( this );
	}
	void Start()
	{
		//Temporary hook up
		playerActors = FindObjectsOfType<Actor>();
		start = true;
	}

	void Update()
	{
		if ( start )
		{
			if ( _state == GameState.InGame )
			{
				if ( playerActors[playerActors.Length - 1].GetComponent<Grimoire>().loaded )
					foreach ( InGameSpellPanel obj in GameObject.Find( "User Interface" ).transform.GetComponentsInChildren<InGameSpellPanel>() )
						obj.StartUI();

			}
			start = false;
		}
	}

	public static Actor[] GetAllActors()
	{
		return playerActors;
	}

	public static AbstractStage GetStageObject()
	{
		return stageObject;
	}
}
