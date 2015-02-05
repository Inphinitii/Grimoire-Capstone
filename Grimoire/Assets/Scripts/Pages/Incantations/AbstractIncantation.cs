using UnityEngine;
using System.Collections;

public abstract class AbstractIncantation
{
    int mDamage;                      //Damage Inflicted
    int mSpeed;                       //Spells movement speed
    int mSpellOwner;                  //Indicate what player ID owns this spell.

    float mStartupTime;               //Determines how long before the spell goes off.
    float mVulnerabilityTime;         //Determines the amount of time after the spell that the player is left in a vulnerable state
    float mCooldown;                  //Cooldown before you can use this incantation again


    Vector2 mHitDirection;            //The direction that the hit player will travel in 

    GameObject mIncantationPrefab;
    GameObject mIncantationOnHitEffect;
    GameObject mIncantationOnUseEffect;


    public abstract void OnUse();     //Called when the button is pressed
    public abstract void OnRelease(); //Called when the button is released
    public abstract void OnHit();     //Called when the spell hits the enemy player.
}
