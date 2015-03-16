using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*========================================================
 * Author: Tyler Remazki
 *
 * Class : Player Finite State Machine
 *
 * Description: Holds the states that the player can switch between. All of their interactivity is done
 * within the states themselves. This simply ensures that they're connected together. 
 =========================================================*/

public class PlayerFSM : MonoBehaviour {

    public enum States
    {
        STANDING,
        MOVING,
        CROUCHING,
        JUMPING,
        DASHING,
        ATTACKING,
        HIT,
        FALLING
    };
    public IState currentState;

   private List<IState> m_stateList;
   private bool m_block;

	private Actor m_actorReference;
    private AttackList[] m_attackList;
    
	void Start () {
        m_attackList			= GetComponents<AttackList>();
		m_actorReference	= GetComponent<Actor>();

        m_stateList = new List<IState>();
        
        // Pre-load the state objects into the state list. This is all done at compile time. 
        m_stateList.Add(new StandingState());
        m_stateList.Add(new MovementState());
        m_stateList.Add(new CrouchingState());
        m_stateList.Add(new JumpingState());

        foreach (IState item in m_stateList)
		{
            item.SetFSM(this); 
        }

		currentState = m_stateList[0]; 
	}
	
	//Make this update. Figure out why the animations are being retarded. 
	void Update () 
	{
		Debug.Log( currentState );
		currentState.ExecuteState();
	}


	/// <summary>
	/// Returns whether or not the current state is being blocked by an action.
	/// </summary>
    public bool Blocking 
	{ 
		get { return m_block; } 
		set { m_block = value; } 
	}

	/// <summary>
	/// Set the current state to that of the parameter as long as it is not currently being blocked.
	/// </summary>
	/// <param name="_states">The desired state.</param>
	/// <param name="_force">  Force the state change regardless of the block </param>
    public void SetCurrentState(States _states, bool _force){
        if (!m_block || _force)
        {
			currentState = m_stateList[(int)_states];
			currentState.OnSwitch();
        }
    }

	/// <summary>
	/// Function for use in the States that have no access to Unity functions. Call an IEnumerator through this GameObject.
	/// </summary>
	/// <param name="_coroutine">IEnumerator object.</param>
    public void StartChildCoroutine(IEnumerator _coroutine)
    {
        StartCoroutine(_coroutine);
    }

	

	/// <summary>
	/// Returns the Actor reference stored in this script.
	/// </summary>
	/// <returns>Actor Reference</returns>
	public Actor GetActorReference() { return m_actorReference; }

	/// <summary>
	/// Get the Input Handler from the Actor Reference. 
	/// </summary>
	/// <returns>Input Handler Reference</returns>
	public InputHandler GetInput()
	{
		return m_actorReference.GetInputHandler();
	}

	/// <summary>
	/// Get the Movement Controller from the Actor Reference. 
	/// </summary>
	/// <returns>Movement Controller Reference</returns>
	public MovementController GetMovement()
	{
		return m_actorReference.GetMovementController();
	}

	/// <summary>
	/// Get the Physics Controller from the Actor Reference	
	/// </summary>
	/// <returns>Physics Controller Reference</returns>
	public PhysicsController GetPhysics()
	{
		return m_actorReference.GetPhysicsController();
	}

	/// <summary>
	/// Return that attack list of the supplied ID.
	/// </summary>
	/// <param name="ID">Attack List Identifier.</param>
	/// <returns></returns>
    public AttackList GetAttackList( string ID )
    {
        for ( int i = 0; i < m_attackList.Length; i++ )
        {
            if ( m_attackList[i].GetID() == ID )
                return m_attackList[i];
        }
        return default( AttackList );
    }
}
