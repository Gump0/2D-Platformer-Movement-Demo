using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isFacingRight;
    
    private Vector2 moveInput;
    
    // Enviorment Check Perameters
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.49f, 0.03f);
    
    // Enviorment Layers
    [SerializeField] private LayerMask groundLayer;
    
    // Run Stuff
    private float maxRunSpeed;
    private float timeLastGrounded;
    public float speed = 5f;
    public float runAccelValue;
    public float runDeccelValue;
    public float accelInAir;
    public float deccelInAir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
	
	private void Update()
	{
		timeLastGrounded -= Time.deltaTime;
		
		moveInput.x = Input.GetAxisRaw("Horizontal");
		
		// Ground Check
		
	}
    private void FixedUpdate()
    {
		Run();
    }
    
    private void Run()
    {
		float targetSpeed = moveInput.x * maxRunSpeed;
		
		float accelRate;
		
		// Gets an acceleration value based on if we are accelerating (includes turning) 
		// or trying to decelerate (stop). As well as applying a multiplier if we're airborne
		
		if(timeLastGrounded > 0)
		{
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelValue : runDeccelValue;
		}
		else 
		{
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelValue * accelInAir : runDeccelValue * deccelInAir;
		}
		
		// Prevents the player from surpassing their top speed
		// However we shall not take away the players momentum upon change of direction (yet)
		// ...
	}
	private void Flip()
	{
		
	}
	
}
