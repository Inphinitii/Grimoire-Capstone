using UnityEngine;
using System.Collections;

public class ParticleManager : MonoBehaviour {

	public ParticleSystem jumpingParticle;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public ParticleSystem Jump() { return jumpingParticle; }

	public void JumpStart()
	{
		//Instantiate( jumpingParticle, this.transform.position, jumpingParticle.transform.rotation );
	}
}
