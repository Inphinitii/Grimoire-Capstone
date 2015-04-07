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

//TODO 
// HOOKUP PROPERTIES WITH MOVEMENT CONTROLLER VARIABLES
// HOOKUP PROPERTIES WITH HIT BOX VARIABLES
public class Properties
{
    public enum ForceType { BLUE = 0x00, RED = 0x01, GREEN = 0x02, YELLOW = 0x03 };
	private int 	p_entityHealth,
					    p_entityDurability;

    private float p_entityJumpHeight,
                        p_entityMovementSpeed,
                        p_entityAirMovementSpeed,
                        p_entityAttackSpeed;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public Properties()
    {
        Default();
    }

    /// <summary>
    /// Clears all of the values of the properties to 0.
    /// </summary>
	public void Clear()
	{
		p_entityHealth 				        = 0;
		p_entityDurability 			        = 0;
		p_entityAttackSpeed 		        = 0.0f;
		p_entityJumpHeight 			    = 0.0f;
		p_entityMovementSpeed 		= 0.0f;
		p_entityAirMovementSpeed	= 0.0f;
	}	
	
    /// <summary>
    /// Sets all of the values of the properties to be an arbitrary default value. 
    /// </summary>
	public void Default()
	{
		p_entityHealth 				        = 100;
		p_entityDurability 			        = 0;
		p_entityAttackSpeed 		        = 1.0f;
		p_entityJumpHeight 			    = 3.0f;
		p_entityMovementSpeed 		= 10.0f;
		p_entityAirMovementSpeed	= 6.0f;
	}	
	
    /// <summary>
    /// Properties addition operator overload. Adds two properties together. 
    /// </summary>
    /// <param name="_prop1"> The first property </param>
    /// <param name="_prop2"> The second property </param>
    /// <returns> The newly returned property with the sum of both passed properties. </returns>
	public static Properties operator+(Properties _prop1, Properties _prop2)
	{
		Properties _temp = new Properties();
		_temp.Health 			            = _prop1.Health 				                + _prop2.Health;
		_temp.Durability 		            = _prop1.Durability 			                + _prop2.Durability;
		_temp.JumpHeight 		        = _prop1.JumpHeight 			            + _prop2.JumpHeight;
		_temp.MovementSpeed 	    = _prop1.MovementSpeed 			+ _prop2.MovementSpeed;
		_temp.AirMovementSpeed 	= _prop1.AirMovementSpeed 		+ _prop2.AirMovementSpeed;
		return _temp;
	}

    /// <summary>
    /// Properties subtraction operator overload. Subtracts two properties. 
    /// </summary>
    /// <param name="_prop1"> The first property </param>
    /// <param name="_prop2"> The second property </param>
    /// <returns> The newly returned property with the sum of both passed properties. </returns>
	public static Properties operator-(Properties _prop1, Properties _prop2)
	{
		Properties _temp = new Properties();
		_temp.Health 			            = _prop1.Health 				                - _prop2.Health;
		_temp.Durability 		            = _prop1.Durability 			                - _prop2.Durability;
		_temp.JumpHeight 		        = _prop1.JumpHeight 			            - _prop2.JumpHeight;
		_temp.MovementSpeed 	    = _prop1.MovementSpeed 			- _prop2.MovementSpeed;
		_temp.AirMovementSpeed 	= _prop1.AirMovementSpeed 		- _prop2.AirMovementSpeed;
		return _temp;
	}

    /// <summary>
    /// Getters and setters for the Properties .. properties.
    /// </summary>
	public int   Health 			            {get{return p_entityHealth;} 	 		            set{p_entityHealth 				        = value;}}
	public int   Durability 		            {get{return p_entityDurability;} 		            set{p_entityDurability 			        = value;}}
	public float JumpHeight 		        {get{return p_entityJumpHeight;} 		        set{p_entityJumpHeight 			    = value;}}
	public float MovementSpeed 		{get{return p_entityMovementSpeed;} 	set{p_entityMovementSpeed 	= value;}}
	public float AirMovementSpeed 	{get{return p_entityAirMovementSpeed;} set{p_entityAirMovementSpeed	= value;}}
	public float AttackSpeed 		        {get{return p_entityAttackSpeed;} 		    set{p_entityAttackSpeed 		    = value;}}

}

