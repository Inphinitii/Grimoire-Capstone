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
	private bool		m_attackButton,
						m_defendButton,
						m_utilityButton,
						m_movementButton;
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

	public bool 	Attack() 	{ return m_attackButton; 	}
	public bool 	Utility() 	{ return m_utilityButton; 	}
	public bool 	Defend() 	{ return m_defendButton; 	}
	public bool 	Movement() 	{ return m_movementButton; 	}
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
            JumpButton.Held = Input.GetKey(KeyCode.X);
            JumpButton.Down = Input.GetKeyDown(KeyCode.X);
            JumpButton.Up = Input.GetKeyUp(KeyCode.X);




			m_utilityButton		= Input.GetKeyDown(KeyCode.A);
			m_defendButton 		= Input.GetKeyDown(KeyCode.S);
			m_movementButton 	= Input.GetKeyDown(KeyCode.Z);
		}
		else
		{
			m_attackButton 		= false;
			m_utilityButton 	= false;
			m_defendButton 		= false;
			m_movementButton 	= false;
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
			m_attackButton 		= GamePad.GetButton(GamePad.Button.A, (GamePad.Index)m_playerNumber);
			m_utilityButton	 	= GamePad.GetButton(GamePad.Button.Y, (GamePad.Index)m_playerNumber);
			m_defendButton 		= GamePad.GetButton(GamePad.Button.B, (GamePad.Index)m_playerNumber);
			m_movementButton 	= GamePad.GetButton(GamePad.Button.X, (GamePad.Index)m_playerNumber);
		}
		else
		{
			m_attackButton 		= false;
			m_utilityButton 	= false;
			m_defendButton 		= false;
			m_movementButton 	= false;
		}
	}

	
}
