using UnityEngine;
using System.Collections.Generic;
using ComponentLocation = ComponentCollection.ComponentLocation; //C# Hack of a TypeDef

/*=================================================================================================
 * Author: Tyler Remazki
 *
 * Class : Action Controller
 *
 * Description: The action controller dictates what happens when face buttons on the controller are hit.
 * This class acts as a layer of abstraction between InputHandler.cs and ComponentCollection.cs, as the
 * latter will hold components that house their own implementation of their OnUse() functions.
 =================================================================================================*/

[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(ComponentCollection))]
public class ActionController : MonoBehaviour
{
	InputHandler 			m_inputHandler;
	ComponentCollection 	m_componentCollection;
	float 					m_actorCooldown;

	void Start()
	{
		m_inputHandler 				= GetComponent<InputHandler>();
		m_componentCollection		= GetComponent<ComponentCollection>();
		m_actorCooldown 			= 0.0f;
	}

	void Update()
	{
		//TODO
		//Create a queue of moves
		if(m_actorCooldown > 0)
		{
			m_actorCooldown -= Time.deltaTime;
		}
		else if(m_actorCooldown <= 0)
		{			
			if(m_inputHandler.FreezeKeypress)
				m_inputHandler.FreezeKeypress = false;
				
			if(m_inputHandler.FreezeMovement)
				m_inputHandler.FreezeMovement = false;
				
			if(m_inputHandler.Attack())
			{
				GetActionProperties(m_componentCollection.GetComponent(ComponentLocation.HANDS).Action); // Cooldown
				m_componentCollection.GetComponent(ComponentLocation.HANDS).OnUse(); // Use Move
			}
			else if(m_inputHandler.Defend())
			{
				GetActionProperties(m_componentCollection.GetComponent(ComponentLocation.BODY).Action); // Cooldown
				m_componentCollection.GetComponent(ComponentLocation.BODY).OnUse();
			}
			else if(m_inputHandler.Utility())
			{
				GetActionProperties(m_componentCollection.GetComponent(ComponentLocation.HEAD).Action); // Cooldown
				m_componentCollection.GetComponent(ComponentLocation.HEAD).OnUse();
			}
			else if(m_inputHandler.Movement())
			{
				GetActionProperties(m_componentCollection.GetComponent(ComponentLocation.LEGS).Action); // Cooldown
				m_componentCollection.GetComponent(ComponentLocation.LEGS).OnUse();
			}
		}
	}
	
	void GetActionProperties(AbstractAction _action)
	{
		bool _keys, _movement;
		_action.GetFreezeValues(out _keys, out _movement);
		
		m_inputHandler.FreezeKeypress 	= _keys;
		m_inputHandler.FreezeMovement 	= _movement;
		m_actorCooldown 		= _action.ActorCooldown();
	}	

}