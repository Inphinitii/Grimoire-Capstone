using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MusicOption : IncDecMenu
{

    public Text displayText;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        int display = round(GameManager.musicVolume * 100 );
        displayText.text = display.ToString();
    }

    public override void Decrement()
    {
        if ( GameManager.musicVolume >= 0.1f )
            GameManager.musicVolume -= 0.1f;
    }

    public override void Increment()
    {
        if ( GameManager.musicVolume < 1.0f )
            GameManager.musicVolume += 0.1f;
    }

    public int round( float _number )
    {
        return ( (int)( _number / 10 ) ) * 10;
    }
}
