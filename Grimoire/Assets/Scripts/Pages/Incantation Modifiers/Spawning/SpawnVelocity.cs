using UnityEngine;
using System.Collections;

public class SpawnVelocity : AbstractSpawn {

    public float       ObjectSpeed;
    public bool        Forward;
    public Vector2  Direction;

    private Rigidbody2D obj;

	// Use this for initialization
	void Start () {
        if (Forward)
            Direction = pGameObjectSpawnLocation.right;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void ActivateSpell()
    {
        if (!pSpawnInstantly)
            obj = (Rigidbody2D)Instantiate(pGameObjectToSpawn, pGameObjectSpawnLocation.transform.position, Quaternion.identity) as Rigidbody2D;

       obj.gameObject.AddComponent<Force>().ItemForce = pForceType;
       obj.velocity = Direction * ObjectSpeed;
    }

    public override void UseIncantation(Force.ForceType _force)
    {
        if (pSpawnInstantly)
            obj = (Rigidbody2D)Instantiate(pGameObjectToSpawn, pGameObjectSpawnLocation.transform.position, Quaternion.identity) as Rigidbody2D;
        pForceType = _force;
    }

    public override void ReleaseIncantation(Force.ForceType _force)
    {
    }
}
