using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	Rigidbody2D rigidBody;
	BoxCollider2D collision;
	
	public float moveSpeed;
	public float airborneMoveSpeed; // seperate move speed value to improe airborne mobibilty
	
	//Acceleration on horizontal ground movement...
	public float acceleration;
	public float decceleration;
	public float currentSpeed;
	
	private float moveX;
	
	//Logic mainly found in "Jump" function to properly check if the player is grounded or not.
	public bool playerIsGrounded;
	private int groundContactCount;
	public float jumpForce;
	
	//GroundChecker
	[SerializeField] Transform groundCheckObject;
	[SerializeField] LayerMask groundLayer;
	public float groundCheckRadius;
	
	//Jump Height Control Variables
	public float jumpDecelRate;
	public bool playerIsJumping;
	
	//Coyote Time Variables
	public float coyoteTime;
    public float coyoteTimeCounter;
    
    //"IncreaseFall" Function Variables
    public float baseGravityScale;
    public float fallMultiplier;
    public float jumpMultiplier;
	
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }
	
	//Generally its better to have physics related code synced with FixedUpdate
	//However our "Jump" function logic kinda goofs when detecting the space button input when its in FixedUpdate
	//Mabye this could be fixed later but for now it shall stay in "Void Update()" for now.
	void FixedUpdate()
	{
		Run(); //Look to "Run" Function
	}
    void Update()
    {
		HandleJump(); //Look to "HandleJump" Function
		GroundCheck(); //Look to "GroundCheck" Function
		IncreaseFall(); //Look to "IncreaseFall" Function
    }
    
	private void Run()
	{
		moveX = Input.GetAxisRaw("Horizontal");

		if (moveX != 0)
		{
			currentSpeed = Mathf.MoveTowards(currentSpeed, moveX * moveSpeed, acceleration * Time.deltaTime);
		}
		else
		{
			currentSpeed = Mathf.MoveTowards(currentSpeed, 0, decceleration * Time.deltaTime);
		}

		rigidBody.velocity = new Vector2(currentSpeed, rigidBody.velocity.y);
		
		//airborne movement
		if (!playerIsGrounded)
		{
			float moveX = Input.GetAxisRaw("Horizontal");
			rigidBody.AddForce(Vector2.right * moveX * airborneMoveSpeed);
		}	
	}
	private void HandleJump()
	{
		if (Input.GetButtonDown("Jump") && (playerIsGrounded || coyoteTimeCounter > 0))
		{
			StartJump();
		}
		
		if (Input.GetButton("Jump") && playerIsJumping)
		{
			ContinueJump(); // Check if the jump button is still held down for fine control
		}
		
		if(Input.GetButtonUp("Jump") && !playerIsGrounded && playerIsJumping)
		{
			StopJump();
		}
		
		//Coyote Time Implementation
		if(playerIsGrounded)
		{
			coyoteTimeCounter = coyoteTime;
		}
		else
		{
			coyoteTimeCounter -= Time.deltaTime;
		}
	}
	
	private void StartJump()
	{
		playerIsJumping = true;
		
		rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
	}
	
	private void ContinueJump()
	{
		if (rigidBody.velocity.y < jumpForce &&  rigidBody.velocity.y > 0)
		{
			rigidBody.velocity = new Vector2(rigidBody.velocity.x, Mathf.Lerp(rigidBody.velocity.y, jumpForce, Time.deltaTime));
		}
	}
	
	private void StopJump()
	{		
		playerIsJumping = false;
		rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * 0.5f);
	}
	
	//Logic to check if GroundChecker object attatched to player is colliding with ground
	//be sure to tag the platforms as "Ground" in the editor
	private void GroundCheck()
	{
		playerIsGrounded = false;
		
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckObject.position, groundCheckRadius, groundLayer);
		
		if(colliders.Length > 0)
		{
			playerIsGrounded = true;
		}
	}
	
	private void IncreaseFall()
	{
		if(rigidBody.velocity.y < 0)
		{
			rigidBody.gravityScale = baseGravityScale * fallMultiplier;
		}
		else if(rigidBody.velocity.y < 0 && !Input.GetButton("Jump"))
		{
			rigidBody.gravityScale = baseGravityScale * jumpMultiplier;
		}
		else
		{
			rigidBody.gravityScale = baseGravityScale;
		}
	}
}
