using UnityEngine;
using System.Collections;

public class Charge : MonoBehaviour {

	public bool isActive;
	public float resetTime = 3.0f;
	public ParticleSystem onChargeUse;

	private float					m_internalTime;
	private TrailRenderer	m_trailRenderer;
	private Renderer			m_renderer;

	void Start()
	{
		m_trailRenderer		= GetComponent<TrailRenderer>();
		m_renderer				= GetComponentInChildren<MeshRenderer>();
		isActive = true;
		SetRenderers( true );
	}

	void Update()
	{
		if ( !isActive )
		{
			if ( m_internalTime <= 0.0f )
			{
				isActive = true;
				SetRenderers( true );
			}
			else
				m_internalTime -= Time.deltaTime;
		}
	}

	/// <summary>
	/// Refresh the charge instantly.
	/// </summary>
	public void Refresh()
	{
		resetTime	= 0.0f;
		isActive		= true;
	}

	/// <summary>
	/// Set the recharge time manually.
	/// </summary>
	/// <param name="_rechargeTime">Recharge time.</param>
	public void SetRechargeTime( float _rechargeTime )
	{
		resetTime = _rechargeTime;
	}

	/// <summary>
	/// Use a charge, turning off this current charge and having it refresh over time. 
	/// </summary>
	public void ConsumeCharge()
	{
		Instantiate(onChargeUse,this.transform.position,Quaternion.identity);
		m_internalTime			= resetTime;
		isActive						= false;
		SetRenderers( false );
	}

	/// <summary>
	/// Disable/Enable the renderers of the current charge based on the boolean passed.
	/// </summary>
	/// <param name="_bool">Enable/Disable</param>
	float time;
	public void SetRenderers(bool _bool)
	{
		if (!_bool ) 
			GetComponent<Animator>().SetBool( "Despawn", true );
		else
			GetComponent<Animator>().SetBool( "Despawn", false );


		m_renderer.enabled		= _bool;
		m_trailRenderer.enabled = _bool;

	}

	public void StartRotationAnimation()
	{
		//GetComponent<Animator>().SetBool( "Despawn", false);
	}

	/// <summary>
	/// Returns the Trail component attached to the Charge object. 
	/// </summary>
	/// <returns></returns>
	public TrailRenderer GetTrail() { return m_trailRenderer; }
	

	/// <summary>
	/// Getter/Setter for the position of the charge. Used within SpellCharges.cs to space the charges around the player in a circular formation.
	/// </summary>
	public Vector3 Position{ get { return this.transform.position; } set { this.transform.position = value; } }

	/// <summary>
	/// Returns the internal timer of the object.
	/// </summary>
	public float InternalTime{ get { return m_internalTime; }	set { m_internalTime = value; } }
}
