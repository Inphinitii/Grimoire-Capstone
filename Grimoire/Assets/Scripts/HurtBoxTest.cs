using UnityEngine;
using System.Collections;

public class HurtBoxTest : AbstractHurtBox 
{

	public override void OnFriendlyHit(Collider2D _collider)
    {

    }

    public override void OnEnemyHit( Collider2D _collider )
    {
        base.OnEnemyHit( _collider );
    }

    public override void OnAnyHit()
    {
        
    }
}
