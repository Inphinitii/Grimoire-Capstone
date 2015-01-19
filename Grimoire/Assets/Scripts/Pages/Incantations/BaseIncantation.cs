using UnityEngine;
using System.Collections;

public abstract class BaseIncantation
{
    int mDamage;
    int mSpeed;
    GameObject mIncantationPrefab;

    public abstract void OnUse();
    public abstract void OnHit();
    public abstract void OnRelease();
}
