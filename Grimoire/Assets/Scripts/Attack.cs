using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {

    public HurtBox[]  m_BoxColliders;

	// Use this for initialization
	void Start () {
        HurtBox _temp;
        for (int i = 0; i < m_BoxColliders.Length; i++)
        {
            _temp = (HurtBox)Instantiate(m_BoxColliders[i], this.transform.position, Quaternion.identity);
            _temp.transform.parent = this.transform;
        }
        this.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
