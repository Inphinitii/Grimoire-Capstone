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

    public Actor playerPrefab;
	public		    AbstractStage   stage;
	public static   GameManager     instance = null;
	public static   AbstractStage   stageObject;
	public static   Actor[]	        playerActors;

	private GameState _state = GameState.Menu;

	//Audio Options
	public static float sfxVolume = 0.1f;
	public static int musicVolume;

    private bool start, loading;

	void Awake()
	{
        loading = false;
        playerActors = new Actor[2];
		if ( instance == null )
			instance = this;
		else if ( instance != this )
			Destroy( gameObject );

		DontDestroyOnLoad( this );

        Actor _temp;
        playerActors[0] = _temp = (Actor)Instantiate( playerPrefab, Vector3.zero, Quaternion.identity );
        playerActors[0].gameObject.SetActive( false );
        playerActors[1] = _temp = (Actor)Instantiate( playerPrefab, Vector3.zero, Quaternion.identity );
        playerActors[1].gameObject.SetActive( false );


        
	}

	void LoadContent()
	{
        switch(_state)
        {
            case GameState.Menu:
                break;
            case GameState.InGame:
                AbstractStage _stage = (AbstractStage)Instantiate( stage, Vector3.zero, stage.transform.rotation ); //Instantiate the stage.
                stageObject = _stage;

                //Spawn Characters
                _stage.InitialSpawn( playerActors );

                //Set camera focus
                Camera.main.GetComponent<CameraScript>().cameraFoci = playerActors;
                Camera.main.GetComponent<CameraScript>().StartCamera();
                break;
            default:
                break;
        }
	}

    public void OnGameScene()
    {
        start = true;
        loading = true;
        Application.LoadLevel( 1 );
        _state = GameState.InGame;
    }


	void Update()
	{
        if ( Application.isLoadingLevel == false && loading )
        {
            loading = false;
            LoadContent();
        }

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
