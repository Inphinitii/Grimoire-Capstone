using UnityEngine;
using System.Collections;

public class ParticleManager : MonoBehaviour
{

	public ParticleSystem jumpingParticle;
	public ParticleSystem skidParticle;
	public ParticleSystem smokeOnHitParticle;

	//Internally kept particle systems that don't need constant recreating.
	private ParticleSystem m_skidParticle;
	private ParticleSystem m_smokeHit;


	void Start()
	{
		m_skidParticle			= (ParticleSystem)Instantiate( skidParticle, this.transform.position, Quaternion.identity )					as ParticleSystem;
		m_smokeHit			= (ParticleSystem)Instantiate( smokeOnHitParticle, this.transform.position, Quaternion.identity )	as ParticleSystem;



		m_skidParticle.transform.SetParent( this.transform );
		m_smokeHit.transform.SetParent( this.transform );


		SetSkidParticle( false );
		SetSmokeHitParticle( false );
	}


	void Update()
	{

	}

	public void SetSkidParticle( bool _active )
	{
		m_skidParticle.enableEmission = _active;
	}

	public void SetSmokeHitParticle( bool _active )
	{
		m_smokeHit.enableEmission = _active;
	}

	public void JumpParticle()
	{
		Instantiate( jumpingParticle, this.transform.position, jumpingParticle.gameObject.transform.rotation );
	}
}
