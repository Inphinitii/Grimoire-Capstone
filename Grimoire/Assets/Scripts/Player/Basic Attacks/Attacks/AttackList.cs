using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackList : MonoBehaviour {
	
	//Hold a dictionary containing a custom structure with an Attack and its Time, with the key being the string identifier. Ie; <HighPunch, new AttackStruct(AttackScriptObject,1.0f)>;
    public struct AttackStruct
    {
		public AttackStruct( AbstractAttack _ref, float _duration, float _startup, float _cooldown )
		{
			attackRef		= _ref;
			duration		= _duration;
			startup			= _startup;
			cooldown		= _cooldown;
		}
		public AbstractAttack attackRef;
        public float	duration,
							startup,
							cooldown;
    }

    [SerializeField]
    public string    attackListID;

    [SerializeField]
    public Properties.ForceType forceType;

    [SerializeField]
	public AbstractAttack[] attackList;

    [SerializeField]
    public string[] attackNames;

    [SerializeField]
    private Dictionary<string, AttackStruct> m_attackDictionary;

    private AttackStruct _temp;
    private AbstractAttack _tempAtk;

    void Start()
    {
        m_attackDictionary = new Dictionary<string, AttackStruct>();
        for ( int i = 0; i < attackList.Length; i++ )
        {
			_tempAtk = (AbstractAttack)Instantiate( attackList[i], this.transform.position, Quaternion.identity ) as AbstractAttack;
            _tempAtk.SetForce( forceType );
            _tempAtk.transform.parent = this.transform;
			_tempAtk.gameObject.SetActive( false );

            _temp = new AttackStruct(_tempAtk, _tempAtk.duration, _tempAtk.startupTime, _tempAtk.cooldownTime);
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
