using UnityEngine;
using System.Collections;

/*========================================================
 * Author: Tyler Remazki
 *
 * Class : IState (Abstract Class)
 *
 * Description: Abstract class that all derived states use the functionality of.
 =========================================================*/

namespace PlayerStates
{
	public abstract class IState
	{
		protected PlayerFSM m_playerFSM;
		protected bool		m_attacking;

		/// <summary>
		/// Set the FSM that contains this State so that we can have access to reference variables.
		/// </summary>
		/// <param name="_fsm">The FSM Reference.</param>
		public void SetFSM( PlayerFSM _fsm )
		{
			m_playerFSM = _fsm;
		}

		/// <summary>
		/// Called when the current state in the FSM is switched to this state.
		/// </summary>
		public virtual void OnSwitch() { }

		/// <summary>
		/// Called when the current state in the FSM is switched from this state.
		/// </summary>
		public virtual void OnExit() { }

		/// <summary>
		/// Called when this state is the FSM's current state. 
		/// </summary>
		public abstract void ExecuteState();

		/// <summary>
		/// Used simply for organization purposes of keeping the state exit conditions within a single method call. 
		/// </summary>
		public virtual void ExitConditions() { } 

		/// <summary>
		/// Receive the current FSM that this state is contained within.
		/// </summary>
		/// <returns></returns>
		protected PlayerFSM GetFSM() { return m_playerFSM; }
	}
}
