using UnityEngine;
using System.Collections;

public class InheritColor : MonoBehaviour
{
	public Color reference;

	void Awake()
	{
		GetComponent<ParticleSystem>().startColor = reference;
	}
	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
