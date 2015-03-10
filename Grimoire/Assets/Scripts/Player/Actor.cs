using UnityEngine;
using System.Collections;

/*========================================================
 * Author: Tyler Remazki
 *
 * Class : Actor
 *
 * Description: Actor holds all of the generic data necessary 
 * to each living actor in the game. Will also support some
 * utility functionality for the data structures within it
 =========================================================*/

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(MovementController))]
public class Actor : MonoBehaviour 
{
	public string 			            actorName;
    public Properties.ForceType forceType;

	//-- Private Properties -- //
	Properties 	                m_actorProperties;

	//-- Script References --//
	Rigidbody2D 			    m_rigidBody;
	BoxCollider2D 			    m_boxCollider;
	MovementController 	m_movementController;
	Animator 				        m_componentAnimator;
	InputHandler                m_inputHandler;
	
	void Start () 
	{
	    actorName 			            = "Default";
		m_actorProperties 		    = new Properties();
		m_boxCollider 			        = GetComponent(typeof(BoxCollider2D)) 		        as BoxCollider2D;
		m_movementController	= GetComponent(typeof(MovementController)) 	as MovementController;
		m_componentAnimator  = GetComponent(typeof(Animator)) 			            as Animator;
		m_inputHandler			    = GetComponent(typeof(InputHandler)) 		        as InputHandler;
	}
	
	#region Utility Functions
	public Properties AccessProperties
	{
		get { return m_actorProperties; }
		set { m_actorProperties = value;}
	}

	public string Name 
	{
		get { return actorName; }
		set { actorName = value;}
	}
    public Properties.ForceType Force
    {
        get { return forceType; }
        set { forceType = value; }
    }
	
	public BoxCollider2D 		    GetCollider() 			            { return m_boxCollider;			        }
	public Animator 			        GetAnimator() 			        { return m_componentAnimator;  	}
	public MovementController 	GetMovementController()   { return m_movementController; 	}
	public InputHandler 		        GetInputHandler() 		        { return m_inputHandler; 	        	}
	

	
	

	/// <summary>
	/// This utility function allows the actor to view the currently viewed components in the ComponentCollection
	/// script and add up the properties to get the Actors output properties.
	/// </summary>
	public void UpdateActorProperties()
	{
		m_actorProperties.Default();
	}

	/// <summary>
	/// Change the health value within the Actor's properties in accordance with the value parameter. Used to both damage and heal the actor. 
	/// </summary>
	public void IncrementHPBy(int _value)
	{
		m_actorProperties.Health += _value;
	}
	#endregion
}
