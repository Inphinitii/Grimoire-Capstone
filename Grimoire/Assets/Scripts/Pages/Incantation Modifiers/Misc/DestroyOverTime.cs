using UnityEngine;
using System.Collections;

public class DestroyOverTime : MonoBehaviour {

    public float timer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer <= 0.0f)
            Destroy(this.gameObject);
	}
}
