using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GMComboKing : GameMode {

    public Actor dominantActor;
    private ComboCounter[] m_comboCounters;
    private int m_counter;

    private int lastCombo;

    public GMComboKing()
    {

    }

    public GMComboKing( Actor[] _actors )
    {
        _actorReference     = _actors;
        m_counter           = 0;
        m_comboCounters     = new ComboCounter[_actors.Length];
        dominantActor       = null;
        winnerActor         = null;
        _timer              = GameManager.gameModifiers.GameTime;

        for ( int i = 0; i < m_comboCounters.Length; i++ )
            m_comboCounters[i] = _actorReference[i].GetComponent<ComboCounter>();
    }

    public override void Initialize()
    {
        throw new System.NotImplementedException();
    }

    public override void SpawnUI()
    {
        base.SpawnUI();
    }
    public override void Update()
    {
        if ( _timer > 0.0f )
        {
            if ( m_counter == 0 )
            {
                m_uiReference.GetComponentInChildren<Text>().color = Color.black;
            }
            for ( int i = 0; i < m_comboCounters.Length; i++ )
            {
                if ( m_comboCounters[i].hitConfirmed )
                {
                    if ( m_counter != 0 )
                    {
                        if ( dominantActor == _actorReference[i] )
                        {
                            m_counter++;
                        }
                        else
                        {
                            m_counter--;
                        }
                    }
                    else
                    {
                        dominantActor = _actorReference[i];
                        m_counter++;
                    }
                    m_uiReference.GetComponentInChildren<Text>().text = m_counter.ToString();
                    m_uiReference.GetComponentInChildren<Text>().color = dominantActor.actorColor;
                }
            }
            _timer -= Time.deltaTime;
        }
        else
        {
            winnerActor = dominantActor;
        }
    }

    public override string GetDescription()
    {
        return "Battle for domination over the combo counter. By hitting the other player, tip the counter in your favour and come out on top.";
    }

    public override bool GameEnd()
    {
        return winnerActor != null;
    }

  
}
