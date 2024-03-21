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
    public float minJumpForce = 2f; // Minimum jump force (when releasing spacebar early)
    public float jumpArcHeight = 5f; // Height of the jump arc
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Start jump charging when holding down spacebar and grounded
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && hasReleasedSpace)
        {
            Debug.Log("Spacebar pressed");
            jumpStartTime = Time.time;
            isChargingJump = true;
            hasReleasedSpace = false;
        }

        // Stop jump charging when releasing spacebar
        if (Input.GetKeyUp(KeyCode.Space))
        {
            float chargeTime = Time.time - jumpStartTime;
            if (chargeTime < jumpChargeDuration)
            {
                // Calculate jump force based on charge duration
                float chargeRatio = chargeTime / jumpChargeDuration;
                float currentJumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, chargeRatio);
                Jump(currentJumpForce);
            }
            else
            {
                // Jump with max force if the spacebar was held down for the full charge duration
                Jump(maxJumpForce);
            }

            isChargingJump = false;
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
