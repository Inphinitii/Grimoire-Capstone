using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class IncreaseSpeed : MonoBehaviour {

    public float multiplier;
    private Rigidbody2D reference;

	// Use this for initialization
	void Start () {
        reference = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        reference.velocity += reference.velocity * 0.01f * multiplier;
	}
}
