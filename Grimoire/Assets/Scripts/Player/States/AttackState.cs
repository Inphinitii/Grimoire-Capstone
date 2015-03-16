using UnityEngine;
using System.Collections;

public class AttackState : IState {

	private Attack m_attack;
	private bool m_interruptable;

	public AttackState()
	{
	}
	
	public override void ExecuteState()
	{
		GetFSM().StartCoroutine( Attack( m_attack ) );
		if (m_interruptable) 
		{
			m_attack.HandleInput(m_playerFSM.GetActorReference().GetInputHandler());
		}

	}

	public void SetAttack(Attack _attack, bool _interruptable){
		m_attack = _attack;
		m_interruptable = _interruptable;
	}
}