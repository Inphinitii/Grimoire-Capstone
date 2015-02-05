using UnityEngine;
using System.Collections;

public abstract class ISpell : MonoBehaviour {

    public int ownerUID;
    public GameObject spellProjectile;
    public Properties ownerProperties;
    public ParticleSystem spellTrail;
    public ParticleSystem spellExplosion;

    public float CastTime;
    public float CooldownTime;
    public float VulnerabilityTime;
    public bool  FreezeMovement;

    private float cdTimer;

    public virtual void Start() { }
    public virtual void Update() {
        if (cdTimer > 0.0f)
            cdTimer -= Time.deltaTime;
        //Instantiate  the object
        //Start the vulnerability cooldown

    }
    void SetUID(int UID) {
        ownerUID = UID;
    }

    public virtual void OnUse() {
        if (cdTimer <= 0.0f) {
            ownerProperties.CastTime = CastTime;
            ownerProperties.State = Properties.PlayerState.CASTING;
            cdTimer = CooldownTime;
        }
    }
    public virtual void OnRelease() { }
    public virtual void OnHit() { }
}
