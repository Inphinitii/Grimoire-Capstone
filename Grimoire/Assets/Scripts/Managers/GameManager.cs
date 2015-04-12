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
	public AbstractStage stageObject;
	public Actor[]		 playerActors;

	void Start()
	{
		DontDestroyOnLoad( this );
	}

	void Update()
	{

	}
}
