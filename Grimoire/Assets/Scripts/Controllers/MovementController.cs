using UnityEngine;
using System.Collections;

[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(PhysicsController))]
[RequireComponent(typeof(GroundCheck))]
public class MovementController : MonoBehaviour {

	//TODO NEEDS REFACTORING 
    //Acceleration
    public float groundAccel	= 100.0f;
    public float airAccel		= 25.0f;
    public float jumpAccel		= 12.0f;

    //Maximum Accelerations
    public float maxGroundAccel	= 12.0f;
    public float maxAirAccel	= 12.0f;

    //Dampeners
    [Range( 0, 1)]
    public float groundDampeningConstant	= 0.88f;
    [Range( 0, 1 )]
    public float airDampeningConstant		= 0.98f;

    //Turning Modifiers
    public float groundTurningConstant	= 3.0f;
    public float airTurningConstant		= 0.1f;

	//Fast Fall Variables
    public float fastFallRate = 100.0f;

	public bool groundCheck;

	//Jumping Variables
    public int	 totalJumps = 2;
    private int  jumpCount	= 0;

	//Collision Variables
	public	LayerMask groundCheckLayerMask;
	public	LayerMask platformLayerMask;

	private LayerMask   mCurrentMask;

	//Reference Variables
	private Actor				m_actorReference;
    private PhysicsController	m_physicsController;
    private InputHandler		m_inputHandler;
	private GroundCheck		    m_groundCheck;

	//Movement Booleans
    private bool		m_isJumping;
    private bool		m_isMoving;
	private bool		m_facingRight;
	private bool		m_onPlatform;
	private bool		m_fallThrough;
	private bool		m_onGround;
	public bool		m_capAcceleration = true;

	//Temporary forces to be added to the PhysicsController.
    private Vector2 m_tempForce;
    private Vector2 m_tempVel;

	//Signs to determine orientation.
    private int signLastFrame;
    private int sign;

    private float turningMultiplier;
    private float turningSpeedType;
    private float movementSpeedType;
	private float dampeningConstant;


	private float temp;

    //TODO FALLING THROUGH PLATFORMS 

    void Start() {
        m_inputHandler		= GetComponent<InputHandler>();
        m_physicsController	= GetComponent<PhysicsController>();
		m_groundCheck		= GetComponent<GroundCheck>();
		m_actorReference	= GetComponent<Actor>();
        m_isJumping			= true;
		groundCheck			= true;
		m_capAcceleration	= true;

		if ( transform.localRotation.y == 0.0f )
			m_facingRight = true;
		else
			m_facingRight = false;
    }

	void Update()
	{
		turningMultiplier = 1.0f;

		turningSpeedType		= !m_isJumping ? groundTurningConstant : airTurningConstant;
		movementSpeedType		= !m_isJumping ? groundAccel : airAccel;
		dampeningConstant		= !m_isJumping ? groundDampeningConstant : airDampeningConstant;

		if ( m_capAcceleration ) 
			CapAcceleration();

		DampenMovement();
		ApplyTurningSpeed( ref turningMultiplier );

		if ( m_physicsController.Velocity.y < 0 )
			GroundCheck();

		signLastFrame = sign;

	}

	void LateUpdate()
	{
		OneWayPlatform();
	}


    void FixedUpdate() {         
        m_physicsController.AddToForce(m_tempForce);
        m_physicsController.AddToVelocity(m_tempVel);

        m_tempForce = Vector2.zero;
        m_tempVel   = Vector2.zero;
    }



	/// <summary>
	/// Apply a jump force to the player character when a button is pressed. The longer the button is pressed, the longer the force is applied.
	/// This allows for 'Short Hopping'.
	/// </summary>
	/// <param name="_buttonPressed"> Boolean representing if the button has been pressed or not.</param>
	/// <param name="xDirection">X Direction Influence for use in the second jump.</param> //DO THIS LATER <----- <----
    public void ApplyJump(bool _buttonPressed, Vector2 xDirection) 
	{
		if ( !m_isJumping )
		{
			jumpCount = 0;
			temp = 0.0f;
			m_isJumping = true;
		}
 
		if ( _buttonPressed && jumpCount < totalJumps )
		{
			if(temp == 0.0f)
				m_actorReference.GetParticleManager().JumpParticle();

			if ( temp < 0.20f )
			{
				m_physicsController.Velocity = new Vector2( m_physicsController.Velocity.x, jumpAccel );
			}

			temp += ( Time.deltaTime );
		}
		else if ( !_buttonPressed )
		{
			temp = 0.0f;
			jumpCount++;
		}
		
    }

	/// <summary>
	/// Apply a force downwards when the player wants to 'fast fall'. This is used in the Jumping State. 
	/// </summary>
	public void ApplyFastFall()
	{
		m_tempForce.y -= fastFallRate;
	}

	/// <summary>
	/// Used externally by other controllers to move our character into the supplied direction. 
	/// </summary>
	/// <param name="m_direction">The direction of movement.</param>
    public void MoveX(Vector2 _direction)
    {
		OrientationCheck( _direction );
        if (m_isMoving)
        {
				m_tempForce.x += ( _direction.x * movementSpeedType );
        }
    }

	/// <summary>
	/// Check the orientation of the character based
	/// </summary>
	/// <param name="_stick">The input vector corresponding to the left stick of the Xbox Controller.</param>
    public void OrientationCheck(Vector2 _stick) {
		float deadZone = 0.4f;

		if ( _stick.x > deadZone )
        {
            m_isMoving = true;
			signLastFrame = sign;
            sign = 1;

            if (!m_isJumping)
                if (!m_facingRight) 
				{
					m_facingRight = true;
					transform.rotation = new Quaternion( 0, 0, 0, 1 );
				}

        }
		else if ( _stick.x < -deadZone )
        {
            m_isMoving = true;
			signLastFrame = sign;
            sign = -1;

            if (!m_isJumping)
				if ( m_facingRight ) 
				{
					m_facingRight = false;
					transform.rotation = new Quaternion( 0, 180, 0, 1 );
                }

        }
    }

	/// <summary>
	/// Apply a dampening force to the players velocity. If this velocity is zero, our character has stopped moving. 
	/// </summary>
    public void DampenMovement()
    {
		m_isMoving = m_inputHandler.LeftStick().x != 0 ? true : false;
		if ( m_physicsController.Velocity.x != 0.0f )
		{
			m_physicsController.Velocity = new Vector2( m_physicsController.Velocity.x * dampeningConstant, m_physicsController.Velocity.y );
		}

		//Skid particles. Separate these later, messy.
		if (!m_isJumping && Mathf.Abs( m_physicsController.Velocity.x ) > 1.5f )
		{
			m_actorReference.GetParticleManager().SetSkidParticle( true );
		}
		else
		{
			m_actorReference.GetParticleManager().SetSkidParticle( false );
		}
    }


	/// <summary>
	/// Multiply a constant modifier to the movement speed when the orientation changes to simulate a skidding effect. 
	/// </summary>
	/// <param name="_turningSpeed">The turning speed variable.</param>
    void ApplyTurningSpeed(ref float _turningSpeed) {
        if (signLastFrame != sign && sign != 0) {
            _turningSpeed = turningSpeedType * sign;
            m_tempForce.x += Mathf.Abs(m_physicsController.Velocity.x) * _turningSpeed;
        }
    }

	/// <summary>
	/// Check for collision with One Way Platforms when moving upwards on the Y axis. 
	/// </summary>
    public void OneWayPlatform()
    {
		BoxCollider2D[] objects = GameManager.GetStageObject().GetPlatforms();
		if ( m_isJumping || m_physicsController.Velocity.y > 0.0f )
        {
			for(int i = 0; i < objects.Length; i++)
			{
				Physics2D.IgnoreCollision( this.GetComponent<BoxCollider2D>(), objects[i], true );
			}
        }
        else
        {
			for ( int i = 0; i < objects.Length; i++ )
			{
				Physics2D.IgnoreCollision( this.GetComponent<BoxCollider2D>(), objects[i], false );
			}
        }
    }

	public void FallThrough()
	{
		BoxCollider2D[] objects = GameManager.GetStageObject().GetPlatforms();
		if (m_onPlatform )
		{
			for ( int i = 0; i < objects.Length; i++ )
			{
				Physics2D.IgnoreCollision( this.GetComponent<BoxCollider2D>(), objects[i], true );
				m_onPlatform = false;
				m_fallThrough = true;
			}
		}
		else if(!m_fallThrough)
		{
			for ( int i = 0; i < objects.Length; i++ )
			{
				Physics2D.IgnoreCollision( this.GetComponent<BoxCollider2D>(), objects[i], false );
			}
		}
	}

	/// <summary>
	/// Check whether or not the player is grounded using a single raycast.
	/// </summary>
    public void GroundCheck() {
		m_onGround	= m_groundCheck.CastRayVelocity( m_physicsController.Velocity.y, groundCheckLayerMask );

		if(m_fallThrough)
			m_fallThrough = !m_onGround;

		if(!m_fallThrough)
		m_onPlatform = m_groundCheck.CastRayVelocity( m_physicsController.Velocity.y, platformLayerMask );

		if ( !m_onGround && !m_onPlatform )
		{
			if ( m_isJumping == false )
				jumpCount++;

			m_isJumping = true;
			m_fallThrough = false;
		}
		else if ( m_onGround || m_onPlatform )
		{
			m_physicsController.Velocity = new Vector2( m_physicsController.Velocity.x, 0 );
			m_isJumping = false;
			jumpCount = 0;
		}
    }


	/// <summary>
	/// Cap the velocity of the characters movement speed.
	/// </summary>
    void CapAcceleration() {
        float value = !m_isJumping ? maxGroundAccel : maxAirAccel;
        if (m_physicsController.Velocity.x > value)
            m_physicsController.Velocity = new Vector2(value, m_physicsController.Velocity.y);
        if (m_physicsController.Velocity.x < -value)
            m_physicsController.Velocity = new Vector2(-value, m_physicsController.Velocity.y);
    }

	/// <summary>
	/// Returns whether or not the character is currently jumping. 
	/// </summary>
	/// <returns>Jump variable </returns>
    public bool IsJumping()		{ return m_isJumping; }

	/// <summary>
	/// Returns whether or not the character is currently moving.
	/// </summary>
	/// <returns>Movement variable.</returns>
	public bool IsMoving()		{ return m_isMoving; }

	/// <summary>
	/// Returns whether or not the character is facing right.
	/// </summary>
	/// <returns>Facing Right Boolean</returns>
	public bool IsFacingRight() { return m_facingRight; }

	/// <summary>
	/// Returns whether or not the character is currently on a platform. 
	/// </summary>
	/// <returns>On Platform Boolean</returns>
	public bool IsOnPlatform() { return m_onPlatform;  }

	/// <summary>
	/// Returns whether or not the character is currently on a platform. 
	/// </summary>
	/// <returns>On Platform Boolean</returns>
	public bool IsOnGround() { return m_onGround; }

	/// <summary>
	/// Returns the orientation sign of the previous frame.
	/// </summary>
	/// <returns>Orientation sign of the previous frame.</returns>
	public int SignLastFrame() { return signLastFrame; }

	/// <summary>
	/// Returns the orientation sign of the current frame.
	/// </summary>
	/// <returns>Orientation sign of the current frame.</returns>
	public int SignThisFrame() { return sign; }

	/// <summary>
	/// Set the jumping variable to the one supplied.
	/// </summary>
	public void SetJumping( bool _jumping ) { m_isJumping = _jumping; }

	public int JumpCount()
	{
		return jumpCount;
	}

	public int JumpTotal()
	{
		return totalJumps-1;
	}

}