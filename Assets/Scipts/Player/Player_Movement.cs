using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool isFacingRight = true;
    private float horizontal;
    public float currentSpeed = 0f; // Current movement speed
    public float baseSpeed = 2f; // Base movement speed
    public float maxSpeed = 5f; // Maximum movement speed
    public float acceleration = 2f; // Acceleration rate
    public float deceleration = 4f; // Deceleration rate
    public float sprintSpeedMultiplier = 2f; // Speed multiplier when sprinting
    public float sprintCostPerSecond = 20f; // Stamina cost per second of sprinting
    public float staminaRegenPerSecond = 5f; // Stamina regeneration per second
    public float maxStamina = 100f; // Maximum stamina
    public float currentStamina; // Current stamina
    [SerializeField] private Rigidbody2D rb;

    private void Start()
    {
        // Set the initial currentSpeed to the baseSpeed
        currentSpeed = baseSpeed;
        currentStamina = maxStamina; // Set initial stamina to max
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        // Sprinting
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && currentStamina > 0f;
        float sprintMultiplier = isSprinting ? sprintSpeedMultiplier : 1f;

        // Calculate movement speed based on sprinting and stamina
        float targetSpeed = isSprinting ? maxSpeed * sprintMultiplier : baseSpeed;
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);

        // Decrease stamina when sprinting
        if (isSprinting)
        {
            currentStamina -= sprintCostPerSecond * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina); // Clamp stamina between 0 and maxStamina
        }
        else
        {
            // Regenerate stamina when not sprinting
            currentStamina += staminaRegenPerSecond * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina); // Clamp stamina between 0 and maxStamina
        }

        Flip();
    }

    private void FixedUpdate()
    {
        // Apply horizontal movement
        rb.velocity = new Vector2(horizontal * currentSpeed, rb.velocity.y);
    }

    private void Flip()
    {
        if ((horizontal > 0 && !isFacingRight) || (horizontal < 0 && isFacingRight))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;

            // Reset current speed to baseSpeed when flipping
            currentSpeed = baseSpeed;
        }
    }
}