using UnityEngine;
using System.Collections;

/*========================================================
 * Author: Tyler Remazki
 *
 * Class : Spell Charges
 *
 * Description: Data structure that keeps track of the number
 * of available spell charges that the player has. 
 * References in the Grimoire class to determine when the player
 * can use a spell.
 * 
 * Internally handles recharging and keeping track of available
 * charges.
 =========================================================*/

public class SpellCharges : MonoBehaviour
{

	class Charge
	{
		public float	resetTime;
		public bool		active;

		private float	m_internalTime;

		public void Default()
		{
			resetTime = 0.0f;
			active = true;
		}
		public void SetRechargeTime(float _rechargeTime)
		{
			resetTime = _rechargeTime;
		}
		public void ConsumeCharge()
		{
			m_internalTime = resetTime;
			active = false;
		}
		public void Update(float _time)
		{
			if ( !active )
			{
				if ( m_internalTime <= 0.0f )
					active = true;
				else
					m_internalTime -= _time;
			}
		}
	}

	public int		maxSpellCharges;
	public float	chargeTime;

	private Charge[] m_Charges;

	void Start()
	{
		m_Charges = new Charge[maxSpellCharges];
		ResetCharges();
	}
	

	void Update()
	{
		for ( int i = 0; i < maxSpellCharges; i++ )
		{
			m_Charges[i].Update( Time.deltaTime );
		}
	}

	/// <summary>
	/// Check through the array of charges for an available charge to be consumed.
	/// </summary>
	/// <returns>
	/// True if an available charge is found.
	///	False if no available charge is found.
	///	</returns>
	public bool UseCharge()
	{
		for(int i = 0; i < maxSpellCharges; i++)
		{
			if ( m_Charges[i].active == true )
			{
				m_Charges[i].ConsumeCharge();
				return true;
			} 
		}
		return false;
	}

	/// <summary>
	/// Reset all of the charges in the array of charges to their default values.
	/// </summary>
	public void ResetCharges()
	{
		for ( int i = 0; i < maxSpellCharges; i++ )
		{
			m_Charges[i].Default();
		}
	}

	/// <summary>
	/// Reset the specific charge to default values.
	/// </summary>
	/// <param name="_index">Index of the charge in our array of charges.</param>
	public void ResetCharge(int _index)
	{
		m_Charges[_index].Default();
	}

	/// <summary>
	/// Visual representation of the charges. Might want to separate this into another class,
	/// but it doesn't really matter. 
	/// </summary>
	public void DisplayCharges()
	{
		//Display the charges in a circle around a transform.
		// locationOfCharge = anchor.x + Mathf.sin(3.14/maxCharges * i) + radius;
		// locationOfCharge = anchor.y + Mathf.cos(3.14/maxCharges * i) + radius;
		
	}
}
