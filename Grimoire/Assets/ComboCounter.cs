using UnityEngine;
using System.Collections;

public class ComboCounter : MonoBehaviour {

	private int m_highestCombo;
	private int m_currentCombo;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void HitEnemy(Collider2D _collider)
	{
		PlayerFSM _temp = _collider.gameObject.GetComponent<PlayerFSM>();

		if ( _temp.currentState == _temp.GetState( PlayerFSM.States.HIT ) )
			m_currentCombo++;
		else
			m_currentCombo = 1;

		m_highestCombo = m_currentCombo > m_highestCombo ? m_currentCombo : m_highestCombo;
	}

	public int HighestCombo { get { return m_highestCombo; } set { m_highestCombo = value; } }
	public int CurrentCombo { get { return m_currentCombo; } set { m_currentCombo  = value; } }

}
