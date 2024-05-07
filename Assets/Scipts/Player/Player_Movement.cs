using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool isFacingRight = true;
    private float horizontal;
    public float currentSpeed = 0f; // Current movement speed
    public float baseSpeed = 1.5f; // Base movement speed
    public float maxSpeed = 3f; // Maximum movement speed
    public float acceleration = 3f; // Acceleration rate
    public float deceleration = 4f; // Deceleration rate
    public float sprintSpeedMultiplier = 1.3f; // Speed multiplier when sprinting
    public bool isSprinting = false;
    [SerializeField] private Rigidbody2D rb;
    public InventoryUIManager inventoryUIManager;

    public bool movementEnabled = true; // Flag to track if movement is enabled

    private void Start()
    {
        // Set the initial currentSpeed to the baseSpeed
        currentSpeed = baseSpeed;   
    }

    public void Move(float input)
    {
        if (!movementEnabled)
            return; // Exit Move() if movement is disabled

        // Assign input value to horizontal
        horizontal = input;

        // Sprinting
        float sprintMultiplier = Input.GetKey(GlobalInputMapping.activeInputMappings["Sprint"]) ? sprintSpeedMultiplier : 1f;

        // Acceleration
        if(horizontal != 0f && isSprinting)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed * sprintMultiplier, acceleration * Time.deltaTime);
        }
        if (horizontal != 0f)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = 0f;
        }

        Flip();
        
    }


    private void FixedUpdate()
    {
        if (movementEnabled) {
            rb.velocity = new Vector2(horizontal * currentSpeed, rb.velocity.y);
        }   
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

    public void EnableMovement()
    {
        movementEnabled = true;
    }

    public void DisableMovement()
    {
        movementEnabled = false;
        Move(0f);
        currentSpeed = 0f;
        horizontal = 0f;
    }

}