using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool canJump = true; // Flag to track if the player can jump
    private bool isGrounded = false; // Flag to track if the player is grounded
    private bool isChargingJump = false; // Flag to track if the player is charging the jump
    private float jumpStartTime; // Time when jump charging started
    private float jumpChargeDuration = 1f; // Duration to charge the jump in seconds
    public float maxJumpForce = 12f; // Maximum jump force
    public float minJumpForce = 5f; // Minimum jump force (when releasing spacebar early)
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Check if the player can jump and is grounded
        if (canJump && isGrounded)
        {
            // Start charging the jump when spacebar is pressed
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartChargingJump();
            }

            // Release the jump when spacebar is released
            if (Input.GetKeyUp(KeyCode.Space))
            {
                ReleaseJump();
            }
        }

        // Update the jump charging
        UpdateChargingJump();
    }

    private void StartChargingJump()
    {
        isChargingJump = true;
        jumpStartTime = Time.time;
    }

    private void ReleaseJump()
    {
        isChargingJump = false;
        float chargeTime = Time.time - jumpStartTime;

        // Calculate jump force based on charge duration
        float chargeRatio = Mathf.Clamp01(chargeTime / jumpChargeDuration);
        float currentJumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, chargeRatio);

        // Jump with the calculated force
        Jump(currentJumpForce);
    }

    private void Jump(float jumpForce)
    {
        // Apply the jump force directly upward
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        canJump = false; // Set canJump to false after jumping
    }

    private void UpdateChargingJump()
    {
        // Reset charging if spacebar is not held down
        if (!Input.GetKey(KeyCode.Space))
        {
            isChargingJump = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player has collided with an object on the ground layer
        if (groundLayer == (groundLayer | (1 << collision.gameObject.layer)))
        {
            canJump = true; // Set canJump to true when grounded
        }
    }
}
