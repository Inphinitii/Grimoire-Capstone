using UnityEngine;
using System.Collections;

/*========================================================
 * Author: Tyler Remazki
 *
 * Class : Properties
 *
 * Description: Holds the characteristics of components
 * and actors
 =========================================================*/

public class Properties : MonoBehaviour
{
    public enum PlayerState {
        NONE,
        INVULNERABLE,
        STUNNED,
        CASTING,
    }

	private int 	p_entityHealth,
					    p_entityDurability;
	
	private float	p_entityJumpHeight,
					p_entityMovementSpeed,
					p_entityAirMovementSpeed,
					p_entityAttackSpeed,
					p_entityStunTime,  //Stun timer will be used for different purposes between Actors and Components
                    p_entityCastTime;

    private PlayerState m_currentState;

    public Properties()
    {
    	Default();
        m_currentState = PlayerState.NONE;
    }

	public void Clear()
	{
		p_entityHealth 				= 0;
		p_entityDurability 			= 0;
		p_entityAttackSpeed 		= 0.0f;
		p_entityJumpHeight 			= 0.0f;
		p_entityMovementSpeed 		= 0.0f;
		p_entityAirMovementSpeed	= 0.0f;
		p_entityStunTime 			= 0.0f;
        p_entityCastTime            = 0.0f;
	}	
	
	public void Default()
	{
		p_entityHealth 				= 100;
		p_entityDurability 			= 0;
		p_entityAttackSpeed 		= 1.0f;
		p_entityJumpHeight 			= 3.0f;
		p_entityMovementSpeed 		= 10.0f;
		p_entityAirMovementSpeed	= 6.0f;
		p_entityStunTime 			= 0.0f;
        p_entityCastTime            = 0.0f;
	}	
	
	public static Properties operator+(Properties _prop1, Properties _prop2)
	{
		Properties _temp = new Properties();
		_temp.Health 			= _prop1.Health 				+ _prop2.Health;
		_temp.Durability 		= _prop1.Durability 			+ _prop2.Durability;
		_temp.JumpHeight 		= _prop1.JumpHeight 			+ _prop2.JumpHeight;
		_temp.MovementSpeed 	= _prop1.MovementSpeed 			+ _prop2.MovementSpeed;
		_temp.AirMovementSpeed 	= _prop1.AirMovementSpeed 		+ _prop2.AirMovementSpeed;
		_temp.AttackSpeed 		= _prop1.AttackSpeed 			+ _prop2.AttackSpeed;
		_temp.StunTime 			= _prop1.StunTime 				+ _prop2.StunTime;
		return _temp;
	}

	public static Properties operator-(Properties _prop1, Properties _prop2)
	{
		Properties _temp = new Properties();
		_temp.Health 			= _prop1.Health 				- _prop2.Health;
		_temp.Durability 		= _prop1.Durability 			- _prop2.Durability;
		_temp.JumpHeight 		= _prop1.JumpHeight 			- _prop2.JumpHeight;
		_temp.MovementSpeed 	= _prop1.MovementSpeed 			- _prop2.MovementSpeed;
		_temp.AirMovementSpeed 	= _prop1.AirMovementSpeed 		- _prop2.AirMovementSpeed;
		_temp.AttackSpeed 		= _prop1.AttackSpeed 			- _prop2.AttackSpeed;
		_temp.StunTime 			= _prop1.StunTime 				- _prop2.StunTime;
		return _temp;
	}

	public int   Health 			{get{return p_entityHealth;} 	 		set{p_entityHealth 				= value;}}
	public int   Durability 		{get{return p_entityDurability;} 		set{p_entityDurability 			= value;}}
	public float JumpHeight 		{get{return p_entityJumpHeight;} 		set{p_entityJumpHeight 			= value;}}
	public float MovementSpeed 		{get{return p_entityMovementSpeed;} 	set{p_entityMovementSpeed 		= value;}}
	public float AirMovementSpeed 	{get{return p_entityAirMovementSpeed;} 	set{p_entityAirMovementSpeed	= value;}}
	public float AttackSpeed 		{get{return p_entityAttackSpeed;} 		set{p_entityAttackSpeed 		= value;}}
	public float StunTime 			{get{return p_entityStunTime;} 			set{p_entityStunTime 			= value;}}
    public float CastTime           {get{return p_entityCastTime; }         set{p_entityCastTime            = value;}}
    public PlayerState State { get { return m_currentState; } set { m_currentState = value; } }

	
	//MONO BEHAVIOUR DEPENDENCIES
	void Start()
	{
        Default();
        m_currentState = PlayerState.NONE;
	}

    void Update() {
        if (p_entityCastTime > 0.0f) {
            p_entityCastTime -= Time.deltaTime;
        }
        else {
            m_currentState = PlayerState.NONE;
        }
    }
}

