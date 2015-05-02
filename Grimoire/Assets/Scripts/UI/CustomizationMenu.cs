using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CustomizationMenu : AbstractMenu
{
	public bool lockedIn;
    private bool justLocked;
	public GameObject  lockedText;
    public ColorMenu colorPicker;

    public PageScrollMenu spell1;
    public PageScrollMenu spell2;
    public PageScrollMenu spell3;



	public override void Start()
	{
		lockedIn = false;
		base.Start();
	}

	public override void Update()
	{
        if ( isActive )
        {
            if ( !lockedIn )
            {
                base.Update();
                if ( GamepadInput.GamePad.GetButtonDown( GamepadInput.GamePad.Button.Start, (GamepadInput.GamePad.Index)controllerNumber ) )
                {
                    lockedIn = true;
                    justLocked = true;
                    colorPicker.SetActive( true );

                }
            }
            else
            {
                if ( GamepadInput.GamePad.GetButtonDown( GamepadInput.GamePad.Button.B, (GamepadInput.GamePad.Index)controllerNumber ) )
                {
                    lockedIn = false;
                    lockedText.gameObject.SetActive( false );
                    colorPicker.SetActive( false );
                }

                if ( justLocked )
                {
                    lockedText.gameObject.SetActive( true );

                    Grimoire actorGrimoire = GameManager.GetAllActors()[controllerNumber - 1].GetComponent<Grimoire>();

                    actorGrimoire.Reset();
                    actorGrimoire.AllocateSpace( 3 );

                    actorGrimoire.AddPage( spell1.m_current );
                    actorGrimoire.AddPage( spell2.m_current );
                    actorGrimoire.AddPage( spell3.m_current );

                    justLocked = false;
                }
            }
        }

	}


}
