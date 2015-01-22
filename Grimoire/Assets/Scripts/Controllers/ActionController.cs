using UnityEngine;
using System.Collections.Generic;

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
public class ActionController : MonoBehaviour
{
	InputHandler 			m_inputHandler;
	float 					m_actorCooldown;
	
	//TODO 
	//Associate the player-equipped Grimoire with the ActionController
	
	void Start()
	{
		m_inputHandler 				= GetComponent<InputHandler>();;
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
				
			//TODO 
			//Use the Grimoire pages under these buttons.
			if(m_inputHandler.Y())
			{
                //Kick
			}
			else if(m_inputHandler.A())
			{
                //Jump
			}
			else if(m_inputHandler.X())
			{
                //Punch
			}
			else if(m_inputHandler.B())
			{
                //Use Currently Selected Page
			}
            else if(m_inputHandler.RB())
            {
                //Switch Page -> Right
            }
            else if (m_inputHandler.LB())
            {
                //Switch Page <- Left
            }

		}
	}
	
	/*
	void GetActionProperties(AbstractAction _action)
	{
		bool _keys, _movement;
		_action.GetFreezeValues(out _keys, out _movement);
		
		m_inputHandler.FreezeKeypress 	= _keys;
		m_inputHandler.FreezeMovement 	= _movement;
		m_actorCooldown 		= _action.ActorCooldown();
	}	
	*/

}