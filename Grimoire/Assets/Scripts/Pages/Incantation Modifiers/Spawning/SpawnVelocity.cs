using UnityEngine;
using System.Collections;

public class SpawnVelocity : AbstractSpawn {

    public float       ObjectSpeed;
    public bool        Forward;
    public Vector2  Direction;

    private Rigidbody2D obj;

	// Use this for initialization
	public override void Start () {
        base.Start();
        if (Forward)
            Direction = gameObjectSpawnLocation.right;
	}
	
	// Update is called once per frame
	public override void Update()
	{
		base.Update();
	}

    public override void ActivateSpell()
    {
        if (!spawnInstantly)
            obj = (Rigidbody2D)Instantiate(gameObjectToSpawn, pSpawnPosition, Quaternion.identity) as Rigidbody2D;

       obj.gameObject.AddComponent<Force>().ItemForce = pForceType;
       obj.velocity = Direction * ObjectSpeed;
    }

    public override void UseIncantation(Force.ForceType _force)
    {
        if (spawnInstantly)
            obj = (Rigidbody2D)Instantiate(gameObjectToSpawn, pSpawnPosition, Quaternion.identity) as Rigidbody2D;
        pForceType = _force;
    }

    public override void ReleaseIncantation(Force.ForceType _force)
    {
    }
}
