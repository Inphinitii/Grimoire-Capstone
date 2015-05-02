using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeOption : IncDecMenu
{

    public Text displayText;
    public float[] Times = {20.0f, 60.0f, 120.0f, 180.0f,  240.0f}; //One Min, One.5 Mins, 2 Mins, 4 Mins

    private float m_time;
    private int index;

    // Use this for initialization
    void Start()
    {
        index = 0;
        m_time = Times[index];
        GameManager.gameModifiers.GameTime = m_time;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if ( m_time == 20.0f )
        {
            displayText.text = "Twenty Seconds";
        }
        if ( m_time == 60.0f )
        {
            displayText.text = "One Minute";

        }
        if ( m_time == 120.0f )
        {
            displayText.text = "Two Minutes";

        }
        if ( m_time == 180.0f )
        {
            displayText.text = "Three Minutes";

        }
        if ( m_time == 240.0f )
        {
            displayText.text = "Four Minutes";
        }
    }

    public override void Decrement()
    {
        index--;
        if ( index < 0 )
            index = Times.Length -1;
        m_time = Times[index];
        GameManager.gameModifiers.GameTime = m_time;

    }

    public override void Increment()
    {
        index++;
        if ( index > Times.Length )
            index = 0;
        m_time = Times[index];
        GameManager.gameModifiers.GameTime = m_time;
    }

    public int round( float _number )
    {
        return ( (int)( _number / 10 ) ) * 10;
    }
}
