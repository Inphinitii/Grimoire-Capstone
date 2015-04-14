using UnityEngine;
using System.Collections;

/*========================================================
 * Author: Tyler Remazki
 *
 * Class : Actor
 *
 * Description: Actor holds all of the generic data necessary 
 * to each living actor in the game. Will also support some
 * utility functionality for the data structures within it
 =========================================================*/

[RequireComponent( typeof( BoxCollider2D ) )]
[RequireComponent( typeof( MovementController ) )]
public class Actor : MonoBehaviour
{
	public string actorName;
	public Color actorColor;
	public Properties.ForceType forceType;

	//-- Private Properties -- //
	Properties m_actorProperties;

	//-- Script References --//
	Rigidbody2D					m_rigidBody;
	BoxCollider2D				m_boxCollider;
	MovementController	m_movementController;
	PhysicsController			m_physicsController;
	Animator						m_componentAnimator;
	InputHandler				m_inputHandler;
	ParticleManager			m_particleManager;
	Renderer						m_renderer;
	SpellCharges				m_spellCharges;
	Grimoire						m_grimoire;

	private bool m_invulnerable;

	void Start()
	{
		actorName						= "Default";
		m_actorProperties			= new Properties();
		m_movementController	= GetComponent( typeof( MovementController ) )				as MovementController;
		m_physicsController		= GetComponent( typeof( PhysicsController ) )					as PhysicsController;
		m_boxCollider					= GetComponent( typeof( BoxCollider2D ) )						as BoxCollider2D;
		m_inputHandler				= GetComponent( typeof( InputHandler ) )							as InputHandler;
		m_spellCharges				= GetComponent( typeof( SpellCharges ) )						as SpellCharges;
		m_componentAnimator	= GetComponent( typeof( Animator ) )								as Animator;
		m_grimoire						= GetComponent( typeof( Grimoire ) )								as Grimoire;
		m_particleManager			= GetComponentInChildren( typeof( ParticleManager ) )	as ParticleManager;
		m_renderer						= GetComponentInChildren( typeof( Renderer ) )				as Renderer;
		m_renderer.material.color = actorColor;
		m_invulnerable = false ;
	}
	void Update()
	{

	}

	#region Utility Functions
	public Properties AccessProperties
	{
		get { return m_actorProperties; }
		set { m_actorProperties = value; }
	}
	public Properties.ForceType Force
	{
		get { return forceType; }
		set { forceType = value; }
	}
	public string Name
	{
		get { return actorName; }
		set { actorName = value; }
	}

	public MovementController GetMovementController()	{ return m_movementController; }
	public PhysicsController		GetPhysicsController()			{ return m_physicsController; }
	public ParticleManager			GetParticleManager()			{ return m_particleManager; }
	public SpellCharges				GetSpellCharges()				{ return m_spellCharges; }
	public InputHandler				GetInputHandler()				{ return m_inputHandler; }
	public Animator					GetAnimator()						{ return m_componentAnimator; }
	public Renderer					GetRenderer()						{ return m_renderer; }
	public Grimoire					GetGrimoire()						{ return m_grimoire; }
	public BoxCollider2D			GetCollider()						{ return m_boxCollider; }


	/// <summary>
	/// This utility function allows the actor to view the currently viewed components in the ComponentCollection
	/// script and add up the properties to get the Actors output properties.
	/// </summary>
	public void UpdateActorProperties()
	{
		m_actorProperties.Default();
	}

	/// <summary>
	/// Change the health value within the Actor's properties in accordance with the value parameter. Used to both damage and heal the actor. 
	/// </summary>
	public void IncrementHPBy( int _value )
	{
		m_actorProperties.Health += _value;
	}

	/// <summary>
	/// Function for use in the States that have no access to Unity functions. Call an IEnumerator through this GameObject.
	/// </summary>
	/// <param name="_coroutine">IEnumerator object.</param>
	public void StartChildCoroutine( IEnumerator _coroutine )
	{
		StartCoroutine( _coroutine );
	}

	public void SetInvulnerable(bool _state)
	{
		m_invulnerable = _state;
	}

	public bool GetInvulnerable()
	{
		return m_invulnerable;
	}

	public void OnHit()
	{
		//Deal with being hit
	}
	#endregion
}
