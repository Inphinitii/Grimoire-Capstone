using UnityEngine;
using System.Collections;

/*========================================================
 * Author: Tyler Remazki
 *
 * Class : IState (Abstract Class)
 *
 * Description: Abstract class that all derived states use the functionality of.
 =========================================================*/

public abstract class IState
{
    protected PlayerFSM m_playerFSM;

	/// <summary>
	/// Set the FSM that contains this State so that we can have access to reference variables.
	/// </summary>
	/// <param name="_fsm">The FSM Reference.</param>
    public void SetFSM(PlayerFSM _fsm)
	{
        m_playerFSM = _fsm;
    }

	/// <summary>
	/// Block the state switch for X seconds.
	/// </summary>
	/// <param name="_time">Time to block the state switch for.</param>
	/// <returns></returns>
    public IEnumerator BlockStateSwitch(float _time)
    {
        m_playerFSM.Blocking = true;
        yield return new WaitForSeconds(_time);
        m_playerFSM.Blocking = false;
    }

	/// <summary>
	/// Called when the current state in the FSM is switched to this state.
	/// </summary>
    public virtual void OnSwitch() { }

	/// <summary>
	/// Called when this state is the FSM's current state. 
	/// </summary>
    public abstract void ExecuteState();

	/// <summary>
	/// Receive the current FSM that this state is contained within.
	/// </summary>
	/// <returns></returns>
    protected PlayerFSM GetFSM() { return m_playerFSM; }
}
