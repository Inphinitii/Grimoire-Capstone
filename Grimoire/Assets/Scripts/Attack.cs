using UnityEngine;
using System.Collections;

[System.Serializable]
public class Attack : MonoBehaviour {


    public AbstractHurtBox[]  boxColliders;
    public Properties.ForceType forceType;
    public Transform position;

    public Quaternion rotation;

	// Use this for initialization
    void Start()
    {
        AbstractHurtBox _temp;
        for (int i = 0; i < boxColliders.Length; i++)
        {
            _temp = (AbstractHurtBox)Instantiate( boxColliders[i], this.transform.position, transform.parent.transform.rotation );
            _temp.transform.parent = this.transform;
            _temp.SetForce( forceType );
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

   public  void SetForce( Properties.ForceType _force )
    {
        forceType = _force;
        for ( int i = 0; i < boxColliders.Length; i++ )
        {
            boxColliders[i].SetForce( _force );
        }
    }
}
