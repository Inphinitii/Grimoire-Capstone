using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicAttacks : MonoBehaviour
{

	//Hold a dictionary containing a custom structure with an Attack and its Time, with the key being the string identifier. Ie; <HighPunch, new AttackStruct(AttackScriptObject,1.0f)>;
	[SerializeField]
	public string attackListID;
	public bool useActorForce;

	public enum Attacks
	{
		CROUCHING_ATTACK,
		STANDING_NEUTRAL,
		STANDING_AIR,
		AIR_NEUTRAL,
		AIR_BACK,
		AIR_FRONT,
		AIR_DOWN
	}

	[SerializeField]
	public Properties.ForceType forceType;

	[SerializeField]
	public AbstractAttack crouchingAttack;
	public AbstractAttack standingNeutral;
	public AbstractAttack standingAir;
	public AbstractAttack airNeutral;
	public AbstractAttack airBack;
	public AbstractAttack airFront;
	public AbstractAttack airDown;



	[SerializeField]
	public string[] attackNames;


	[SerializeField]
	private AbstractAttack[] attackList;
	private AbstractAttack _tempAtk;


	void Start()
	{
		attackList = new AbstractAttack[7];
		attackList[0] = crouchingAttack;
		attackList[1] = standingNeutral;
		attackList[2] = standingAir;
		attackList[3] = airNeutral;
		attackList[4] = airBack;
		attackList[5] = airFront;
		attackList[6] = airDown;


		if ( useActorForce )
			forceType = GetComponent<Actor>().forceType;

		for ( int i = 0; i < attackList.Length; i++ )
		{
			_tempAtk = (AbstractAttack)Instantiate( attackList[i], this.transform.position, Quaternion.identity ) as AbstractAttack;
			_tempAtk.transform.parent = this.transform;
			_tempAtk.forceType = forceType;
			attackList[i] = _tempAtk;
		}
	}


	void Update()
	{

	}

	/// <summary>
	/// Returns the ID associated with this attack list.
	/// </summary>
	/// <returns>Identifier</returns>
	public string GetID() { return attackListID; }

	/// <summary>
	/// Returns a specific attack with the associated name.
	/// </summary>
	/// <param name="_identifier">Identifier</param>
	/// <returns></returns>
	public AbstractAttack GetAttack( Attacks _type )
	{
		return attackList[(int)_type];
	}
}
