using UnityEngine;
using System.Collections;

/*========================================================
 * Author: Tyler Remazki
 *
 * Class : Camera Script
 *
 * Description: Controls the movement and position of the camera based on the Foci passed to it.
 =========================================================*/

public class CameraScript : MonoBehaviour
{

	//Public Properties
	public GameObject[]	cameraFoci;
	public float					cameraFollowDelay;
	public float					cameraZoomDelay;
	public bool					cameraFollow;
	public bool					dynamicCameraZoom;

	public float maximumZoom;
	public float minimumZoom;

	//Private Properties
	private Camera	m_cameraReference;
	private Vector3	m_cameraPosition;
	private Vector3	m_origCamOffset;
	private Vector2	m_focalPoint;
	private float			m_cameraMagnification;




	void Start()
	{
		m_cameraReference		= GetComponent<Camera>();
		m_cameraPosition			= m_cameraReference.transform.position;
		m_origCamOffset			= m_cameraPosition;
		m_cameraMagnification	= CameraMagnification();
	}

	void FixedUpdate()
	{

		if ( cameraFollow )
			ReturnFocalPoint();
		if ( dynamicCameraZoom )
			m_cameraMagnification = CameraMagnification();

		if ( m_cameraMagnification > maximumZoom )
			m_cameraMagnification = maximumZoom;
		else if ( m_cameraMagnification < minimumZoom )
			m_cameraMagnification = minimumZoom;

		m_cameraPosition = new Vector3(
		Mathf.Lerp( m_cameraPosition.x, m_focalPoint.x, Time.deltaTime * cameraFollowDelay ),
		Mathf.Lerp( m_cameraPosition.y, m_focalPoint.y, Time.deltaTime * cameraFollowDelay ),
		Mathf.Lerp( m_cameraPosition.z, -m_cameraMagnification, Time.deltaTime * cameraZoomDelay ) );

		m_cameraReference.transform.position = m_cameraPosition + new Vector3( m_origCamOffset.x, m_origCamOffset.y, 0.0f );
	}

	/// <summary>
	/// Returns the Vector2 point between all of the Foci in m_CameraFoci
	/// </summary>
	void ReturnFocalPoint()
	{
		m_focalPoint = Vector2.zero;
		for ( int i = 0; i < cameraFoci.Length; i++ )
		{
			m_focalPoint += new Vector2(	cameraFoci[i].transform.position.x,
																cameraFoci[i].transform.position.y );
		}
		m_focalPoint = m_focalPoint / cameraFoci.Length;
	}

	///  <Summary>
	///  Manually the focus of the camera and remove the follow effect. 
	///  </Summary>
	public void SetFocalPoint( Vector2 _focalPoint )
	{
		cameraFollow	= false;
		m_focalPoint	= _focalPoint;
	}

	///  <Summary>
	///  Manually set the level of magnification for the camera and disable automatic magnificiation 
	///  </Summary>
	public void SetCameraZoom( float _zoom )
	{
		dynamicCameraZoom	= false;
		m_cameraMagnification	= _zoom;
	}

	///  <Summary>
	///  Lock the camera's ability to follow the focal point
	///  </Summary>
	public void LockCameraFollow()
	{
		cameraFollow = false;
	}

	///  <Summary>
	///  Resume the camera's ability to follow the focal point
	///  </Summary>
	public void ResumeCameraFollow()
	{
		cameraFollow = true;
	}

	///  <Summary>
	///  Lock the camera's ability to zoom based on the list of Foci 
	///  </Summary>
	public void LockCameraZoom()
	{
		dynamicCameraZoom = false;
	}

	///  <Summary>
	/// Resume the camera's ability to zoom based on the list of Foci
	///  </Summary>
	public void ResumeCameraZoom()
	{
		dynamicCameraZoom = true;
	}

	/// <Summary>
	///  Find two Vector2's. A minimum and a maximum based on their X and Y positions.
	/// Compare the distance between these Vector2's and use the magnitude to return an appropriate scalar for the camera.
	/// </Summary>
	float CameraMagnification()
	{
		//Since we need at least one focal point for the script to work, let's assume our base values are the first focal point in the array. 
		Vector2 _maximum = cameraFoci[0].transform.position;
		Vector2 _minimum	= cameraFoci[0].transform.position;

		for ( int i = 0; i < cameraFoci.Length; i++ )
		{
			if ( cameraFoci[i].transform.position.x <= _minimum.x )
				_minimum.x = cameraFoci[i].transform.position.x;

			if ( cameraFoci[i].transform.position.x >= _maximum.x )
				_maximum.x = cameraFoci[i].transform.position.x;

			if ( cameraFoci[i].transform.position.y <= _minimum.y )
				_minimum.y = cameraFoci[i].transform.position.y;

			if ( cameraFoci[i].transform.position.y >= _maximum.y )
				_maximum.y = cameraFoci[i].transform.position.y;

		}
		
		return ( _maximum - _minimum ).magnitude;
	}


}