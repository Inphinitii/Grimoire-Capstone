using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class HurtBox : MonoBehaviour {

    public Vector2 m_hitDirection;
    public float m_hitForce;
    public bool m_staticForce;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D _collider){
        if (_collider.gameObject.tag == "Player")
        {
            Debug.Log("Ding");
            _collider.gameObject.GetComponent<PhysicsController>().Velocity = m_hitDirection;
            _collider.gameObject.GetComponent<PhysicsController>().Forces = m_hitDirection * m_hitForce;
        }
    }
}
