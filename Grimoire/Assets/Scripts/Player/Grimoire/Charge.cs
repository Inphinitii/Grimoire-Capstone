using UnityEngine;
using System.Collections;

public class Charge : MonoBehaviour {
	public float resetTime;
	public bool active;
	public Transform follow;
	public ParticleSystem onChargeUse;

	private float					m_internalTime;
	private TrailRenderer	m_trailRenderer;
	public void Default()
	{
		resetTime = 0.0f;
		active = true;
	}

	public void SetRechargeTime( float _rechargeTime )
	{
		resetTime = _rechargeTime;
	}

	public void ConsumeCharge()
	{
		Instantiate( onChargeUse, this.transform.position, Quaternion.identity );
		m_internalTime = resetTime;
		active = false;
	}

	void Start()
	{
		m_trailRenderer = GetComponent<TrailRenderer>();
	}

	void Update()
	{
		if ( !active )
		{
			if ( m_internalTime <= 0.0f )
				active = true;
			else
				m_internalTime -= Time.deltaTime;
		}
	}

	public TrailRenderer GetTrail() { return m_trailRenderer; }

	public void Follow(Vector3 _transform)
	{
		this.transform.position = _transform;
	}
}
