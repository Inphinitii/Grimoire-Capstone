using UnityEngine;
using System.Collections;

public class DefaultHurtBox : AbstractHurtBox 
{

	public override void OnHurtboxHit( Collider2D _collider )
	{
		base.OnHurtboxHit( _collider );
	}

	public override void OnFriendlyHit( Collider2D _collider )
    {
		base.OnFriendlyHit( _collider );
    }

	public override void OnEnemyHit( Collider2D _collider )
    {
        base.OnEnemyHit( _collider );
    }

    public override void OnAnyHit()
    {
		base.OnAnyHit();
    }
}
