//#define KEYBOARD_DEBUG 

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

public class InputHandler : MonoBehaviour
{

	public struct InputButton
	{
		public bool thisFrame,
					lastFrame;

		public void Clear()
		{
			thisFrame = false;
			lastFrame = false;
		}
	}
	public struct InputTrigger
	{
		public float thisFrame,
					 lastFrame;

		public void Clear()
		{
			thisFrame = 0.0f;
			lastFrame = 0.0f;
		}
	}

	public struct InputVector
	{
		public Vector2 Direction;
	}


	public static int s_playerNumber = 1;

	public bool AssignNumber;
	public byte UID;
	public float comboInputWindow;

	//Controller Information
	public int		m_playerNumber = 1;
	private bool	m_freezeMovement,
					m_freezeKeypress,
					m_active,
					m_combinationCheck;

	#region Controller Inputs
	private InputButton m_XButton,
						m_YButton,
						m_BButton,
						m_AButton,
						m_leftShoulder,
						m_rightShoulder;


	private Vector2 m_leftStick,
					m_rightStick;

	private InputTrigger	m_leftTrigger,
							m_rightTrigger;

	#endregion

	void Start()
	{
		if ( !AssignNumber )
		{
			if ( s_playerNumber <= 4 )
			{
				m_playerNumber = s_playerNumber;
				m_active = true;
				s_playerNumber++;

				m_freezeMovement = false;
				m_freezeKeypress = false;
			}
		}
		else
		{
			if ( UID <= 4 )
			{
				m_playerNumber = UID;
				m_active = true;

				m_freezeMovement = false;
				m_freezeKeypress = false;
			}
		}
	}

	void Update()
	{
		if ( m_active )
		{
			#if (KEYBOARD_DEBUG)
								ProcessKeyboardInput();
			#endif

			#if (!KEYBOARD_DEBUG)
						ProcessGamepadInput();
			#endif
		}
	}

	public InputButton Attack() { return m_XButton; }
	public InputButton Jump() { return m_AButton; }
	public InputButton SpellSwap() { return m_YButton; }
	public InputButton Special() { return m_BButton; }
	public InputButton LB() { return m_leftShoulder; }
	public InputButton RB() { return m_rightShoulder; }
	public InputTrigger LT() { return m_leftTrigger; }
	public InputTrigger RT() { return m_rightTrigger; }
	public InputTrigger Triggers()
	{
		m_rightTrigger.lastFrame += m_leftTrigger.lastFrame;
		m_rightTrigger.thisFrame += m_leftTrigger.thisFrame;
		return m_rightTrigger;
	}

	public Vector2 LeftStick() { return m_leftStick; }
	public Vector2 RightStick() { return m_rightStick; }


	public bool FreezeKeypress { get { return m_freezeKeypress; } set { m_freezeKeypress = value; } }
	public bool FreezeMovement { get { return m_freezeMovement; } set { m_freezeMovement = value; } }
	public bool ComboCheck { get { return m_combinationCheck; } set { m_combinationCheck = value; } }
	public bool FreezeAll { get { return m_active; } set { m_active = value; } }


	public GamepadState GetCurrentGamepadState() { return GamePad.GetState( (GamePad.Index)m_playerNumber, true ); }

	/// <summary>
	/// Process input using the keyboard. To use this, uncomment the define statement located at the top of this file. 
	/// </summary>
	private void ProcessKeyboardInput()
	{
		//------------KEYBOARD MOVEMENT DEBUG ------------//
		if ( !m_freezeMovement )
		{
			if ( Input.GetKey( KeyCode.LeftArrow ) )
				m_leftStick.x = -1;
			if ( Input.GetKey( KeyCode.RightArrow ) )
				m_leftStick.x = 1;
			if ( Input.GetKey( KeyCode.UpArrow ) )
				m_leftStick.y = 1;
			if ( Input.GetKey( KeyCode.DownArrow ) )
				m_leftStick.y = -1;
			if ( !Input.anyKey )
				m_leftStick = Vector2.zero;
		}
		else
			m_leftStick = Vector2.zero;

		//-----------KEYBOARD ACTION KEYS ---------------//
		//if(!m_freezeKeypress)
		//{
		//	//Jumping
		//	m_XButton       = Input.GetKey(KeyCode.Z);
		//	m_AButton		= Input.GetKey(KeyCode.X);
		//	m_BButton 		= Input.GetKey(KeyCode.S);
		//	m_YButton 	    = Input.GetKey(KeyCode.A);
		//}
		//else
		//{
		//	m_AButton    = false;
		//	m_BButton 	 = false;
		//	m_YButton    = false;
		//	m_XButton 	 = false;
		//}
	}

	/// <summary>
	/// Process input using the Xbox360 Gamepad. To use this, comment out the define statement located at the top of this file. 
	/// </summary>
	private void ProcessGamepadInput()
	{
		//------------GAMEPAD DIRECTIONAL INPUT ------------//
		if ( !m_freezeMovement )
		{
			m_leftStick		= GamePad.GetAxis( GamePad.Axis.LeftStick, (GamePad.Index)m_playerNumber, true );
			m_rightStick = GamePad.GetAxis( GamePad.Axis.RightStick, (GamePad.Index)m_playerNumber, true );
		}
		else
		{
			m_leftStick = Vector2.zero;
			m_rightStick = Vector2.zero;
		}

		//------------GAMEPAD FACE BUTTON INPUT ------------//
		if ( !m_freezeKeypress )
		{

			//m_rightShoulder		= GamePad.GetButton(GamePad.Button.RightShoulder,   (GamePad.Index)m_playerNumber);
			//m_leftShoulder		= GamePad.GetButton(GamePad.Button.LeftShoulder,    (GamePad.Index)m_playerNumber);

			GetGamepadButton( ref m_AButton, GamePad.Button.A, (GamePad.Index)m_playerNumber );
			GetGamepadButton( ref m_YButton, GamePad.Button.Y, (GamePad.Index)m_playerNumber );
			GetGamepadButton( ref m_BButton, GamePad.Button.B, (GamePad.Index)m_playerNumber );
			GetGamepadButton( ref m_XButton, GamePad.Button.X, (GamePad.Index)m_playerNumber );

			GetGamepadButton( ref m_rightShoulder, GamePad.Button.RightShoulder, (GamePad.Index)m_playerNumber );
			GetGamepadButton( ref m_leftShoulder, GamePad.Button.LeftShoulder, (GamePad.Index)m_playerNumber );

			GetGamepadTrigger( ref m_rightTrigger, GamePad.Trigger.RightTrigger, (GamePad.Index)m_playerNumber );
			GetGamepadTrigger( ref m_leftTrigger, GamePad.Trigger.LeftTrigger, (GamePad.Index)m_playerNumber );



		}
		else
		{
			m_AButton.Clear();
			m_YButton.Clear();
			m_BButton.Clear();
			m_XButton.Clear();

			m_rightShoulder.Clear();
			m_leftShoulder.Clear();

			m_rightTrigger.Clear();
			m_leftTrigger.Clear();
		}

	}

	void GetGamepadButton( ref InputButton _button, GamePad.Button _gamepadButton, GamePad.Index _index )
	{
		_button.lastFrame = _button.thisFrame;
		_button.thisFrame = GamePad.GetButton( _gamepadButton, _index );
	}

	void GetGamepadTrigger( ref InputTrigger _button, GamePad.Trigger _gamepadButton, GamePad.Index _index )
	{
		_button.lastFrame = _button.thisFrame;
		_button.thisFrame = GamePad.GetTrigger( _gamepadButton, _index, true );
	}
}
