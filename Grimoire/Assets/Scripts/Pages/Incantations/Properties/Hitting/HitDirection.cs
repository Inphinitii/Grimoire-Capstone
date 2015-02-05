using UnityEngine;
using System.Collections;

public class HitDirection : MonoBehaviour {

    public Vector2 Direction;
    public bool SetDistance;
    public float Force;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //Propably needs revisioning
    void OnCollisionEnter2D(Collision2D _collider) {
        if (SetDistance)
            _collider.collider.attachedRigidbody.AddForce(Direction * Force);
        else
            _collider.collider.attachedRigidbody.AddForce(Direction * Force * _collider.collider.GetComponent<Properties>().Health);
    }


    void OnParticleCollision(GameObject _collider) {
        Debug.Log(_collider.name);
        if (SetDistance)
            _collider.GetComponentInParent<PhysicsController>().AddToForce(Direction * Force);
        else
            _collider.GetComponentInParent<PhysicsController>().AddToForce(Direction * Force * _collider.collider.GetComponent<Properties>().Health);
    }
}
