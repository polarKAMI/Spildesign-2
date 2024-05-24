using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderMovement : MonoBehaviour, IInteractable
{
    private Rigidbody2D rb;
    private PlayerMovement playerMovement;
    public bool isClimbing = false;
    private Transform ladderTransform;
    private float vertical;
    private float ladderTopY;
    public float ladderTopYOffSet = 0.7f;
    private float pushOffVelocityX = 0f;
    private bool isPushingOff = false;
    private Collider2D platformCollider;

    [SerializeField] private float climbSpeed = 3f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void Interact()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f); // Adjust radius as needed
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Ladder"))
            {
                if (isClimbing)
                {
                    StopClimbing();
                }
                else
                {
                    StartClimbing(collider.transform);
                }
                return; // Exit loop after interacting with one ladder
            }
        }
    }

    public void StartClimbing(Transform ladder)
    {
        playerMovement.DisableMovement();
        GlobalInputMapping.SetActiveInputMappings(GlobalInputMapping.climbInputMapping);
        isClimbing = true;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(0, 0);
        ladderTransform = ladder;
        SnapToLadder();

        // Get the platform collider
        platformCollider = ladder.Find("ladderPlatform")?.GetComponent<Collider2D>();
        if (platformCollider != null)
        {
            Debug.Log("Platform collider detected: " + platformCollider.name);
            platformCollider.isTrigger = true; // Set the platform collider to trigger
            ladderTopY = platformCollider.bounds.max.y;
        }
    }

    public void StopClimbing()
    {
        isClimbing = false;
        rb.gravityScale = 2f;
        playerMovement.EnableMovement();
        GlobalInputMapping.SetActiveInputMappings(GlobalInputMapping.inGameInputMapping);

        // Reset the push-off flag and velocity
        isPushingOff = false;
        pushOffVelocityX = 0f;

        // Reset the platform collider to non-trigger
        if (platformCollider != null)
        {
            platformCollider.isTrigger = false;
        }
    }

    public void PushOffLadder(int direction)
    {
        // Set flag to indicate the player is pushing off the ladder
        isPushingOff = true;
        // Determine the direction of the push
        pushOffVelocityX = direction * climbSpeed * 1.3f;

        // Apply the impulse force to push the player off the ladder
        rb.AddForce(new Vector2(pushOffVelocityX, 0f), ForceMode2D.Impulse);

        // Stop climbing
        StopClimbing();
    }

    private void SnapToLadder()
    {
        Vector3 targetPosition = new Vector3(ladderTransform.position.x, transform.position.y, transform.position.z);
        rb.MovePosition(targetPosition);
    }

    public void Climb(float input)
    {
        if (isClimbing)
        {
            vertical = input;
            Debug.Log("vertical is" + vertical);
        }
        else
        {
            vertical = 0f; // No input, stop climbing
        }
    }
    private void FixedUpdate()
    {
        if (isClimbing)
        {
            // Set vertical velocity based on climbing input
            rb.velocity = new Vector2(0, vertical * climbSpeed);

            if (transform.position.y >= ladderTopY + ladderTopYOffSet)
            {
                StopClimbing();
            }
        }
        else if (isPushingOff)
        {
            // Set velocity based on push-off direction
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }
    }
}
