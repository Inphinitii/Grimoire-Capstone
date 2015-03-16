using UnityEngine;
using System.Collections;

/*
 *  IState is an interface that is extended by every available State that the player can be in.
 */
public abstract class IState
{
    protected PlayerFSM m_playerFSM;

    public void SetFSM(PlayerFSM _fsm){
        m_playerFSM = _fsm;
    }

    public IEnumerator BlockStateSwitch(float _time)
    {
        m_playerFSM.Blocking = true;
        yield return new WaitForSeconds(_time);
        m_playerFSM.Blocking = false;
    }

    public IEnumerator Attack(Attack _attack)
    {
		m_playerFSM.StartCoroutine (BlockStateSwitch (_attack.duration + _attack.cooldownTime));
		yield return new WaitForSeconds (_attack.startupTime);
        _attack.gameObject.SetActive( true );
        yield return new WaitForSeconds( _attack.duration );
        _attack.gameObject.SetActive( false );
    }

    public virtual void OnSwitch() { }
    public abstract void ExecuteState();


    protected PlayerFSM GetFSM() { return m_playerFSM; }
}
