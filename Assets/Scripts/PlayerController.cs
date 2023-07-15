using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	Rigidbody2D rigidBody;
	
	public float moveSpeed;
	public float jumpForce;
	
	public float acceleration;
	public float currentSpeed;
	
	//
	public float movementDirectionalInput;
	
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
		float moveX = Input.GetAxisRaw("Horizontal");
		currentSpeed = Mathf.MoveTowards(currentSpeed, moveX * moveSpeed, acceleration * Time.deltaTime);
		rigidBody.velocity = new Vector2(moveX * currentSpeed, rigidBody.velocity.y);
		
	}
	
	private void Jump()
	{ 
		if (Input.GetButtonDown("Jump"))
		{
			rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
		}
	}
}
