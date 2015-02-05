using UnityEngine;
using System.Collections;

public class SpawnWithVelocity : MonoBehaviour {

    public GameObject Prefab;
    public Transform SpawnLocation;
    public Vector2 Direction;
    public float Speed;

    private GameObject reference;

    private InputHandler mInputReference;
	// Use this for initialization
	void Start () {
        mInputReference = GetComponentInParent<InputHandler>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnFire() {
        reference = (GameObject)Instantiate(Prefab, SpawnLocation.position, Quaternion.identity);
        //reference.GetComponent<Rigidbody2D>().AddForce(mInputReference.LeftStick() * Speed);
        reference.GetComponent<Rigidbody2D>().velocity = mInputReference.LeftStick() * Speed;
    }
}
