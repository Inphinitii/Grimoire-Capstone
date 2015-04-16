using UnityEngine;
using System.Collections;

public class HoldingIncantation : AbstractIncantation
{

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

	public override void HandleUse()
	{
		base.HandleUse();
		BroadcastMessage( "Casting", true );
	}

	public override void HandleRelease()
	{
		base.HandleRelease();
		BroadcastMessage( "Casting", false );
		SendMessage( "ActivateSpell" );
	}
}
