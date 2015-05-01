using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SFXOption : IncDecMenu {

    public Text displayText;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public override void Update () {
	    base.Update();
        int display = (int)( GameManager.sfxVolume * 100 );
        displayText.text = display.ToString();
	}

    public override void Decrement()
    {
        if(GameManager.sfxVolume >= 0.1f)
        GameManager.sfxVolume -= 0.1f;
    }

    public override void Increment()
    {
        if ( GameManager.sfxVolume < 1.0f)
        GameManager.sfxVolume += 0.1f;
    }
}
