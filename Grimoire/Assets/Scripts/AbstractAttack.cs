using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class AbstractAttack : MonoBehaviour {


    public AbstractHurtBox[]  boxColliders;
    public Properties.ForceType forceType;

	public float duration;
	public float startupTime;
	public float cooldownTime;

	public float dashWindow;
	private bool m_dashAvailable;


    public virtual void Start()
    {
        AbstractHurtBox _temp;
        for (int i = 0; i < boxColliders.Length; i++)
        {
            _temp = (AbstractHurtBox)Instantiate( boxColliders[i], this.transform.position, transform.parent.transform.rotation );
            _temp.transform.parent = this.transform;
            _temp.SetForce( forceType );
        }
	}
	
	public virtual void Update () 
	{

	}

	public virtual void OnEnable()
	{

	}

	/// <summary>
	/// Sets the 'force' of the attack. This means what color it has, or what team it belongs to.
	/// </summary>
	/// <param name="_force">The force value.</param>
   public void SetForce( Properties.ForceType _force )
    {
        forceType = _force;
        for ( int i = 0; i < boxColliders.Length; i++ )
        {
            boxColliders[i].SetForce( _force );
        }
    }

	/// <summary>
	/// Get a float for use in the FSM.
	/// </summary>
   /// <returns>Return a float that is used in the FSM to block state switching.</returns>
   public float GetStateBlockTime()
   {
	   return startupTime + duration + cooldownTime;
   }

	/// <summary>
	/// Handle the input appropriately for this particular attack. 
	/// </summary>
	/// <param name="_input">The input handler reference of the actor.</param>
	public virtual void HandleInput (InputHandler _input)
   {
			Debug.Log( "Ding" );
		if(m_dashAvailable)
		{
			if(_input.LeftStick().x != 0.0f)
			{
				Debug.Log( "Dash!" );
			}
		}
   }

	/// <summary>
	/// Called when an AbstractHurtBox makes contact with an enemy. 
	/// </summary>
	public virtual void HitEnemy()
	{
		StartCoroutine( DashWindow( dashWindow ) );
	}

	/// <summary>
	/// The window of time that, when activated, allows the player to perform a dash. 
	/// </summary>
	/// <param name="_time">The window of opportunity in seconds.</param>
	/// <returns></returns>
	public IEnumerator DashWindow(float _time)
	{
		m_dashAvailable = true;
		yield return new WaitForSeconds( _time );
		m_dashAvailable = false;
	}

	/// <summary>
	/// Called to start the specific attack.
	/// </summary>
	/// <returns></returns>
	public IEnumerator StartAttack()
	{
		yield return new WaitForSeconds( startupTime );
		gameObject.SetActive( true );
		yield return new WaitForSeconds(  duration );
		gameObject.SetActive( false );
		yield return new WaitForSeconds( cooldownTime );
	}
}
