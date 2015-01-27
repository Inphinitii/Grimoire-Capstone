using UnityEngine;
using System.Collections;

public class PhysicsController : MonoBehaviour {
	
	//Public Variables
	public float p_gravitationalForce;	
	public float p_groundFriction = 20.0f;
	public float p_airFriction = 5.0f;
	public float p_mass = 10.0f;
	public bool  p_applyGravity;
	
	[Range(0.0f, 1.0f)]
	public float p_dragResistance 	= -0.15f;
	
	//Private Variables
	private Vector2 m_velocity;
    private Vector2 m_finalVelocity;
	private Vector2 m_acceleration;
	private Vector2 m_force;
    private Vector2 m_position;

    private float gravity;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
        m_position = (Vector2)transform.position;

        if (p_applyGravity)
            m_force.y += -p_gravitationalForce;
        else
            m_velocity.y = 0.0f;
		
	//	m_acceleration += new Vector2 / p_mass;
        //m_velocity  += m_force * Time.deltaTime; //Gravity

        m_acceleration = m_force / p_mass;
        m_velocity += m_acceleration * Time.deltaTime;
        m_position += m_velocity * Time.deltaTime;

        transform.position = (Vector3)m_position;
        m_force = Vector2.zero;
	}
		
	public Vector2 Velocity 	{ get{return m_velocity;} 		set{m_velocity = value;		} }
	public Vector2 Acceleration { get{ return m_acceleration;} 	set{m_acceleration = value;	} }
	public Vector2 Forces   	{ get{return m_force;   } 		set{m_force = value; 		} }
    public Vector3 Position     { get { return transform.position; } set { transform.position = value; } }
    
    public void AddToVelocity(Vector2 _vel)
    {
        m_velocity += _vel;
    }

    public void AddToForce(Vector2 _force) {
        m_force += _force;
    }
	
	
	
}
