using UnityEngine;
using System.Collections;

public class ReplaceOnHit : MonoBehaviour {

    public GameObject ReplacementPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D _collision)
    {
        Instantiate(ReplacementPrefab, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
