using UnityEngine;

public class Jump : MonoBehaviour
{
    public PlayerMovement playerMovement;
    private bool isChargingJump = false; // Flag to track if the player is charging the jump
    private float jumpStartTime; // Time when jump charging started
    public float maxChargeTime = 1.0f; // Maximum time allowed for charging the jump

    private bool canJump = true; // Flag to track if the player can jump
    private bool isGrounded = false; // Flag to track if the player is grounded
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
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
        if (canJump && isChargingJump)
        {
            isChargingJump = false;
            float chargeTime = Time.time - jumpStartTime;

            // Check if charge time exceeds the maximum allowed
            if (chargeTime > maxChargeTime)
            {
                chargeTime = maxChargeTime; // Cap charge time to maximum
            }

            // Calculate trajectory
            Vector2 trajectory = CalculateTrajectory(chargeTime);

            // Apply the trajectory (example: move the player to the calculated position)
            transform.position = trajectory;

            // Reset jump flags and movement
            canJump = false;
            playerMovement.DisableMovement();
        }
    }

    private Vector2 CalculateTrajectory(float t)
    {
        // Example coefficients for a quadratic trajectory (adjust as needed)
        float v = 5.0f; // Horizontal speed
        float a = 1.0f; // Coefficient for t^2
        float b = 0.0f; // Coefficient for t
        float c = 0.0f; // Constant term

        // Calculate trajectory using parametric equations
        float x = v * t; // Horizontal position at time t
        float y = a * t * t + b * t + c; // Vertical position (height) at time t

        return new Vector2(x, y);
    }

    private void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player has collided with an object on the ground layer
        if (groundLayer == (groundLayer | (1 << collision.gameObject.layer)))
        {
            if (!playerMovement.movementEnabled)
            {
                playerMovement.EnableMovement();
            }
            canJump = true; // Set canJump to true when grounded
        }
    }
}