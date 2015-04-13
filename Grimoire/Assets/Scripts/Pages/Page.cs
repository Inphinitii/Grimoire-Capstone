using System.Collections;
using UnityEngine;

/*========================================================
 * Author: Tyler Remazki
 *
 * Class : Page
 *
 * Description Similar to BasicAttack.cs but houses the four available attakcs that a Page can have.
 * Used inside of the Grimoire.cs MonoBehaviour. 
 =========================================================*/

public class Page : MonoBehaviour
{
	public enum Type
	{
		STANDING_NEUTRAL,
		STANDING_DIRECTIONAL,
		AIR_NEUTRAL,
		AIR_DIRECTIONAL
	};

	public bool useActorForce;
	public GameObject parent;
	public Properties.ForceType forceType;
	public float ChargeRefresh;

	public AbstractAttack standingNeutral;
	public AbstractAttack standingDirectional;
	public AbstractAttack airNeutral;
	public AbstractAttack airDirectional;

	private AbstractAttack[] m_attacks;
	private AbstractAttack _tempAtk;

	void Start()
	{
		m_attacks = new AbstractAttack[4];
		m_attacks[0] = standingNeutral;
		m_attacks[1] = standingDirectional;
		m_attacks[2] = airNeutral;
		m_attacks[3] = airDirectional;

		if(useActorForce)
			forceType = parent.GetComponent<Actor>().forceType;

		for ( int i = 0; i < m_attacks.Length; i++ )
		{
			_tempAtk									= (AbstractAttack)Instantiate( m_attacks[i], this.transform.position, Quaternion.identity ) as AbstractAttack;
			_tempAtk.transform.parent		= parent.transform;
			_tempAtk.forceType					= forceType;
			m_attacks[i]							= _tempAtk;
		}
	}

	public virtual AbstractAttack UsePage( Type _type )
	{
		return m_attacks[(int)_type];
	}

    public virtual void OnPageUse()
    {

    }

    public virtual void OnPageRelease()
    {

    }
}
