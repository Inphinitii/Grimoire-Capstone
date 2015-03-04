using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Who cares about modular components. This is patchwork.

public class PlayerFSM : MonoBehaviour {

    bool m_block;
    public IState m_currentState;
    List<IState> m_stateList;

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

    private InputHandler p_inputHandler;
    private MovementController p_movementController;
    private AnimationController p_animationController;
    private PhysicsController p_physicsController;
    
	// Use this for initialization
	void Start () {
        p_inputHandler  = GetComponent<InputHandler>();
        p_movementController = GetComponent<MovementController>();
        p_animationController = GetComponent<AnimationController>();
        p_physicsController = GetComponent<PhysicsController>();
        
        

        m_stateList = new List<IState>();
        
        //Add States in order of the enum
        m_stateList.Add(new StandingState());
        m_stateList.Add(new MovementState());
        m_stateList.Add(new CrouchingState());
        m_stateList.Add(new JumpingState());

        foreach (IState item in m_stateList){
            item.SetFSM(this); //Set all of their FSM components to this.
        }

        m_currentState = m_stateList[0]; //Default State
	}
	
	// Update is called once per frame
	void FixedUpdate () {
                m_currentState.ExecuteState();
                Debug.Log(m_currentState);
	}

    public InputHandler GetInput() { return p_inputHandler; }
    public MovementController GetMovementController() { return p_movementController; }
    public AnimationController GetAnimationController() { return p_animationController; }
    public PhysicsController GetPhysicsController() { return p_physicsController; }

    public bool Blocking { get { return m_block; } set { m_block = value; } }
    public void StartChildCoroutine(IEnumerator _coroutine)
    {
        StartCoroutine(_coroutine);
    }
    public void SetCurrentState(States _states){
        if (!m_block)
        {
            m_currentState = m_stateList[(int)_states];
            m_currentState.OnSwitch();
        }
    }
}
