using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	Rigidbody2D rigidBody;
	BoxCollider2D collision;
	
	public float moveSpeed;
	public float jumpForce;
	
	//Acceleration on horizontal ground movement...
	public float acceleration;
	public float decceleration;
	public float currentSpeed;
	
	private float moveX;
	
	public bool playerIsGrounded;
	
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Run(); //Look to "Run" Function
        Jump(); //Look to "Jump" Function
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
	}
	
	private void Jump()
	{ 
		if (Input.GetButtonDown("Jump") && playerIsGrounded)
		{
			rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
			
			playerIsGrounded = false;
		}
	}
	
	//Logic to check if player is grounded, be sure to tag the platforms as "Ground" in the editor
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
			playerIsGrounded = true;
		}
	}
}
