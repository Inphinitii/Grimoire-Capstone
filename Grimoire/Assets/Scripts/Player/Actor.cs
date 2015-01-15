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
	private string 					m_actorName;
	private Properties 				m_actorProperties;
	
	//-- Script References --//
	// This is for use within our components that affect our Actor.
	Rigidbody2D 			m_rigidBody;
	BoxCollider2D 			m_boxCollider;
	MovementController 		m_movementController;
	Animator 				m_componentAnimator;
	InputHandler            m_inputHandler;
	
	void Start () 
	{
		m_actorName 			= "Default";
		m_actorProperties 		= new Properties();
		m_boxCollider 			= GetComponent(typeof(BoxCollider2D)) 		as BoxCollider2D;
		m_movementController	= GetComponent(typeof(MovementController)) 	as MovementController;
		m_componentAnimator     = GetComponent(typeof(Animator)) 			as Animator;
		m_inputHandler			= GetComponent(typeof(InputHandler)) 		as InputHandler;
	}
	
	void Update () 
	{
	
	}

	#region Utility Functions
	public Properties AccessProperties
	{
		get { return m_actorProperties; }
		set { m_actorProperties = value;}
	}

	public string Name 
	{
		get { return m_actorName; }
		set { m_actorName = value;}
	}
	
	public BoxCollider2D 		GetCollider() 			{ return m_boxCollider;			}
	public Animator 			GetAnimator() 			{ return m_componentAnimator;  	}
	public MovementController 	GetMovementController() { return m_movementController; 	}
	public InputHandler 		GetInputHandler() 		{ return m_inputHandler; 		}
	

	
	

	/// <summary>
	/// This utility function allows the actor to view the currently viewed components in the ComponentCollection
	/// script and add up the properties to get the Actors output properties.
	/// </summary>
	public void UpdateActorProperties()
	{
		m_actorProperties.Default();
	}
	#endregion

	/// <summary>
	/// Change the health value within the Actor's properties in accordance with the value parameter. Used to both damage and heal the actor. 
	/// </summary>
	public void IncrementHPBy(int _value)
	{
		m_actorProperties.Health += _value;
	}

	/// <summary>
	/// Change the StunTime value within the Actor's properties in accordance with the value parameter. Used to incapacitate the Actor for a certain amount of time. 
	/// </summary>
	public void SetStunTime(int _value)
	{
		m_actorProperties.StunTime = _value;
	}

	//WE'RE GOING TO BE USING A MESSAGE CALLBACK SYSTEM IN ORDER TO IMPLEMENT CERTAIN FEATURES 
	//IE; ACTOR IS HIT : SEND ACTOR_HIT_MESSAGE
	//COMPONENT RECEIVED ACTOR_HIT_MESSAGE AND DOES SOMETHING ACCORDINGLY
}
