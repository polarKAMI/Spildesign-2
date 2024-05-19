using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lygtemanden : MonoBehaviour
{
    public Transform player; // Reference to the player object
    public float moveSpeed = 5f; // Movement speed of the enemy
    public float detectionRange = 5f; // Range within which the enemy detects the player
    public float stopRange = 2f;
    public LayerMask obstacleLayer; // Layer mask for the obstacles (e.g., bush)
    public Busk busk;

    private Rigidbody2D rb;
    private Vector3 initialPosition;
    private bool isChasing = false;
    private bool isStopped = false;

    public float resumePatrollingDistance = 10f;

    private Coroutine flipCoroutine;
    private Vector2 lastPlayerPosition;

    // Reference to Lygtemandenmovement script
    private Lygtemandenmovement movementScript;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
        movementScript = GetComponent<Lygtemandenmovement>();
        IgnorePlayerCollision();
    }

    void FixedUpdate()
    {
        if (!isChasing)
        {
            // Check for player within detection range
            if (player != null && Vector2.Distance(transform.position, player.position) <= detectionRange)
            {
                // Check if player is not under a bush
                Collider2D[] obstacles = Physics2D.OverlapCircleAll(player.position, 0.5f, obstacleLayer);
                if (obstacles.Length == 0)
                {
                    isChasing = true;
                    // Disable Lygtemandenmovement script when chasing
                    movementScript.enabled = false;
                }
            }
        }
        else
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= stopRange)
            {
                // Stop moving and follow player's movement
                if (!isStopped)
                {

                    StartCoroutine(StopMovementCoroutine());
                    isStopped = true;
                    lastPlayerPosition = player.position;
                    if (flipCoroutine != null)
                    {
                        StopCoroutine(flipCoroutine);
                    }
                    flipCoroutine = StartCoroutine(FlipTowardsPlayerDelayed());
                }




            }
            else
            {
                if (isStopped)
                {

                    StartCoroutine(ResumeMovementAfterDelay());
                }

                // Check if player is further away than resumePatrollingDistance
                if (distanceToPlayer > resumePatrollingDistance)
                {
                    ResumePatrollingAfterDelay();
                    return; // Stop chasing if player is further away
                }

                // Chase the player
                if (player != null && !isStopped)
                {
                    MoveTowards(player.position);
                }
            }
        }

        rb.freezeRotation = true;
    }

    void MoveTowards(Vector2 targetPosition)
    {
        Debug.Log("Is chasing");
        // Calculate the direction towards the player
        Vector2 moveDirection = (targetPosition - (Vector2)transform.position).normalized;

        // Change the scale based on the direction
        if (moveDirection.x > 0)
        {
            // Facing right
            transform.localScale = new Vector3(-3.5f, transform.localScale.y, transform.localScale.z);
        }
        else if (moveDirection.x < 0)
        {
            // Facing left
            transform.localScale = new Vector3(3.5f, transform.localScale.y, transform.localScale.z);
        }

        // Check if the enemy is on the ground (optional)
        if (IsGrounded())
        {
            // Apply movement force in the direction of the player
            rb.velocity = moveDirection * moveSpeed;
        }
    }

    IEnumerator FlipTowardsPlayerDelayed()
    {
        while (isStopped)
        {
            yield return new WaitForSeconds(2f);
            Debug.Log("Has flipped when stopped after 4 seconds");

            // Check if player has moved since the last check
            if ((player.position - (Vector3)lastPlayerPosition).sqrMagnitude > 0.01f)
            {
                // Update the last known position
                lastPlayerPosition = player.position;

                // Calculate the direction towards the player
                Vector2 moveDirection = (player.position - transform.position).normalized;

                // Change the scale based on the direction
                if (moveDirection.x > 0)
                {
                    transform.localScale = new Vector3(-3.5f, transform.localScale.y, transform.localScale.z);
                }
                else if (moveDirection.x < 0)
                {
                    transform.localScale = new Vector3(3.5f, transform.localScale.y, transform.localScale.z);
                }
            }
        }
    }

    bool IsGrounded()
    {
        // Define a raycast origin slightly below the enemy's position
        Vector2 raycastOrigin = transform.position - new Vector3(0f, 0.1f, 0f);

        // Define the length of the raycast
        float raycastDistance = 2f;

        // Create a layer mask to only detect collisions with the "Ground" layer
        LayerMask groundLayerMask = LayerMask.GetMask("Ground");

        // Cast a raycast downwards to check for ground collision with the "Ground" layer
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, raycastDistance, groundLayerMask);

        // Check if the raycast hit something (ground)
        if (hit.collider != null)
        {
            // Optionally, you can debug draw the raycast for visualization
            Debug.DrawRay(raycastOrigin, Vector2.down * raycastDistance, Color.green);

            Debug.Log("Has hit ground");

            // Return true indicating that the enemy is grounded
            return true;
        }
        else
        {
            // Optionally, you can debug draw the raycast for visualization
            Debug.DrawRay(raycastOrigin, Vector2.down * raycastDistance, Color.red);

            // Return false indicating that the enemy is not grounded
            return false;
        }
    }

    // Add method to enable Enemymovement script after a delay
    public void ResumePatrollingAfterDelay()
    {
        StartCoroutine(ResumePatrollingCoroutine());
    }

    IEnumerator ResumePatrollingCoroutine()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second
        isChasing = false;
        movementScript.enabled = true; // Enable Enemymovement script after the delay
        Debug.Log("Is patrolling again");
    }

    void IgnorePlayerCollision()
    {
        Collider2D myCollider = GetComponent<Collider2D>();

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            Collider2D playerCollider = player.GetComponent<Collider2D>();
            if (playerCollider != null && myCollider != null)
            {
                Physics2D.IgnoreCollision(myCollider, playerCollider);
            }
        }
    }

    IEnumerator StopMovementCoroutine()
    {
        rb.velocity = Vector2.zero;
        Debug.Log("Is stopped");
        yield return null; // Ensure movement is stopped immediately
    }

    IEnumerator ResumeMovementAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        isStopped = false;
        Debug.Log("Ischasing again");
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player entered the trigger of a bush
        if (other.gameObject.CompareTag("Bush") && busk.isin)
        {

            StartCoroutine(HandleBushCollision());
           
        }
    }


   

    IEnumerator FlipGameObjectCoroutine()
    {

        
        // Step 1: Stop all movement
        rb.velocity = Vector2.zero;

        // Step 2: Move a little bit in the current direction
        Vector2 currentDirection = rb.velocity.normalized;
        rb.MovePosition(rb.position + currentDirection * 1f); // Move a little bit

        // Wait for a short duration before flipping
        yield return new WaitForSeconds(1f);

        // Step 3: Perform the flips
        for (int i = 0; i < 3; i++)
        {
            // Flip the x scale
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

            // Wait for 1 second between each flip
            yield return new WaitForSeconds(3f);
        }
    }

    IEnumerator HandleBushCollision()
    {
        // Perform the flipping first
        yield return FlipGameObjectCoroutine();

        // After flipping, set isChasing to false and enable the movement script
        isChasing = false;
        movementScript.enabled = true;

        StopCoroutine(FlipGameObjectCoroutine());
        StopCoroutine(HandleBushCollision());
    }
}