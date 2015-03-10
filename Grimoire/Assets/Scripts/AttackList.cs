using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackList : MonoBehaviour {
	
	//Hold a dictionary containing a custom structure with an Attack and its Time, with the key being the string identifier. Ie; <HighPunch, new AttackStruct(AttackScriptObject,1.0f)>;

    public string    attackListID;
    public Properties.ForceType forceType;
    public Attack[] attackList;
    public float[]    attackCooldown;

	void Awake () {
        for ( int i = 0; i < attackList.Length; i++ )
        {
            attackList[i].forceType = forceType;    
        }
	}


	void Update () {
	
	}
}
