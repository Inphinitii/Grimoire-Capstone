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

	public virtual Transform GetSpawnLocation(bool _main)
	{
		if ( _main )
		{
			return spawnLocations[0];
		}
		else
			return spawnLocations[Random.Range( 0, spawnLocations.Length )];
	}
	public virtual BoxCollider2D[] GetPlatforms()
	{
		return platformObjects;
	}
}
