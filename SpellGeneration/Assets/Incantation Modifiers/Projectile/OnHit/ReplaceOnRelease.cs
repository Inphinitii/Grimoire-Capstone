using UnityEngine;
using System.Collections;

public class ReplaceOnRelease : MonoBehaviour {

    public GameObject ReplacementPrefab;
    public float Delay;

    private bool pressed;
    private GameObject obj;

	// Use this for initialization
	void Start () {
        pressed = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (pressed)
            StartCoroutine(DelayExplosion());
	}

    public void ReleaseIncantation(Force.ForceType _force)
    {
        pressed = true;
    }

    IEnumerator DelayExplosion()
    {
        yield return new WaitForSeconds(Delay);
        obj = (GameObject)Instantiate(ReplacementPrefab, this.transform.position, Quaternion.identity);
        obj.AddComponent<Force>().ItemForce = this.GetComponent<Force>().ItemForce;
        pressed = false;
        Destroy(this.gameObject);
    }
}
