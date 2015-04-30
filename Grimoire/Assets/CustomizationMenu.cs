using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CustomizationMenu : AbstractMenu
{
	public bool lockedIn;
	public GameObject  lockedText;
	public override void Start()
	{
		lockedIn = false;
		base.Start();
	}

	public override void Update()
	{
		if ( !lockedIn )
		{
			base.Update();
			if ( GamepadInput.GamePad.GetButtonDown( GamepadInput.GamePad.Button.Start, (GamepadInput.GamePad.Index)controllerNumber ) )
			{
				lockedIn = true;
				lockedText.gameObject.SetActive( true );
			}
		}
		else
		{
			if ( GamepadInput.GamePad.GetButtonDown( GamepadInput.GamePad.Button.B, (GamepadInput.GamePad.Index)controllerNumber ) )
			{
				lockedIn = false;
				lockedText.gameObject.SetActive( false );
			}
		}

	}


}
