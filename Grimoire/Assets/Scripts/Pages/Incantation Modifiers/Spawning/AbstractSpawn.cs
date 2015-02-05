using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Force))]
public abstract class AbstractSpawn : MonoBehaviour {

    public Rigidbody2D            pGameObjectToSpawn;
    public Transform                pGameObjectSpawnLocation;
    public bool                         pSpawnInstantly;

    protected Force.ForceType    pForceType;

    public abstract void ActivateSpell();             //Used after a period of time.
    public abstract void UseIncantation(Force.ForceType _force);         //Used Instantly
    public abstract void ReleaseIncantation(Force.ForceType _force);  //Used when the button is released.
}
