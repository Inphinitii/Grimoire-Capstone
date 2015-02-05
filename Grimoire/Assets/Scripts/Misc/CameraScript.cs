using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	
	//Public Properties
	public GameObject[] p_CameraFoci;
	public float        p_CameraFollowDelay;
	public float        p_CameraZoomDelay;
	public bool         p_CameraFollow;
	public bool         p_DynamicCameraZoom;
	
	public float p_maximumZoom;
	public float p_minimumZoom;
	
	//Private Properties
	private Camera  m_cameraReference;
	private Vector3 m_cameraPosition;
    private Vector3 m_origCamOffset;
	private Vector2 m_focalPoint;
	private float   m_cameraMagnification;
	
	
	
	void Start () {
		m_cameraReference = GetComponent<Camera>();
		m_cameraPosition  = m_cameraReference.transform.position;
        m_origCamOffset   = m_cameraPosition;
		m_cameraMagnification = CameraMagnification();
	}
	
	void FixedUpdate () {
		if (p_CameraFollow)
			ReturnFocalPoint();
		if (p_DynamicCameraZoom)
			m_cameraMagnification = CameraMagnification();
		
		if (m_cameraMagnification > p_maximumZoom)
			m_cameraMagnification = p_maximumZoom;
		else if (m_cameraMagnification < p_minimumZoom)
			m_cameraMagnification = p_minimumZoom;
		
		m_cameraPosition = new Vector3
			(
				Mathf.Lerp(m_cameraPosition.x, m_focalPoint.x,Time.deltaTime * p_CameraFollowDelay),
                Mathf.Lerp(m_cameraPosition.y, m_focalPoint.y, Time.deltaTime * p_CameraFollowDelay),
				Mathf.Lerp(m_cameraPosition.z, -m_cameraMagnification,Time.deltaTime * p_CameraZoomDelay)
				);
        m_cameraReference.transform.position = m_cameraPosition + new Vector3(m_origCamOffset.x, m_origCamOffset.y, 0.0f);
        //m_cameraReference.transform.LookAt(new Vector3(0.0f, m_focalPoint.y, 0.0f));
        
	}
	
	///  <Summary>
	///  <para> Returns the Vector2 point between all of the Foci in m_CameraFoci </para>
	///  </Summary>
	void ReturnFocalPoint(){
		m_focalPoint = Vector2.zero;
		for (int i = 0; i < p_CameraFoci.Length; i++){
			m_focalPoint += new Vector2(p_CameraFoci[i].transform.position.x,
			                            p_CameraFoci[i].transform.position.y);
		}
		m_focalPoint = m_focalPoint / p_CameraFoci.Length;
	}
	
	///  <Summary>
	///  <para> Manually the focus of the camera and remove the follow effect. </para>
	///  </Summary>
	public void SetFocalPoint(Vector2 _focalPoint){
		p_CameraFollow = false;
		m_focalPoint = _focalPoint;
	}
	
	///  <Summary>
	///  <para> Manually set the level of magnification for the camera and disable automatic magnificiation </para>
	///  </Summary>
	public void SetCameraZoom(float _zoom)
	{
		p_DynamicCameraZoom = false;
		m_cameraMagnification = _zoom;
	}
	
	///  <Summary>
	///  <para> Lock the camera's ability to follow the focal point </para>
	///  </Summary>
	public void LockCameraFollow()
	{
		p_CameraFollow = false;
	}
	
	///  <Summary>
	///  <para> Resume the camera's ability to follow the focal point </para>
	///  </Summary>
	public void ResumeCameraFollow(){
		p_CameraFollow = true;
	}
	
	///  <Summary>
	///  <para> Lock the camera's ability to zoom based on the list of Foci </para>
	///  </Summary>
	public void LockCameraZoom()
	{
		p_DynamicCameraZoom = false;
	}
	
	///  <Summary>
	///  <para> Resume the camera's ability to zoom based on the list of Foci </para>
	///  </Summary>
	public void ResumeCameraZoom(){
		p_DynamicCameraZoom = true;
	}
	
	/// <Summary>
	/// <para>  Find two Vector2's. A minimum and a maximum based on their X and Y positions.
	/// Compare the distance between these Vector2's and use the magnitude to return an appropriate scalar for the camera. </para>
	/// </Summary>
	float CameraMagnification(){
		Vector2 _maximum = Vector2.zero;
		Vector2 _minimum = Vector2.zero;
		
		for(int i = 0; i < p_CameraFoci.Length; i++){
			if (p_CameraFoci[i].transform.position.x <= _minimum.x)
				_minimum.x = p_CameraFoci[i].transform.position.x;
			if (p_CameraFoci[i].transform.position.x >= _maximum.x)
				_maximum.x = p_CameraFoci[i].transform.position.x;
			
			if (p_CameraFoci[i].transform.position.y <= _minimum.y)
				_minimum.y = p_CameraFoci[i].transform.position.y;
			if (p_CameraFoci[i].transform.position.y >= _maximum.y)
				_maximum.y = p_CameraFoci[i].transform.position.y;
		}
		return (_maximum - _minimum).magnitude;
	}
}