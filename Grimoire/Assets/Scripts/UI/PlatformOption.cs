using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlatformOption : IncDecMenu
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
        displayText.text = GameManager.gameModifiers.Platforms ? "Enabled" : "Disabled";
    }

    public override void Decrement()
    {
        GameManager.gameModifiers.Platforms = !GameManager.gameModifiers.Platforms;
    }

    public override void Increment()
    {
        Decrement();
    }

    public int round( float _number )
    {
        return ( (int)( _number / 10 ) ) * 10;
    }
}
