using UnityEngine;
using System.Collections;

public class PhysicsController : MonoBehaviour {
	
	//Public Variables
	public float p_gravitationalForce;	
	public float p_groundFriction = 20.0f;
	public float p_airFriction = 5.0f;
	public float p_mass = 10.0f;
	public bool  p_applyGravity;
	
	[Range(-1.0f, 0.0f)]
	public float p_dragResistance 	= -0.15f;
	
	//Private Variables
	private Vector2 m_velocity;
    private Vector2 m_finalVelocity;
	private Vector2 m_acceleration;
	private Vector2 m_force;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Step()
	{
		if(p_applyGravity)
            m_force.y -= p_gravitationalForce;
		else
            m_acceleration.y = 0.0f;
		
		m_acceleration += m_force / p_mass;
        m_velocity = m_acceleration * Time.deltaTime;
        transform.position = transform.position + (Vector3)m_velocity * Time.deltaTime;

        m_force = Vector2.zero;

        
		
		//TODO
		//Take into account the properties of the actor
	}
	
	public Vector2 Velocity 	{ get{return m_velocity;} 		set{m_velocity = value;		} }
	public Vector2 Acceleration { get{ return m_acceleration;} 	set{m_acceleration = value;	} }
	public Vector2 Forces   	{ get{return m_force;   } 		set{m_force = value; 		} }
    public Vector3 Position     { get { return transform.position; } set { transform.position = value; } }
	
	
	
}
