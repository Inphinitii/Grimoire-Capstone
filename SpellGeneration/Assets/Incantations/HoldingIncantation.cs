using UnityEngine;
using System.Collections;

public class HoldingIncantation : AbstractIncantation {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void HandleUse()
    {
        base.HandleUse();
        BroadcastMessage("Casting", true);
    }

    public override void HandleRelease()
    {
        base.HandleRelease();
        BroadcastMessage("Casting", false);
        SendMessage("ActivateSpell");
    }
}
