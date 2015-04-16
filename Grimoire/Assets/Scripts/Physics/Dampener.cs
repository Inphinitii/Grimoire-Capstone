using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhysicsController))]
public class Dampener : MonoBehaviour
{
	[Range( 0, 1 )]
	public float xDampener;

	[Range( 0, 1 )]
	public float yDampener;

	private PhysicsController m_reference;

	// Use this for initialization
	void Start()
	{
		m_reference = GetComponent<PhysicsController>();
	}

	// Update is called once per frame
	void Update()
	{
		if ( m_reference.Velocity != Vector2.zero )
		{
			Vector3 temp = m_reference.Velocity;

			temp.x *= xDampener;
			temp.y *= yDampener;

			m_reference.GetComponent<PhysicsController>().Velocity = temp;
		}
	}
}
