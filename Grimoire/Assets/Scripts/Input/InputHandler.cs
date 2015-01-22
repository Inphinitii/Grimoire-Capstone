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
	
	//Controller Information
	private byte 	m_playerNumber = 1;
    private bool    m_freezeMovement, m_freezeKeypress, m_active, m_dashHeld, m_dashPressed;

    private Button JumpButton;
	
	#region Controller Inputs	
	private bool		m_XButton,
						    m_YButton,
						    m_BButton,
						    m_AButton,

                            m_leftTrigger,
                            m_rightTrigger,
                            m_leftButton,
                            m_rightButton;

	private Vector2 	m_leftStick;
						   
	#endregion

	void Start () 
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

	public bool 	X() 	{ return m_XButton; 	}
	public bool 	A() 	{ return m_AButton; 	}
	public bool 	Y() 	{ return m_YButton; 	}
	public bool 	B() 	{ return m_BButton; 	}
    public bool LT()    { return m_leftTrigger; }
    public bool RT()    { return m_rightTrigger; }
    public bool LB()   { return m_leftButton; }
    public bool RB()   { return m_rightButton; }

	public Vector2 	LeftStick() { return m_leftStick; 	 	}

	public bool FreezeKeypress { get { return m_freezeKeypress; } set { m_freezeKeypress = value; }}
	public bool FreezeMovement { get { return m_freezeMovement; } set { m_freezeMovement = value; }}
	public bool FreezeAll 	   { get { return m_active;         } set { m_active = value;         }}
    public Button Jump() { return JumpButton; }


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
            JumpButton.Held    = Input.GetKey(KeyCode.X);
            JumpButton.Down  = Input.GetKeyDown(KeyCode.X);
            JumpButton.Up      = Input.GetKeyUp(KeyCode.X);




			m_AButton		= Input.GetKeyDown(KeyCode.A);
			m_BButton 		= Input.GetKeyDown(KeyCode.S);
			m_YButton 	    = Input.GetKeyDown(KeyCode.Z);
		}
		else
		{
            m_AButton   = false;
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
			m_leftStick = GamePad.GetAxis(GamePad.Axis.LeftStick, (GamePad.Index)m_playerNumber);
		}
		else
			m_leftStick = Vector2.zero;

		//------------GAMEPAD FACE BUTTON INPUT ------------//
		if(!m_freezeKeypress)
		{
			m_AButton 		= GamePad.GetButton(GamePad.Button.A, (GamePad.Index)m_playerNumber);
			m_YButton	 	= GamePad.GetButton(GamePad.Button.Y, (GamePad.Index)m_playerNumber);
			m_BButton 		= GamePad.GetButton(GamePad.Button.B, (GamePad.Index)m_playerNumber);
			m_XButton 	    = GamePad.GetButton(GamePad.Button.X, (GamePad.Index)m_playerNumber);
            m_rightButton = GamePad.GetButton(GamePad.Button.RightShoulder, (GamePad.Index)m_playerNumber);
            m_leftButton   = GamePad.GetButton(GamePad.Button.LeftShoulder, (GamePad.Index)m_playerNumber);
		}
		else
		{
            m_AButton = false;
            m_YButton = false;
            m_BButton = false;
            m_XButton = false;
		}
	}

	
}
