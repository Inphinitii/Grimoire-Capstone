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
[RequireComponent(typeof(MovementController))]
public class ActionController : MonoBehaviour
{
	InputHandler 			m_inputHandler;
    MovementController      m_movementController;
    //PlayerFSM m_stateMachine;
	float 					m_actorCooldown;

    public bool             m_regularAttack;
    public bool             m_smashAttack;

    public float            m_chargeDuration,
                            m_attackDuration = 0.4f; // Padding for the animation played
	//TODO 
	//Associate the player-equipped Grimoire with the ActionController
	
	void Start()
	{
		m_inputHandler 				= GetComponent<InputHandler>();
        m_movementController        = GetComponent<MovementController>();
        //m_stateMachine = GetComponent<PlayerFSM>();
		m_actorCooldown 			= 0.0f;
	}

	void Update()
	{
		//TODO
		//Create a queue of moves
        m_actorCooldown = m_chargeDuration + m_attackDuration;
		if(m_actorCooldown > 0)
		{
            if (m_chargeDuration > 0.0f)
                m_chargeDuration -= Time.deltaTime;
            else
                m_attackDuration -= Time.deltaTime;
		}
		else if(m_actorCooldown <= 0)
		{			
			if(m_inputHandler.FreezeKeypress)
				m_inputHandler.FreezeKeypress = false;
				
			if(m_inputHandler.FreezeMovement)
				m_inputHandler.FreezeMovement = false;

            if (m_smashAttack)
                m_smashAttack = false;

            //m_stateMachine.m_currentState.HandleInput(m_inputHandler);
            //TODO 
            //Use the Grimoire pages under these buttons.
            if (m_inputHandler.Y())
            {
            }
            else if (m_inputHandler.A())
            {
              //  m_movementController.ApplyJump();
            }
            else if (m_inputHandler.X())
            {
                m_movementController.ApplyDash();
            }
            else if (m_inputHandler.B())
            {
                BroadcastMessage("OnFire");
                //Use Currently Selected Page
            }

            if (m_inputHandler.RB())
            {
                //Switch Page -> Right
            }
            else if (m_inputHandler.LB())
            {
                //Switch Page <- Left
            }

            if (m_inputHandler.FSmash())
            {
                m_smashAttack = true;
                m_chargeDuration = 0.5f;
                m_attackDuration = 0.75f;
            }

		}
	}
}