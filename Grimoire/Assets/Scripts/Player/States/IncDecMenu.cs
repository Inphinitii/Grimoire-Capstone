using UnityEngine;
using GamepadInput;
using System.Collections;

public class IncDecMenu : MonoBehaviour
{

    public bool vertical;
    public bool active;

    protected float m_currentTime = 0.0f;
    protected const float SCROLL_DELAY = 0.15f;

    void Start()
    {

    }

    public void SetActive( bool _active )
    {
        active = _active;
    }

    public virtual void Update()
    {
        if ( active )
        {
            Vector2 input = GamePad.GetAxis( GamepadInput.GamePad.Axis.LeftStick, GamePad.Index.Any, true );
            if ( m_currentTime <= 0.0f )
            {
                if ( input != Vector2.zero )
                {
                    Scroll( input, vertical );
                    m_currentTime = SCROLL_DELAY;
                }
            }
            else if ( m_currentTime > 0.0f )
                m_currentTime -= Time.deltaTime; 
        }
    }

    public virtual void Scroll( Vector3 _stick, bool _vertical )
    {

        float direction;
        direction = !_vertical ? -_stick.x : _stick.y;

        if ( direction < 0.0f )
            Increment();
        else if ( direction > 0.0f )
            Decrement();
    }

    public virtual void Increment()
    {

    }

    public virtual void Decrement()
    {

    }
}
