using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Force))]
public abstract class AbstractIncantation : MonoBehaviour {

    public Force force;
    private Force.ForceType forceType;
	// Use this for initialization
	public void Start () {
        if (force == null)
            forceType = Force.ForceType.NONE;
        else
            forceType = force.ItemForce;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public virtual void HandleUse()
    {
        SendMessage("UseIncantation", forceType);
    }
    public virtual void HandleRelease()
    {
        SendMessage("ReleaseIncantation", forceType);
    }
}
