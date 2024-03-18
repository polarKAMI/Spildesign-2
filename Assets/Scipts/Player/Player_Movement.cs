using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
<<<<<<< HEAD
    private bool isFacingRight = true;
    private float horizontal;
    private float currentSpeed = 0f; // Current movement speed
=======
    public bool isFacingRight = true;
    private float horizontal;
    public float currentSpeed = 0f; // Current movement speed
    public float baseSpeed = 2f; // Base movement speed
>>>>>>> main
    public float maxSpeed = 5f; // Maximum movement speed
    public float acceleration = 2f; // Acceleration rate
    public float deceleration = 4f; // Deceleration rate
    public float sprintSpeedMultiplier = 2f; // Speed multiplier when sprinting
    [SerializeField] private Rigidbody2D rb;

<<<<<<< HEAD
=======
    private void Start()
    {
        // Set the initial currentSpeed to the baseSpeed
        currentSpeed = baseSpeed;
    }

>>>>>>> main
    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        // Sprinting
        float sprintMultiplier = Input.GetKey(KeyCode.LeftShift) ? sprintSpeedMultiplier : 1f;

        // Acceleration
        if (horizontal != 0f)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed * sprintMultiplier, acceleration * Time.deltaTime);
        }
        else
        {
<<<<<<< HEAD
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
=======
            currentSpeed = Mathf.MoveTowards(currentSpeed, baseSpeed, deceleration * Time.deltaTime);
>>>>>>> main
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

<<<<<<< HEAD
            // Reset current speed to zero when flipping
            currentSpeed = 0f;
=======
            // Reset current speed to baseSpeed when flipping
            currentSpeed = baseSpeed;
>>>>>>> main
        }
    }
}