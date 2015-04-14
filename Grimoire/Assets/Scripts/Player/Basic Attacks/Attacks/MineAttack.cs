using UnityEngine;
using System.Collections;

/*========================================================
 * Author: Tyler Remazki
 *
 * Class : Default Attack
 *
 * Description: Default attack that uses the virtual implementation within the AbstractAttack. 
 =========================================================*/

public class MineAttack : AbstractAttack
{

	public GameObject		mineObject;
	public ParticleSystem onDestroyParticle;
	public float					mineLifetime;
	public bool					allowMovement;



	public override void Start()
	{
		m_exitFlag			= false;
	}

	public override void Update()
	{
		base.Update();
	}

	public override IEnumerator StartAttack()
	{
		m_parentActor = this.transform.parent.gameObject.GetComponent<Actor>();
		yield return new WaitForSeconds( Startup() );
		SpawnMineObject();
		yield return new WaitForSeconds( Cooldown() );
	}

	public override void HandleInput( InputHandler _input )
	{
		base.HandleInput( _input );
	}

	public override void AfterAttack()
	{
		throw new System.NotImplementedException();
	}

	public override void DuringAttack()
	{
		throw new System.NotImplementedException();
	}

	public override void BeforeAttack()
	{
		throw new System.NotImplementedException();
	}

	public override void HitHurtBox( Collider2D _collider )
	{
		base.HitHurtBox( _collider );
	}

	private void SpawnMineObject()
	{
		GameObject _mine = (GameObject)Instantiate( mineObject, m_parentActor.gameObject.transform.position + new Vector3( 0.0f, 1.0f, 0.0f ), Quaternion.identity );
		_mine.AddComponent<OnDestroyParticle>().particleToUse = onDestroyParticle;

		//Auto cleanup
		_mine.AddComponent<DestroyOverTime>().enabled = false;
		_mine.GetComponent<DestroyOverTime>().timer = 4.0f;

		ParticleSystem _particles = (ParticleSystem)Instantiate( particleOnUse, _mine.transform.position, Quaternion.identity );
		_particles.transform.parent = _mine.transform;

		AbstractHurtBox _temp;
		for ( int i = 0; i < boxColliders.Length; i++ )
		{
			_temp = (AbstractHurtBox)Instantiate( boxColliders[i], this.transform.position, transform.parent.transform.rotation );
			_temp.SetReference( _mine );
			_temp.SetForce( forceType );
			_temp.EnableHurtBox();
		}
	}
}
