using UnityEngine;
using System.Collections;

public class AbstractStage : MonoBehaviour
{
	public BoxCollider2D[] platformObjects;
	public Transform[]	spawnLocations; //Main spawn location will be at index 0.

	public virtual void Start()
	{

	}

	public virtual void Update()
	{

	}

	public virtual Transform GetRandomSpawnLocation(bool _main)
	{
		if ( _main )
		{
			return spawnLocations[0];
		}
		else
			return spawnLocations[Random.Range( 0, spawnLocations.Length )];
	}

    //Takes two players.
    public virtual void InitialSpawn( Actor[] _actors )
    {
        _actors[0].transform.position = spawnLocations[0].position;
        _actors[1].transform.position = spawnLocations[1].position;

    }

	public virtual BoxCollider2D[] GetPlatforms()
	{
		return platformObjects;
	}

    public void DisablePlatforms()
    {
        foreach ( BoxCollider2D obj in platformObjects )
        {
            Debug.Log( obj.transform.parent );
            obj.transform.parent.gameObject.SetActive( false );
        }
    }
}
