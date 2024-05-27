using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lygtemanden : MonoBehaviour
{
    public Transform player; // Reference to the player object
    public float moveSpeed = 5f; // Movement speed of the enemy
    public float detectionRange = 5f; // Range within which the enemy detects the player
    public float stopRange = 2f; // Range within which the enemy stops chasing
    public LayerMask obstacleLayer; // Layer mask for the obstacles (e.g., bush)

    public PlayerMovement Playermovement_script;


    private Rigidbody2D rb; // Rigidbody2D component for physics interactions
    private Vector3 initialPosition; // Initial position of the enemy
    public bool isChasing = false; // Is the enemy chasing the player?
    public bool isStopped = false; // Is the enemy stopped?

    public float resumePatrollingDistance = 10f; // Distance to resume patrolling after chasing

    private Coroutine flipCoroutine; // Coroutine for flipping
    private Vector2 lastPlayerPosition; // Last known player position

    private bool disabled = false;

    private Lygtemandenmovement movementScript; // Reference to the movement script

    public bool isHidingHandled = false; // Flag to check if hiding is handled
    public Animator animator;

    public GameObject Awakesound;

    private bool hasPlayedAwakeSound = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
        movementScript = GetComponent<Lygtemandenmovement>();
        IgnorePlayerCollision();
        DisableMovement();
        Playermovement_script = player.GetComponent<PlayerMovement>();
    }

    void DisableMovement()
    {
        movementScript.enabled = false;
        disabled = true;
        Debug.Log("Er jeg i kørestol ven");
    }

    void FixedUpdate()
    {
        if (disabled == true)
        { 
          if (!isChasing)
        {
            // Check for player within detection range
            if (player != null && IsPlayerInFront())
            {


                
                
                    if (Playermovement_script.PlayerHidden == true)
                    {
                        // keep patrolling??
                        Collider2D[] obstacles = Physics2D.OverlapCircleAll(player.position, 0.5f, obstacleLayer);
                    Debug.Log(" Lygte kan ikke se dig");
                    }
                    else
                    {
                        isChasing = true;
                        if (!hasPlayedAwakeSound)
                        {
                            Instantiate(Awakesound);
                            hasPlayedAwakeSound = true;
                        }
                        // Disable Lygtemandenmovement script when chasing
                        movementScript.enabled = false;
                    animator.SetBool("IsSpawned", true);
                    animator.SetBool("HasCaught", true);
                    Debug.Log("Lygte chaser");
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

                    StartCoroutine(ResumeMovementAfterDelay()); // might be this
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
    }

    bool IsPlayerInFront()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;

        // Determine the facing direction based on the local scale's x value
        Vector2 facingDirection = transform.localScale.x > 0 ? Vector2.left : Vector2.right;

        float dotProduct = Vector2.Dot(facingDirection, directionToPlayer);

        // Check if the player is within detection range and in front of the Lygtemanden
        return dotProduct > 0 && Vector2.Distance(transform.position, player.position) <= detectionRange;
    }



    void MoveTowards(Vector2 targetPosition) // This handles the chase
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

    IEnumerator FlipTowardsPlayerDelayed() //  flips the enemy towards the player if they have moved and if the lygtemanden is stooped.
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

    
    public void ResumePatrollingAfterDelay() // starts the coroutine to resume patrolling.
    {
        StartCoroutine(ResumePatrollingCoroutine());
    }

    IEnumerator ResumePatrollingCoroutine() // handles the logic for resuming patrolling after a delay.
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second
        isChasing = false;
        movementScript.enabled = true; // Enable Enemymovement script after the delay
        Debug.Log("Is patrolling again");
        
    }

    void IgnorePlayerCollision() // ignores collisions with player objects.
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

    IEnumerator StopMovementCoroutine() // stops the enemy's movement.
    {
        rb.velocity = Vector2.zero;
        Debug.Log("Is stopped");
        yield return null; // Ensure movement is stopped immediately
    }

    IEnumerator ResumeMovementAfterDelay() //  resumes the enemy's movement after a delay.
    {
        yield return new WaitForSeconds(2f);
        isStopped = false;
        Debug.Log("Ischasing again");
    }



    private Coroutine flipCoroutine3; // Reference to the flip coroutine

    IEnumerator FlipGameObjectCoroutine() // flips the enemy's direction after chase.
    {


        // Step 1: Stop all movement
        rb.velocity = Vector2.zero;
        Debug.Log("Is trying to find player");

        StartCoroutine(Getpatrollingbitch());

        // Step 2: Perform the flips
        for (int i = 0; i < 2; i++)
        {
            // Flip the x scale
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            Debug.Log("Flipped " + (i + 1) + " time(s)");
            // Wait for 3 seconds between each flip
            yield return new WaitForSeconds(3f);
        }

        // Reset the flipCoroutine reference to null after finishing
        flipCoroutine3 = null;
        Debug.Log("Flip coroutine finished");

        

    }

    public IEnumerator HandleBushCollision() // handles the logic when the enemy collides with a bush, including stopping chasing and resuming patrolling.
    {
        // Perform the flipping first
        if (flipCoroutine3 == null)
        {
            flipCoroutine3 = StartCoroutine(FlipGameObjectCoroutine());
            Debug.Log("Flip has started");
            // Wait for the flipping to finish
            yield return flipCoroutine3;
        }
    }

    IEnumerator Getpatrollingbitch()
    {
        yield return new WaitForSeconds(7f);
        Debug.Log("Started patrolling after 7 seconds");
        // After flipping, set isChasing to false and enable the movement script
        isChasing = false;
        isStopped = false;
        movementScript.enabled = true;
        isHidingHandled = false;

    }

    public void FaceTarget(Vector2 targetPosition)
    {
        Vector2 direction = targetPosition - (Vector2)transform.position;
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(-3.5f, transform.localScale.y, transform.localScale.z);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(3.5f, transform.localScale.y, transform.localScale.z);
        }
    }


}