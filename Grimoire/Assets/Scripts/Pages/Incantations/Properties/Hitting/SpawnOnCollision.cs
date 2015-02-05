using UnityEngine;
using System.Collections;

public class SpawnOnCollision : MonoBehaviour {

    public GameObject ExplosionPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D _collider) {
       // if (_collider.collider.tag != "Player") {
            Instantiate(ExplosionPrefab, this.transform.position, Quaternion.identity);
            GameObject.Destroy(this.gameObject);
        
    }

    void OnParticleCollision(GameObject _collider) {
        // if (_collider.collider.tag != "Player") {
        Instantiate(ExplosionPrefab, _collider.transform.position, Quaternion.identity);
        GameObject.Destroy(this.gameObject);

    }
}
