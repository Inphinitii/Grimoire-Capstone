using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {


    public AbstractHurtBox[]  boxColliders;
    public Properties.ForceType forceType;

	// Use this for initialization
    void Awake()
    {
        AbstractHurtBox _temp;
        for (int i = 0; i < boxColliders.Length; i++)
        {
            _temp = (AbstractHurtBox)Instantiate( boxColliders[i], this.transform.position, Quaternion.identity );
            _temp.transform.parent = this.transform;
            _temp.SetForce( forceType );
        }
        this.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
