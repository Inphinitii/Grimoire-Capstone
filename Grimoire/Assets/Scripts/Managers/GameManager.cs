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

    public Actor                    playerPrefab;
	public		    AbstractStage   stage;
	public static   GameManager     instance = null;
	public static   AbstractStage   stageObject;
	public static   Actor[]	        playerActors;

    public static float gameTimer;
    //public static AbstractGameMode gameMode;

	private GameState _state = GameState.Menu;

	//Audio Options
	public static float sfxVolume = 0.1f;
	public static int musicVolume;

    private bool start, loading;

	void Start()
	{
        loading = false;
        playerActors = new Actor[2];
		if ( instance == null )
			instance = this;
		else if ( instance != this )
			Destroy( gameObject );

		DontDestroyOnLoad( this );

 
        playerActors[0] = (Actor)Instantiate( playerPrefab, Vector3.zero, Quaternion.identity );
        playerActors[0].forceType = Properties.ForceType.BLUE;
        playerActors[0].gameObject.SetActive( false );
        playerActors[1] = (Actor)Instantiate( playerPrefab, Vector3.zero, Quaternion.identity );
        playerActors[1].forceType = Properties.ForceType.RED;
        playerActors[1].gameObject.SetActive( false );

        playerActors[0].actorColor = Color.cyan;
        playerActors[1].actorColor = Color.red;

        DontDestroyOnLoad( playerActors[0] );
        DontDestroyOnLoad( playerActors[1] );


        
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

                foreach ( Actor obj in playerActors )
                {
                    obj.gameObject.SetActive( true );
                    obj.GetComponent<Grimoire>().Init();
                }

                //Set camera focus
                Camera.main.GetComponent<CameraScript>().cameraFoci = playerActors;
                Camera.main.GetComponent<CameraScript>().StartCamera();

				GameObject ui = GameObject.Find( "UserInterface" );

				for(int i = 0; i < ui.transform.childCount; i++)
                {
					if(ui.transform.GetChild(i).gameObject.name == "Player1")
					{
                        ui.transform.GetChild( i ).GetComponent<InGameSpellPanel>().referenceObject = playerActors[0].GetComponent<Grimoire>();
                        ui.transform.GetChild( i ).GetComponent<InGameSpellPanel>().StartUI();
					}
                    if ( ui.transform.GetChild( i ).gameObject.name == "Player2" )
					{
                        ui.transform.GetChild( i ).GetComponent<InGameSpellPanel>().referenceObject = playerActors[1].GetComponent<Grimoire>();
                        ui.transform.GetChild( i ).GetComponent<InGameSpellPanel>().StartUI();
                    }
                }

                playerActors[0].GetComponent<InputHandler>().m_playerNumber = 1;
                playerActors[1].GetComponent<InputHandler>().m_playerNumber = 2;

                //playerActors[0].actorColor = Color.cyan;
                playerActors[0].UpdateColor();

               // playerActors[1].actorColor = Color.red;
                playerActors[1].UpdateColor();


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
