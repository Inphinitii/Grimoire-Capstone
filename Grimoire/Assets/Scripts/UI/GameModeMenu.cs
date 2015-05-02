using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameModeMenu : IncDecMenu
{

    public GameManager.GameModeEnum gameMode;
    public Text descriptionText;
    public Text displayText;
    public bool updateText;

    private int index;

    // Use this for initialization
    void Start()
    {
        updateText = true;
        gameMode = GameManager.GameModeEnum.ComboKing;
        index = 0;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        displayText.text = gameMode.ToString();

        if ( updateText )
        {
            descriptionText.text = GameManager.ReturnGameModeDescription( GameManager.GameModeEnum.ComboKing );
            updateText = false;
        }
    }

    public override void Decrement()
    {
        index--;
        updateText = true;
    }

    public override void Increment()
    {
        index++;
        updateText = true;
    }

    public int round( float _number )
    {
        return ( (int)( _number / 10 ) ) * 10;
    }
}
