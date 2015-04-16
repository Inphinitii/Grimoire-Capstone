using UnityEngine;
using System.Collections;

public class SpawnDistance : AbstractSpawn {

	// Use this for initialization
	public override void Start()
	{
		base.Start();
	}
	
	// Update is called once per frame
	public override void Update()
	{
		base.Update();
	}

    public override void ActivateSpell()
    {
    }

    public override void UseIncantation(Force.ForceType _force)
    {
        throw new System.NotImplementedException();
    }

    public override void ReleaseIncantation(Force.ForceType _force)
    {
        throw new System.NotImplementedException();
    }
}
