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
		public AbstractAttack	attackRef;
        public float		duration,
								startup,
								cooldown;
    }

    [SerializeField]
    public string attackListID;
	public bool useActorForce;

    [SerializeField]
    public Properties.ForceType forceType;

    [SerializeField]
	public AbstractAttack[] attackList;

    [SerializeField]
    public string[] attackNames;


    [SerializeField]
    private Dictionary<string, AttackStruct> m_attackDictionary;

    private AttackStruct	_temp;
    private AbstractAttack	_tempAtk;

    void Start()
    {
		if ( useActorForce )
			forceType = GetComponent<Actor>().forceType;

        m_attackDictionary = new Dictionary<string, AttackStruct>();
        for ( int i = 0; i < attackList.Length; i++ )
        {
			_tempAtk = (AbstractAttack)Instantiate( attackList[i], this.transform.position, Quaternion.identity ) as AbstractAttack;
            _tempAtk.transform.parent = this.transform;
			_tempAtk.forceType = forceType;

            _temp = new AttackStruct(_tempAtk, _tempAtk.Duration(), _tempAtk.Startup(), _tempAtk.Cooldown());
            m_attackDictionary.Add( attackNames[i], _temp );
        }
	}


	void Update () {
	
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
    public AttackStruct GetAttack( string _identifier )
    {
        _temp = new AttackStruct();
        if ( !m_attackDictionary.TryGetValue( _identifier, out _temp ) )
            Debug.Log( "Can not find attack by the name of " + _identifier + " in the Attack List of UID: " + attackListID );
        return _temp;
    }

	///// <summary>
	///// Set the force of the attack objects within the attack list. Used externally. 
	///// </summary>
	///// <param name="_force"></param>
	//public void SetForceType(Properties.ForceType _force)
	//{
	//	forceType = _force;
	//	for ( int i = 0; i < attackList.Length; i++ )
	//		attackList[i].SetForce( forceType );
	//}
}
