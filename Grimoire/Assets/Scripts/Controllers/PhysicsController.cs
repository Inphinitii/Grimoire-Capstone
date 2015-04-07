using UnityEngine;
using System.Collections;

public class PhysicsController : MonoBehaviour {
	
	//Public Variables
	public float gravitationalForce = 100.0f;
	public float mass						= 2.0f;
	public bool  applyGravity			= true;
	
	//Private Variables
	private Vector2 m_velocity;
	private Vector2 m_lastVelocity;
	private Vector2 m_acceleration;
	private Vector2 m_force;
    private Vector2 m_position;

    private float gravity;
    private bool pausePhysics;
	
	// Use this for initialization
	void Start () {
        pausePhysics = false;
        applyGravity = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!pausePhysics) {
			m_lastVelocity = m_velocity;
            m_position = (Vector2)transform.position;

            if (applyGravity)
				m_force.y += -gravitationalForce * mass ;

            m_acceleration = m_force / mass;

            m_velocity	+= m_acceleration * Time.deltaTime;
			m_position	+= m_velocity * Time.deltaTime;

			if ( m_velocity.x < 0.001 && m_velocity.x > 0.0f )
			{
				m_velocity.x = 0.0f;
			}
			else if ( m_velocity.x > -0.001 && m_velocity.x < 0.0f )
			{
				m_velocity.x = 0.0f;
			}

            transform.position = (Vector3)m_position;
            m_force = Vector2.zero;
        }
	}

	public Vector2 Velocity				{ get { return m_velocity; }				set { m_velocity = value;				} }
	public Vector2 LastVelocity		{ get { return m_lastVelocity; }			set { m_lastVelocity = value;			} }
	public Vector2 Acceleration		{ get{ return m_acceleration;} 			set{m_acceleration = value;			} }
	public Vector2 Forces   			{ get{return m_force;   } 					set{m_force = value; 						} }
    public Vector3 Position			{ get { return transform.position; }	set { transform.position = value;	} }

    public void PausePhysics(bool _pause) {
        pausePhysics = _pause;
    }

    public void AddToVelocity(Vector2 _vel)
    {
        m_velocity += _vel;
    }

    public void AddToForce(Vector2 _force) {
        m_force += _force;
    }

    public void ClearValues() {
        m_acceleration = Vector2.zero;
        m_force = Vector2.zero;
        m_velocity = Vector2.zero;
    }
	
	
	
}
