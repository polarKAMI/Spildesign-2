using UnityEngine;
using System.Collections;

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
    public bool isFalling = false;
    private float jumpStartTime; // Time when jump charging started
    private float jumpTime;
    private float fallTime;
    private float jumpChargeDuration = .35f; // Duration to charge the jump in seconds
    public float maxJumpForce = 10f; // Maximum jump force
    public float minJumpForce = 7f; // Minimum jump force (when releasing spacebar early)
    public float sideStepSpeed = 25f;
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    public Animator animator;
    private bool collideonce = false;

    
    public GameObject Jumpaudio;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        ladderMovement = GetComponent<LadderMovement>();
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        
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

        if (canJump && isChargingJump && isGrounded)
        {
            isChargingJump = false;
            // Calculate jump force based on charge duration
            float chargeRatio = Mathf.Clamp01(chargeTime / jumpChargeDuration);
            float currentJumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, chargeRatio);

            // Jump with the calculated force
            Jump(currentJumpForce);
        }
    }

    private void Jump(float jumpForce)
    {
        playerMovement.DisableMovement();

        Instantiate(Jumpaudio);
        // Apply the jump force in both x and y directions
        if (playerMovement.isFacingRight)
        {
            rb.velocity = new Vector2(jumpForce, jumpForce / .7f);
            collideonce = false;
        }
        else
        {
            rb.velocity = new Vector2(-jumpForce, jumpForce / .7f);
            collideonce = false;
        }
        canJump = false; // Set canJump to false after jumping
        isJumping = true; // Set isJumping to true after jumping
        animator.SetBool("IsJumping", true); // Trigger jump animation
    }

    private void Update()
    {
        // Check if the player is grounded only if they are not climbing
        if (!ladderMovement.isClimbing)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, .1f, groundLayer);
        }

        // Update jump time if the player is in the air
        if (isJumping)
        {
            jumpTime = Time.time - jumpStartTime;
        }

        if (!ladderMovement.isClimbing)
        {
            if (!isGrounded)
            {
                if (isJumping && jumpTime > 2.8f)
                {
                    isJumping = false;
                    isFalling = true;
                    fallTime += jumpTime; // Add the time spent jumping to fallTime
                    Debug.Log("free fallin");
                    collideonce = false;
                }
                else if (!isJumping)
                {
                    isFalling = true;
                    fallTime += jumpTime; // Add the time spent jumping to fallTime
                    Debug.Log("free fallin");
                    collideonce = false;
                }
            }
        }

        if (isGrounded)
        {
            if (isFalling)
            {
                isFalling = false;
                animator.SetBool("IsLanded", true);
                animator.SetBool("IsFalling", false);
            }
        }
        else if (isFalling)
        {
            animator.SetBool("IsFalling", true);
        }

        if (isSliding)
        {
            // Allow a short delay before checking velocity
            StartCoroutine(DelayedSlideCheck());
        }

        // Enable movement after sliding is complete
        if (!ladderMovement.isClimbing)
        {
            if (isSliding && Mathf.Abs(rb.velocity.x) < 0.01f)
            {
                StopSlide();
                playerMovement.EnableMovement();
            }
        }
    }

    public void StopSlide()
    {
        rb.drag = 0f;
        isSliding = false;
        cameraFollow.StopShake();  
    }

    private IEnumerator DelayedSlideCheck()
    {
        // Wait for a short delay before checking velocity
        yield return new WaitForSeconds(0.1f); // Adjust the delay time as needed

        // Check if the player's velocity is close to zero
        if (Mathf.Abs(rb.velocity.x) < 0.01f)
        {
            // Stop sliding and reset other parameters
            rb.drag = 0f;
            isSliding = false;
            cameraFollow.StopShake();
            playerMovement.EnableMovement();
            Debug.Log("CUCKED BITCHBOY");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player has collided with an object on the ground layer
        if (GlobalInputMapping.activeInputMappings == GlobalInputMapping.inGameInputMapping)
        {
            if (!collideonce && groundLayer == (groundLayer | (1 << collision.gameObject.layer)))
            {
                collideonce = true;
                // Check if the jump time is above 0.9 and slide if true
                if (isJumping && jumpTime > .5f && Mathf.Abs(rb.velocity.x) > 0.1f)
                {
                    isSliding = true;
                    playerMovement.DisableMovement();
                    Slide(jumpTime);
                    cameraFollow.StartShake();
                    Debug.Log("SLIDEMENISTER");
                }
                else if (isFalling && fallTime > 2f)
                {
                    cameraFollow.StartViolentShake();
                    playerMovement.EnableMovement();
                    playerMovement.ApplySlow(2f, 0.01f);
                    Invoke("StopShake", 0.3f);
                    isFalling = false;
                    animator.SetBool("IsLanded", true);
                }
                else if (isFalling)
                {
                    animator.SetBool("IsLanded", true);
                }
                else
                {
                    playerMovement.EnableMovement();
                }

                Debug.Log("SmokeWeedErryday");
                canJump = true; // Set canJump to true when grounded
                isJumping = false; // Reset isJumping when grounded
                isFalling = false;
                fallTime = 0f;
                animator.SetBool("IsJumping", false);// Reset jump animation
                animator.SetBool("IsFalling", false);
            }
        }
    }

    public void StopShake()
    {
        cameraFollow.StopShake();
    }

    private void Slide(float jumpTime)
    {
        rb.drag = 4f;
        // Slide the player using forces
        float slideForce = 2 * jumpTime; // Adjust this value as needed
        Vector2 slideDirection = playerMovement.isFacingRight ? Vector2.right : Vector2.left;
        rb.AddForce(slideDirection * slideForce, ForceMode2D.Impulse);
        Debug.Log("slide");
    }
}
