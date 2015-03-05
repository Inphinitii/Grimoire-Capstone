using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackList : MonoBehaviour {
	
	//Hold a dictionary containing a custom structure with an Attack and its Time, with the key being the string identifier. Ie; <HighPunch, new AttackStruct(AttackScriptObject,1.0f)>;
    public Attack[] m_AttackList;
    public float[]    m_AttackCooldown;

	void Start () {
	}
	

	void Update () {
	
	}
}
