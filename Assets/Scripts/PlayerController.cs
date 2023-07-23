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
	
	//Jump Height Control variables
	public float jumpDecelRate;
	public bool playerIsJumping;
	
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
		//Jump(); //Look to "Jump" Function
		HandleJump(); //Look to "HandleJump" Function
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
	/*
	private void Jump()
	{ 
		if (Input.GetButtonDown("Jump") && playerIsGrounded)
		{
			rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
			
			playerIsGrounded = false;
		}
	}
	*/
	///////////////////////
	//////JUMP STUFF///////
	///////////////////////
	private void HandleJump()
	{
		if(Input.GetButtonDown("Jump") && playerIsGrounded && !playerIsJumping)
		{
			StartJump();
		}
		
		if(Input.GetButtonUp("Jump"))
		{
			StopJump();
		}
	}
	
	private void StartJump()
	{
		playerIsJumping = true;
		
		rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
	}
	
	private void StopJump()
	{
		playerIsJumping = false;
		
		rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpDecelRate);
	}
	
	///////////////////////
	///END OF JUMP STUFF///
	///////////////////////
	
	//Logic to check if player is grounded, be sure to tag the platforms as "Ground" in the editor
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
			groundContactCount++;
			playerIsGrounded = true;
		}
	}
	
	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
			groundContactCount--;
			if (groundContactCount <= 0)
			{
				groundContactCount = 0;
				playerIsGrounded = false;
			}
		}
	}
}
