using UnityEngine;
using System.Collections;

/*========================================================
 * Author: Tyler Remazki
 *
 * Class : Camera Shake 
 * Reference Used : http://answers.unity3d.com/questions/212189/camera-shake.html
 *
 * Description: Camera shake causes the camera position and rotation to be momentarily shifted
 * by a random value, giving a screen-shake effect. Use this class to give a heavier feeling to particular
 * actions performed by the user/NPC.
 =========================================================*/

public class CameraShake : MonoBehaviour {

	private bool shaking; 

	private float m_shakeDecay; 
	private float m_shakeIntensity;

	private Vector3		m_originalPosition;
	private Quaternion	m_originalRot;
	
	private const float SHAKE_INTENSITY = 0.075f;
	private const float SHAKE_DECAY		= 0.03f;

	void Start()
	{
		shaking = false;
	}


	void Update()
	{
		//if ( m_shakeIntensity > 0 )
		//{
		//	//Randomize the position of the camera by the shake intensity.
		//	transform.position = m_originalPosition + Random.insideUnitSphere * m_shakeIntensity;

		//	//Randomize the rotation of the camera by the shake intensity. 
		//	transform.rotation = new Quaternion(	m_originalRot.x + Random.Range( -m_shakeIntensity, m_shakeIntensity ) * .2f,
		//																	m_originalRot.y + Random.Range( -m_shakeIntensity, m_shakeIntensity ) * .2f,
		//																	m_originalRot.z + Random.Range( -m_shakeIntensity, m_shakeIntensity ) * .2f,
		//																	1.0f );

		//	m_shakeIntensity -= m_shakeDecay;
		//}
		//else if ( shaking )
		//{
		//	shaking = false;
		//	//Reset to the original values to avoid the camera from being permanently rotated/positioned incorrectly. 
		//	transform.rotation = m_originalRot;
		//	transform.position = m_originalPosition;
		//}
	}

	/// <summary>
	/// Called externally to cause the camera to shake. 
	/// </summary>
	public void Shake()
	{
		m_originalPosition	= transform.position;
		m_originalRot			= transform.rotation;

		m_shakeIntensity	= SHAKE_INTENSITY;
		m_shakeDecay		= SHAKE_DECAY;
		shaking = true;
		StartCoroutine( ShakeCamera() );
	}

	IEnumerator ShakeCamera()
	{
		while ( m_shakeIntensity > 0 )
		{
			if ( m_shakeIntensity > 0 )
			{
				//Randomize the position of the camera by the shake intensity.
				transform.position = m_originalPosition + Random.insideUnitSphere * m_shakeIntensity;

				//Randomize the rotation of the camera by the shake intensity. 
				transform.rotation = new Quaternion( m_originalRot.x + Random.Range( -m_shakeIntensity, m_shakeIntensity ) * .2f,
																				m_originalRot.y + Random.Range( -m_shakeIntensity, m_shakeIntensity ) * .2f,
																				m_originalRot.z + Random.Range( -m_shakeIntensity, m_shakeIntensity ) * .2f,
																				1.0f );

				m_shakeIntensity -= m_shakeDecay;
			}
			yield return null;
		}
		shaking = false;
		//Reset to the original values to avoid the camera from being permanently rotated/positioned incorrectly. 
		transform.rotation = m_originalRot;
		transform.position = m_originalPosition;
		yield return null;
	}
}
