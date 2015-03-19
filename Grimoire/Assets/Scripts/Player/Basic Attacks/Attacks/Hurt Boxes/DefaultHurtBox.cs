using UnityEngine;
using System.Collections;

public class DefaultHurtBox : AbstractHurtBox 
{

	public override void OnFriendlyHit( Collider2D _collider )
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
