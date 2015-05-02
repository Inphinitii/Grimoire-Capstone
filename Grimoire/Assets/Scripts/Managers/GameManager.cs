using UnityEngine;
using UnityEngine.UI;
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
	public enum GameState { Menu, InGame, Pause, Win}
    public enum GameModeEnum { ComboKing }

    public struct Modifiers
    {
        public bool Platforms;
        public float Gravity;
        public float GameTime;
        
    }

    public          Actor           playerPrefab;
	public		    AbstractStage   stage;

    public          AbstractStage[] Stages;

	public static   GameManager     instance = null;
	public static   AbstractStage   stageObject;
	public static   Actor[]	        playerActors;
    public static   Modifiers       gameModifiers;

    GameObject _timerUI;

    public static float gameTimer;

    public static GameMode _gameMode;
    //public static AbstractGameMode gameMode;

	private GameState _state = GameState.Menu;

	//Audio Options
	public static float sfxVolume = 1.0f;
    public static float musicVolume = 0.1f;

    private bool start, loading;

	void Start()
	{
        loading = false;

        gameModifiers.Platforms = true;
        gameModifiers.GameTime = 20.0f;
        
        playerActors = new Actor[2];
		if ( instance == null )
			instance = this;
		else if ( instance != this )
			Destroy( gameObject );

		DontDestroyOnLoad( this );

        LoadContent();
        
	}

	void LoadContent()
	{
        switch(_state)
        {
            case GameState.Menu:

                FindObjectOfType<MenuManager>().ClearStack();

                playerActors = new Actor[2];
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

                break;
            case GameState.InGame:
                AbstractStage _stage = (AbstractStage)Instantiate( stage, Vector3.zero, stage.transform.rotation ); //Instantiate the stage.
                stageObject = _stage;

                //Enable/Disable platforms
                if ( !gameModifiers.Platforms )
                {
                    _stage.DisablePlatforms();
                }

                //Spawn Characters
                _stage.InitialSpawn( playerActors );
                _gameMode = new GMComboKing( playerActors );
                _gameMode._uiPrefab = FindObjectOfType<MenuManager>().ComboKingUI;
                _gameMode.SpawnUI();

                _timerUI = (GameObject) Instantiate( FindObjectOfType<MenuManager>().TimerUI, Vector3.zero, Quaternion.identity );

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

                playerActors[0].GetComponent<InputHandler>().AssignNumber = true;
                playerActors[1].GetComponent<InputHandler>().AssignNumber = true;

                playerActors[0].GetComponent<InputHandler>().UID = 1;
                playerActors[1].GetComponent<InputHandler>().UID = 2;

                playerActors[0].actorName = "Player 1";
                playerActors[1].actorName = "Player 2";


                //playerActors[0].actorColor = Color.cyan;
                playerActors[0].UpdateColor();

               // playerActors[1].actorColor = Color.red;
                playerActors[1].UpdateColor();


                break;
            default:
                break;
        }
	}

    public void OnMenuScene()
    {
        Destroy( playerActors[0].gameObject );
        Destroy( playerActors[1].gameObject );
        Destroy( stageObject.gameObject );

        _state = GameState.Menu;
        start = true;
        loading = true;
    }
    public void OnGameScene()
    {
        start = true;
        loading = true;

        //GetComponent<SceneFadeInOut>().EndScene( 1 );
        Application.LoadLevel( 1 );
        _state = GameState.InGame;
    }

    public void OnPause()
    {

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
                //After countdown..
                _gameMode.Update();
			}
			start = false;
		}


            if ( _state == GameState.InGame )
            {
                //After countdown..
                if ( !_gameMode.GameEnd() )
                {
                    int displayMinutes = (int)( GameManager._gameMode.GetTimer() / 60.0f );
                    int displaySeconds = (int)( GameManager._gameMode.GetTimer() % 60.0f );

                    _timerUI.GetComponentInChildren<Text>().text = string.Format( "{0}:{1:00}", displayMinutes, displaySeconds );
                    _gameMode.Update();
                }
                else
                {
                    GameObject _obj = (GameObject)Instantiate( FindObjectOfType<MenuManager>().WinBanner, Vector3.zero, Quaternion.identity );
                    if(_gameMode.winnerActor != null)
                     _obj.GetComponentInChildren<Text>().text = _gameMode.winnerActor.actorName + " wins!";
                    else
                     _obj.GetComponentInChildren<Text>().text = "Tie!";
                        
                    _state = GameState.Win;

                }
            }

            if ( _state == GameState.Win )
            {
                if ( GamepadInput.GamePad.GetButtonDown( GamepadInput.GamePad.Button.Start, GamepadInput.GamePad.Index.Any ) )
                {
                    OnMenuScene();
                    Application.LoadLevel( 0 );
                }
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

    public static string ReturnGameModeDescription(GameModeEnum _gameMode)
    {
        switch(_gameMode)
        {
            case GameModeEnum.ComboKing:
                GameMode _temp = new GMComboKing();
                return _temp.GetDescription();
              
        }
        return null;
    }

    public static void PauseInput(bool _pause)
    {
        for ( int i = 0; i < GetAllActors().Length; i++ )
        {
            GetAllActors()[i].GetComponent<InputHandler>().FreezeAll = _pause;
        }
    }
}
