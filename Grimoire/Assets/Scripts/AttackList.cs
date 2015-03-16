using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackList : MonoBehaviour {
	
	//Hold a dictionary containing a custom structure with an Attack and its Time, with the key being the string identifier. Ie; <HighPunch, new AttackStruct(AttackScriptObject,1.0f)>;
    public struct AttackStruct
    {
        public Attack attackRef;
        public float    attackTime,
                             attackCD;
    }

    [SerializeField]
    public string    attackListID;

    [SerializeField]
    public Properties.ForceType forceType;

    [SerializeField]
    public Attack[] attackList;

    [SerializeField]
    public string[] attackNames;

    [SerializeField]
    public float[] attackLength;

    [SerializeField]
    public float[]   attackCooldown;

    [SerializeField]
    private Dictionary<string, AttackStruct> m_attackDictionary;

    private AttackStruct _temp;
    private Attack _tempAtk;

    void Start()
    {
        //Generate Attack Structs and push them into the Attack Dictionary.
        m_attackDictionary = new Dictionary<string, AttackStruct>();
        for ( int i = 0; i < attackList.Length; i++ )
        {
            _tempAtk = (Attack)Instantiate( attackList[i], this.transform.position, Quaternion.identity ) as Attack;
            _tempAtk.SetForce( forceType );
            _tempAtk.position = this.transform;
            _tempAtk.transform.parent = this.transform;
			_tempAtk.gameObject.active = false;

            _temp = new AttackStruct();
            _temp.attackRef = _tempAtk;
            _temp.attackCD = attackCooldown[i];
            _temp.attackTime = attackLength[i];

            m_attackDictionary.Add( attackNames[i], _temp );
        }
	}


	void Update () {
	
	}

    public string GetID() { return attackListID; }
    public AttackStruct GetAttack( string _identifier )
    {
        _temp = new AttackStruct();
        if ( !m_attackDictionary.TryGetValue( _identifier, out _temp ) )
            Debug.Log( "Can not find attack by the name of " + _identifier + " in the Attack List of UID: " + attackListID );
        return _temp;
    }
}
