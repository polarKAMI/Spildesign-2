using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private Rigidbody2D rb;
    public PlayerMovement playerMovement;
    private LadderMovement ladderMovement;
    private bool canJump = true; // Flag to track if the player can jump
    public bool isGrounded = false; // Flag to track if the player is grounded
    private bool isChargingJump = false; // Flag to track if the player is charging the jump
    public bool isJumping = false; // Flag to track if the player is in the air
    public bool isSliding = false; // Flag to track if the player is sliding
    public bool isSideStep = false;
    private float jumpStartTime; // Time when jump charging started
    private float jumpTime; // Time spent in the air
    private float jumpChargeDuration = 1f; // Duration to charge the jump in seconds
    public float maxJumpForce = 12f; // Maximum jump force
    public float minJumpForce = 5f; // Minimum jump force (when releasing spacebar early)
    public float sideStepSpeed = 20f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        ladderMovement = GetComponent<LadderMovement>(); 
    }

    public void StartChargingJump()
    {
        if (canJump && isGrounded)
        {
            isChargingJump = true;
            jumpStartTime = Time.time;
        }
    }

    public void ReleaseJump()
    {
        float chargeTime = Time.time - jumpStartTime;

        if (canJump && isChargingJump && isGrounded && chargeTime >= 0.15f)
        {
            isChargingJump = false;   
            // Calculate jump force based on charge duration
            float chargeRatio = Mathf.Clamp01(chargeTime / jumpChargeDuration);
            float currentJumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, chargeRatio);

            // Jump with the calculated force
            Jump(currentJumpForce);
        }
        else if (canJump && isChargingJump && isGrounded && chargeTime <= 0.15f)
        {
            isChargingJump = false;
            SideStep(sideStepSpeed);
        }
        else
        {
            isChargingJump = false;
        }
    }

    private void Jump(float jumpForce)
    {
        playerMovement.DisableMovement();
        // Apply the jump force in both x and y directions
        if (playerMovement.isFacingRight)
        {
            rb.velocity = new Vector2(jumpForce, jumpForce / .7f);
        }
        else
        {
            rb.velocity = new Vector2(-jumpForce, jumpForce / .7f);
        }
        canJump = false; // Set canJump to false after jumping
        isJumping = true; // Set isJumping to true after jumping
    }

    private void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 2f, groundLayer);

        // Update jump time if the player is in the air
        if (isJumping)
        {
            jumpTime = Time.time - jumpStartTime;
        }

        // Enable movement after sliding is complete
        if (!ladderMovement.isClimbing)
        {
            if (isSliding && Mathf.Abs(rb.velocity.x) < 0.01f)
            {
                rb.drag = 0f;
                isSliding = false;
                playerMovement.EnableMovement();
            }
            else if (isSideStep && Mathf.Abs(rb.velocity.x) < 0.01f)
            {
                rb.drag = 0f;
                isSideStep = false;
                playerMovement.EnableMovement();
            }
        }
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player has collided with an object on the ground layer
        if(GlobalInputMapping.activeInputMappings == GlobalInputMapping.inGameInputMapping)
        {
            if (groundLayer == (groundLayer | (1 << collision.gameObject.layer)))
            {
                // Check if the jump time is above 0.9 and slide if true
                if (isJumping && jumpTime > 1.5f)
                {
                    isSliding = true;
                    Slide(jumpTime);
                }
                else
                {
                    playerMovement.EnableMovement();
                }

                canJump = true; // Set canJump to true when grounded
                isJumping = false; // Reset isJumping when grounded
            }
        }
        
    }

    private void Slide(float jumpTime)
    {
        rb.drag = 10f;
        // Slide the player using forces
        float slideForce = 1f * jumpTime; // Adjust this value as needed
        Vector2 slideDirection = playerMovement.isFacingRight ? Vector2.right : Vector2.left;
        rb.AddForce(slideDirection * slideForce, ForceMode2D.Impulse);
        Debug.Log("slide");
    }

    private void SideStep(float sideStepSpeed)
    {
        playerMovement.DisableMovement();
        isSideStep = true;
        rb.drag = 20f;
        Vector2 sideStepDirection = playerMovement.isFacingRight ? Vector2.right : Vector2.left;
        rb.AddForce(sideStepDirection * sideStepSpeed, ForceMode2D.Impulse);
    }
}