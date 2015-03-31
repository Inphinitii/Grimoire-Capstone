using UnityEngine;
using System.Collections;

[RequireComponent( typeof( BoxCollider2D ) )]
public class HumanoidHitbox : MonoBehaviour
{

	public Transform hitboxAnchor;

	private BoxCollider2D m_collider;
	Quaternion m_rotation;
	// Use this for initialization
	void Start()
	{
		m_collider = GetComponent<BoxCollider2D>();
		this.transform.position = hitboxAnchor.position;
	}

	// Update is called once per frame
	void Update()
	{
		this.transform.position = hitboxAnchor.position;
		m_rotation = this.transform.rotation;

		if ( hitboxAnchor.rotation.y > 300 )
		{
			m_collider.offset = new Vector2( m_collider.offset.x + 0.1f, m_collider.offset.y + 0.1f);

		}
		m_rotation.x = 0.0f;
		m_rotation.y = 0.0f;
		this.transform.rotation = m_rotation;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawCube(this.transform.position + (Vector3)m_collider.offset, (Vector3)m_collider.size);
	}
}
