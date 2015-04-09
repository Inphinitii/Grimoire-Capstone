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

//HORRIBLY MESSY PLS DONT LOOK 
[RequireComponent(typeof(MovementController))]
public class SpellCharges : MonoBehaviour
{
	public Charge chargePrefab;
	public int maxSpellCharges;
	public float chargeTime;
	public float chargeRadius;
	public float internalTimer;

	private Charge[] m_Charges;

	void Start()
	{
		m_Charges = new Charge[maxSpellCharges];
		DisplayCharges();
		ResetCharges();
	}


	void Update()
	{
		internalTimer += Time.deltaTime;
		for ( int i = 0; i < maxSpellCharges; i++ )
		{
			if ( m_Charges[i].active == true )
			{
				m_Charges[i].Follow( ChargePosition( i ) );
			}
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
	public void ResetCharge( int _index )
	{
		m_Charges[_index].Default();
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

	Vector3 ChargePosition( int _index )
	{
		Vector3 position = this.transform.position;
		float pi2 = ( 2 * 3.14f );
		_index += 1;
		position.y += 1.0f; //Offset upwards
		float angle = pi2 / maxSpellCharges * _index;

		position.x += chargeRadius * Mathf.Sin(  angle + internalTimer );
		position.y += Mathf.Sin( internalTimer * 4.0f ) * 0.15f;
		position.z += chargeRadius * Mathf.Cos( angle + internalTimer );

		return position;
	}
}
