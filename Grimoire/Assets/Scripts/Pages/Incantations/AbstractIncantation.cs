using UnityEngine;
using System.Collections;

public abstract class AbstractIncantation
{
    int mDamage;
    int mSpeed;

    float mStartupTime;               //Determines how long before the spell goes off.
    float mVulnerabilityTime;       //Determines the amount of time after the spell that the player is left in a vulnerable state
    float mCooldown;                  //Cooldown before you can use this incantation again

    Vector2 mHitDirection;          //The direction that the hit player will travel in 

    GameObject mIncantationPrefab;

    public abstract void OnUse();
    public abstract void OnHit();
    public abstract void OnRelease();
}
