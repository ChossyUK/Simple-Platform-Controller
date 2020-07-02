using UnityEngine;

public class SimplePlatformController2D : MonoBehaviour
{
    [Header("Player Setup")]
    // Player movement speed
    [SerializeField] float movementSpeed;
    // Set how high the player can jump
    [SerializeField] float jumpForce;
    // The number of jumps the player can do
    [SerializeField] int numberOfJumps = 1;
    // Jump fall gravity multiplier
    [SerializeField] float jumpFallMultiplier;
    // Gravity multiplier
    [SerializeField] float gravityMultiplier;


    [Header("Collision Checks")]
    // Ground checker radius
    [SerializeField] float groundCheckerRadius;
    // Ground checker
    [SerializeField] Transform groundChecker;
    // Ground layer
    [SerializeField] LayerMask groundLayer;


    // Float for movement direction
    float movementDirection;
    // Bool to check if the player is grounded
    bool isGrounded;
    // The players rigidbody 
    Rigidbody2D rb;
    // Bool to set the players facing direction.
    bool facingRight = true;
    // Int to reset the number of jumps
    int extraJumps;

    void Start()
    {
        // get the rigidbody the script is attached to
        rb = GetComponent<Rigidbody2D>();
        // Set the gravity scale
        rb.gravityScale = gravityMultiplier;
        // Set the amount of jumpa available
        extraJumps = numberOfJumps;
    }

    void Update()
    {
        // Check for input
        CheckInput();
        CheckJump();
    }

    void FixedUpdate()
    {
        // Check the physics status
        CheckPhysics();
    }

    void CheckPhysics()
    {
        // Check if the player is touching the ground
        isGrounded = Physics2D.OverlapCircle(groundChecker.position, groundCheckerRadius, groundLayer);
        // Move the player
        rb.velocity = new Vector2(movementDirection * movementSpeed, rb.velocity.y);
    }

    void CheckInput()
    {
        // get the input from the horizontal axis
        movementDirection = Input.GetAxisRaw("Horizontal");

        // Flip the player according to direction
        if (!facingRight && movementDirection > 0.1f || facingRight && movementDirection < -0.1f)
        {
            Flip();
        }
    }

    void CheckJump()
    {
        if (isGrounded)
        {
            // Reset the number of jumps allowed
            extraJumps = numberOfJumps;
        }

        if (rb.velocity.y < 0)
        {
            // Increase the gravity when falling to shorten the jump arc
            rb.velocity += Vector2.up * Physics2D.gravity.y * jumpFallMultiplier * Time.deltaTime;
        }

        // If the fire button is pressed and we have jumps available jump
        if (Input.GetButtonDown("Fire1") && extraJumps > 1)
        {
            // Apply an upward force to make the player jump
            rb.velocity = Vector2.up * jumpForce;
            // Subtract 1 jump from our jump value
            extraJumps--;
        }
        // The function for a single jump only
        else if (Input.GetButtonDown("Fire1") && isGrounded && extraJumps == 1)
        {
            // Apply an upward force to make the player jump
            rb.velocity = Vector2.up * jumpForce;
        }
    }

    void Flip()
    {
        // Flip between left and right
        facingRight = !facingRight;
        // Flip the player sprite 
        transform.Rotate(0f, 180f, 0f);     
    }

    void OnDrawGizmos()
    {
        // Draw a sphere so we can see the ground checker
        Gizmos.DrawSphere(groundChecker.position, groundCheckerRadius);
    }
}
