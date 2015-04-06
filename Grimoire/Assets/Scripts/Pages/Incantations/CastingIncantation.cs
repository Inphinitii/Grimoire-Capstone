using UnityEngine;
using System.Collections;

/* Casting Incantation
 * Handles a normal spell. Player stops during cast time and is vulnerable during this state.
 * 
 *
 * */
public class CastingIncantation : AbstractIncantation
{

	public float CastingTime;

	//public Properties pPropertiesReference;

	private bool startTimer;
	private bool spellCast;
	private bool interrupted;

	public override void Start()
	{
		base.Start();
		startTimer = false;
		interrupted = false;
		HandleUse();

	}

	public override void Update()
	{
		if ( startTimer && !spellCast )
			StartCoroutine( FireEvent() );
	}

	public override void HandleUse()
	{
		base.HandleUse();
		if ( !startTimer )
		{
			startTimer = true;
		}
	}

	public override void HandleRelease()
	{
		base.HandleRelease();
	}

	IEnumerator FireEvent()
	{
		spellCast = true;
		BroadcastMessage( "Casting", true );

		if ( !interrupted )
			yield return new WaitForSeconds( CastingTime );
		else
		{
			startTimer = false;
			spellCast = false;
			yield break;
		}

		BroadcastMessage( "Casting", false );
		SendMessage( "ActivateSpell" );

	}
}
