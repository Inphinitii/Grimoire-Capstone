using UnityEngine;
using System.Collections;

public class SpawnSetDistance : MonoBehaviour {

    public GameObject Prefab;
    public Transform ReferenceLocation;
    public Vector2 Direction;
    public float Distance;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnFire() {
        Instantiate(Prefab, ReferenceLocation.position + (Vector3)(Direction * Distance), Quaternion.identity);
    }
}
