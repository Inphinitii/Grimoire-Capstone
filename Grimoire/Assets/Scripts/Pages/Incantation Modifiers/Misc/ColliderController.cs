using UnityEngine;
using System.Collections;

public class ColliderController : MonoBehaviour {

    public float EnableInSeconds;
    bool fired;
	// Use this for initialization
	void Start () {
        fired = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!fired && EnableInSeconds > 0.0f)
            StartCoroutine(CollisionBox());
	}

    IEnumerator CollisionBox()
    {
        fired = true;
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(EnableInSeconds);
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
