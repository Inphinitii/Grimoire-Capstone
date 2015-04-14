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
	public static GameManager instance = null;
	public static AbstractStage stageObject;
	public static Actor[]				playerActors;

	//Audio Options
	public static int sfxVolume = 100;
	public static int musicVolume;

	void Awake()
	{
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
	}

	void Update()
	{

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
