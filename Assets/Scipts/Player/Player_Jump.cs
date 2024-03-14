using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private Rigidbody2D rb;
    private float jumpStartTime; // Time when jump charging started
    private float jumpChargeDuration = 1f; // Duration to charge the jump in seconds
    private bool isChargingJump = false; // Flag to track if jump is currently being charged
    private bool hasReleasedSpace = true; // Flag to track if spacebar has been released since the last jump
    public float maxJumpForce = 5f; // Maximum jump force
    public float jumpArcHeight = 5f; // Height of the jump arc
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Jump charging
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && hasReleasedSpace)
        {
            Debug.Log("Spacebar pressed");
            jumpStartTime = Time.time;
            isChargingJump = true;
            hasReleasedSpace = false;
        }

        // Calculate current jump force based on charge duration
        if (isChargingJump)
        {
            float chargeTime = Time.time - jumpStartTime;
            Debug.Log("Charge time: " + chargeTime);
            float chargeRatio = Mathf.Clamp01(chargeTime / jumpChargeDuration);
            float currentJumpForce = maxJumpForce * chargeRatio;

            if (chargeTime >= jumpChargeDuration)
            {
                Jump(currentJumpForce);
                isChargingJump = false;
                hasReleasedSpace = true; // Set flag to true after jump
            }
        }

        // Reset hasReleasedSpace flag when grounded
        if (IsGrounded())
        {
            hasReleasedSpace = true;
        }
    }

    private void Jump(float jumpForce)
    {
        // Apply the jump force directly upward
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private bool IsGrounded()
    {
        bool grounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        Debug.Log("Grounded: " + grounded);
        return grounded;
    }
}
