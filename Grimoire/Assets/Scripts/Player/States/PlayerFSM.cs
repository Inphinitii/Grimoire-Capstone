using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerStates;

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
        ATTACKING,
        HIT,
		BOUNCE,
        DASHING,
        FALLING,
		LANDING
    };
    public IState currentState;

    private List<IState>	m_stateList;
    private bool					m_block;

    private IState			m_previousState;
	private Actor			m_actorReference;
	private BasicAttacks m_attackList;

	//For use in the AttackState class. 
	private AbstractAttack m_currentAttack;

	private float m_blockTimer;
    
	void Start () {
		m_attackList			= GetComponent<BasicAttacks>();
		m_actorReference	= GetComponent<Actor>();
        m_stateList				= new List<IState>();
        
        // Pre-load the state objects into the state list. This is all done at compile time. 
        m_stateList.Add( new StandingState() );
        m_stateList.Add( new MovementState() );
        m_stateList.Add( new CrouchingState() );
        m_stateList.Add( new JumpingState() );
		m_stateList.Add( new AttackState() );
		m_stateList.Add( new HitState() );
		m_stateList.Add( new BounceState() );
		m_stateList.Add( new DashingState() );


        foreach (IState item in m_stateList)
		{
            item.SetFSM(this); 
        }

		currentState = m_stateList[0]; 


	}
	
	void Update () 
	{
		currentState.ExecuteState();
		currentState.ExitConditions();
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
			m_block					= false;
			m_blockTimer			= 0.0f;
			m_previousState		= currentState;

			currentState.OnExit();
			currentState = m_stateList[(int)_states];
			currentState.OnSwitch();
        }
    }

	/// <summary>
	/// Return to the previous stored state.
	/// </summary>
	/// <param name="_force">Force the state change regardless of the block.</param>
	public void GoToPreviousState(bool _force)
	{
		if ( !m_block || _force )
		{
			m_block				= false;
			m_blockTimer		= 0.0f;

			currentState.OnExit();
			currentState = m_previousState;
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
	/// Get the Movement Controller from the Actor Reference. 
	/// </summary>
	/// <returns>Movement Controller Reference</returns>
	public MovementController GetMovement()
	{
		return m_actorReference.GetMovementController();
	}

	/// <summary>
	/// Return that attack list of the supplied ID.
	/// </summary>
	/// <param name="ID">Attack List Identifier.</param>
	/// <returns></returns>
	public BasicAttacks GetAttackList()
    {
		return m_attackList;
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
	/// Get the Input Handler from the Actor Reference. 
	/// </summary>
	/// <returns>Input Handler Reference</returns>
	public InputHandler GetInput()
	{
		return m_actorReference.GetInputHandler();
	}

	/// <summary>
	/// Return the state instance held within the internal array of states. 
	/// </summary>
	/// <param name="_state">Index of the state, represented as an enumerator</param>
	/// <returns></returns>
	public IState GetState( PlayerFSM.States _state )
	{
		return m_stateList[(int)_state];
	}

	/// <summary>
	/// Getter/Setter for the current attack.
	/// </summary>
	public AbstractAttack CurrentAttack
	{
		get { return m_currentAttack; }
		set { m_currentAttack = value; }
	}

	/// <summary>
	/// Block the state switch for X seconds.
	/// </summary>
	/// <param name="_time">Time to block the state switch for.</param>
	/// <returns></returns>
	public void BlockStateSwitch( float _time )
	{
		if ( !m_block )
		{
			m_blockTimer = _time;
			m_block = true;
		}
		else
		{
			m_blockTimer -= Time.deltaTime;
			if ( m_blockTimer <= 0.0f )
			{
				m_block = false;
			}
		}
	}

	public void InterruptBlocking()
	{
		m_blockTimer = 0.0f;
		m_block = false;
	}

	/// <summary>
	/// Add more time to the current state blocking. 
	/// </summary>
	/// <param name="_time">Time to be added.</param>
	public void AddBlockingTime( float _time )
	{
		m_blockTimer += _time;
	}

}
