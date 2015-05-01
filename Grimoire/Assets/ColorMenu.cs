using UnityEngine;
using UnityEngine.UI;
using GamepadInput;
using System.Collections;

public class ColorMenu : ScrollMenuObject
{
    Color[] _colors;
    private Image m_parentImage;
    public Color m_currentSelection;


    // Use this for initialization
    void Start()
    {
        active = false;
        vertical = false;
        _colors = new Color[5];
        _colors[0] = Color.red;
        _colors[1] = Color.cyan;
        _colors[2] = Color.green;
        _colors[3] = Color.yellow;
        _colors[4] = Color.white;

        m_currentSelection = _colors[4];
        m_parentImage = GetComponent<Image>();
    }

    // Update is called once per frame
    public override void Update()
    {
        if ( active )
        {
            if ( _colors.Length > 0 )
            {
                Vector2 input = GamePad.GetAxis( GamepadInput.GamePad.Axis.LeftStick, (GamePad.Index)parent.controllerNumber, true );
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
    }

    public override void UpdateSelection()
    {
        m_parentImage.color = m_currentSelection;
        GameManager.GetAllActors()[parent.controllerNumber - 1].actorColor = m_currentSelection;
    }

    public override void Scroll( Vector3 _stick, bool _vertical )
    {

        float direction;
        direction = !_vertical ? -_stick.x : _stick.y;

        if ( direction < 0.0f )
            m_selectionIndex++;
        else if ( direction > 0.0f )
            m_selectionIndex--;

        if ( m_selectionIndex >= _colors.Length )
            m_selectionIndex = 0;
        else if ( m_selectionIndex < 0 )
            m_selectionIndex = _colors.Length - 1;

        m_currentSelection = _colors[m_selectionIndex];
        UpdateSelection();
    }
}
