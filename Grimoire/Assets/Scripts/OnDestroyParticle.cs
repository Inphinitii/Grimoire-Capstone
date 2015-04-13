using UnityEngine;
using System.Collections;

public class OnDestroyParticle : MonoBehaviour {
	public ParticleSystem particleToUse;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDestroy()
	{
		//Debug.Log( "ding" );
		//Instantiate( particleToUse, this.gameObject.transform.position, Quaternion.identity );
	}
}
