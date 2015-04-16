using UnityEngine;
using System.Collections;

[RequireComponent( typeof( Force ) )]
public abstract class AbstractSpawn : MonoBehaviour
{

	public Rigidbody2D	gameObjectToSpawn;
	public Transform		gameObjectSpawnLocation;
	public Vector2			spawnOffset;
	public bool				spawnInstantly;

	protected Vector2 pSpawnPosition;

	public virtual void Start()
	{
		pSpawnPosition = (Vector2)gameObjectSpawnLocation.position + spawnOffset;
	}

	public virtual void Update()
	{

	}

	protected Force.ForceType pForceType;

	public abstract void ActivateSpell();             //Used after a period of time.
	public abstract void UseIncantation( Force.ForceType _force );         //Used Instantly
	public abstract void ReleaseIncantation( Force.ForceType _force );  //Used when the button is released.
}
