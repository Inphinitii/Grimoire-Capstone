#define KEYBOARD_DEBUG 

using UnityEngine;
using System.Collections;
using GamepadInput;

/*==================================================================================
 * Author: Tyler Remazki
 *
 * Class : Input Handler
 *
 * Description: Encapsulates the use of controllers and bridges the external Asset
 * Gamepad Input 
 =================================================================================*/

public class InputHandler : MonoBehaviour {

    public struct Button
    {
        public string ButtonName;
        public bool Down,
                    Up,
                    Held;
    }
	static byte s_playerNumber;
	
    public bool AssignNumber;
    public byte UID;

	//Controller Information
	private byte 	m_playerNumber = 1;
    private bool    m_freezeMovement, m_freezeKeypress, m_active, m_dashHeld, m_dashPressed;

    private Button JumpButton;
	
	#region Controller Inputs	
	private bool		m_XButton,
						m_YButton,
						m_BButton,
						m_AButton,
                        m_leftShoulder,
                        m_rightShoulder;

	private Vector2 	m_leftStick,
                        m_rightStick;

    private float       m_leftTrigger,
                        m_rightTrigger;


						   
	#endregion

	void Start () 
	{
        if(!AssignNumber)
        {
	        if(s_playerNumber <= 4)
	        {
		        m_playerNumber 	= s_playerNumber;
		        m_active 		= true;
		        s_playerNumber += 1; 
			
		        m_freezeMovement 	= false;
		        m_freezeKeypress	= false;
	        }
        }
        else
        {
            if(UID <= 4)
	        {
		        m_playerNumber 	= UID;
		        m_active 		= true;
			
		        m_freezeMovement 	= false;
		        m_freezeKeypress	= false;
	        }
        }
	}
	
	void Update () 
	{
		if(m_active)
		{
			#if (KEYBOARD_DEBUG)
			ProcessKeyboardInput();
			#endif 

			#if (!KEYBOARD_DEBUG)
			ProcessGamepadInput();
			#endif
		}
	}

	public bool X() 	{ return m_XButton; 	}
	public bool A() 	{ return m_AButton; 	}
	public bool Y() 	{ return m_YButton; 	}
	public bool B() 	{ return m_BButton; 	}
    public float LT()    { return m_leftTrigger; }
    public float RT()    { return m_rightTrigger; }
    public bool LB()    { return m_leftShoulder; }
    public bool RB()    { return m_rightShoulder; }

	public Vector2 	LeftStick() { return m_leftStick; 	 	}

	public bool FreezeKeypress { get { return m_freezeKeypress; } set { m_freezeKeypress = value; }}
	public bool FreezeMovement { get { return m_freezeMovement; } set { m_freezeMovement = value; }}
	public bool FreezeAll 	   { get { return m_active;         } set { m_active = value;         }}
    public Button Jump()       { return JumpButton; }


	private void ProcessKeyboardInput()
	{
		//------------KEYBOARD MOVEMENT DEBUG ------------//
		if(!m_freezeMovement)
		{
			if(!Input.anyKey)
				m_leftStick = Vector2.zero;
			if(Input.GetKey(KeyCode.LeftArrow))
				m_leftStick.x = -1;
			if(Input.GetKey(KeyCode.RightArrow))
				m_leftStick.x = 1;
			if(Input.GetKey(KeyCode.UpArrow))
				m_leftStick.y = 1;
			if(Input.GetKey(KeyCode.DownArrow))
				m_leftStick.y = -1;			
		}
		else
			m_leftStick = Vector2.zero;
		
		//-----------KEYBOARD ACTION KEYS ---------------//
		if(!m_freezeKeypress)
		{
            //Jumping
            m_XButton       = Input.GetKeyDown(KeyCode.Z);
			m_AButton		= Input.GetKeyDown(KeyCode.X);
			m_BButton 		= Input.GetKeyDown(KeyCode.S);
			m_YButton 	    = Input.GetKeyDown(KeyCode.A);
		}
		else
		{
            m_AButton    = false;
			m_BButton 	 = false;
			m_YButton    = false;
			m_XButton 	 = false;
		}
	}

	private void ProcessGamepadInput()
	{
		//------------GAMEPAD DIRECTIONAL INPUT ------------//
		if(!m_freezeMovement)
		{
			m_leftStick     = GamePad.GetAxis(GamePad.Axis.LeftStick,  (GamePad.Index)m_playerNumber);
			m_rightStick    = GamePad.GetAxis(GamePad.Axis.RightStick, (GamePad.Index)m_playerNumber);
            Debug.Log(m_rightStick);

		}
		else
			m_leftStick     = Vector2.zero;
            m_rightStick    = Vector2.zero;

		//------------GAMEPAD FACE BUTTON INPUT ------------//
		if(!m_freezeKeypress)
		{
			m_AButton 		= GamePad.GetButton(GamePad.Button.A, (GamePad.Index)m_playerNumber);
			m_YButton	 	= GamePad.GetButton(GamePad.Button.Y, (GamePad.Index)m_playerNumber);
			m_BButton 		= GamePad.GetButton(GamePad.Button.B, (GamePad.Index)m_playerNumber);
			m_XButton 	    = GamePad.GetButton(GamePad.Button.X, (GamePad.Index)m_playerNumber);

            //Shoulder Buttons
            m_rightShoulder     = GamePad.GetButton(GamePad.Button.RightShoulder,   (GamePad.Index)m_playerNumber);
            m_leftShoulder      = GamePad.GetButton(GamePad.Button.LeftShoulder,    (GamePad.Index)m_playerNumber);

            //Triggers
            m_rightTrigger = GamePad.GetTrigger(GamePad.Trigger.RightTrigger, (GamePad.Index)m_playerNumber);
            m_leftTrigger = GamePad.GetTrigger(GamePad.Trigger.LeftTrigger, (GamePad.Index)m_playerNumber);

            
		}
		else
		{
            m_AButton = false;
            m_YButton = false;
            m_BButton = false;
            m_XButton = false;

            m_rightShoulder = false;
            m_leftShoulder  = false;

            m_rightTrigger  = 0.0f;
            m_leftTrigger = 0.0f;
		}

	}  
}
