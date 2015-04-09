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

[RequireComponent(typeof(MovementController))]
public class SpellCharges : MonoBehaviour
{
	public int			maxSpellCharges;
	public bool		freezeTime;
	public float		chargeTime;
	public float		chargeRadius;
	public Charge	chargePrefab;

	private Charge[]	m_Charges;
	private float			m_internalTimer;
	private bool			m_chargesRemaining;

	void Start()
	{
		m_Charges = new Charge[maxSpellCharges];
		DisplayCharges();
		ResetCharges();
	}


	void Update()
	{
		if ( !freezeTime )
			m_internalTimer += Time.deltaTime;

		for ( int i = 0; i < maxSpellCharges; i++ )
		{
			if ( m_Charges[i].isActive == true )
			{
				m_Charges[i].Position =  ChargePosition( i );
			}
			//Keep the trails reasonable based on the objects velocity. 
			if ( Mathf.Abs(GetComponent<PhysicsController>().Velocity.magnitude) >= 2.0f )
				m_Charges[i].GetTrail().time = 0.1f;
			else
				m_Charges[i].GetTrail().time = 1.0f;
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
		for ( int i = 0; i < maxSpellCharges; i++ )
		{
			if ( m_Charges[i].isActive == true )
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
			m_Charges[i].SetRechargeTime( chargeTime );
		}
	}

	/// <summary>
	/// Reset the specific charge to default values.
	/// </summary>
	/// <param name="_index">Index of the charge in our array of charges.</param>
	public void ResetCharge( int _index )
	{
		m_Charges[_index].Refresh();
	}

	/// <summary>
	/// Visual representation of the charges. Might want to separate this into another class,
	/// but it doesn't really matter. 
	/// </summary>
	public void DisplayCharges()
	{
		for ( int i = 0; i < maxSpellCharges; i++ )
		{
			m_Charges[i] = (Charge)Instantiate( chargePrefab, this.transform.position, Quaternion.identity ) as Charge;
		}
	}

	/// <summary>
	/// Place the charges in a circular fashion around the transformation.
	/// </summary>
	/// <param name="_index">Index of the charge in the array.</param>
	/// <returns>A modified position to place the charge at.</returns>
	Vector3 ChargePosition( int _index )
	{
		_index++;
		Vector3 position = this.transform.position;
		float pi2 = ( 2 * 3.14f );
		position.y += 1.0f; //Offset upwards
		float angle = pi2 / maxSpellCharges * _index;

		position.x += chargeRadius * Mathf.Sin( angle + m_internalTimer );
		position.y += Mathf.Sin( m_internalTimer * 4.0f ) * 0.15f;
		position.z += chargeRadius * Mathf.Cos( angle + m_internalTimer );
		return position;
	}
}
