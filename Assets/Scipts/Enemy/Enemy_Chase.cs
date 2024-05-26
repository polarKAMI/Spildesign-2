using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Chase : MonoBehaviour
{
    public Transform player; // Reference to the player object
    public float moveSpeed = 5f; // Movement speed of the enemy
    public float detectionRange = 5f; // Range within which the enemy detects the player
   

    private Rigidbody2D rb;
  
    private bool isChasing = false;

    public float resumePatrollingDistance = 10f;
    public float Jumpforce;

    // Reference to Lygtemandenmovement script
    private EnemyMovement movementScript;
    private Enemy_Chase iScriptEnabled;

    private Damagescript damagescript;

    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
      
        movementScript = GetComponent<EnemyMovement>();
        iScriptEnabled = GetComponent<Enemy_Chase>();

      damagescript = GetComponentInChildren<Damagescript>();
        IgnorePlayerCollision();




    }

    void FixedUpdate()
    {
        if (!isChasing)
        {
            // Check for player within detection range
            if (player != null && Vector2.Distance(transform.position, player.position) <= detectionRange)
            {
               
                
                    isChasing = true;
                    // Disable Lygtemandenmovement script when chasing
                    movementScript.enabled = false;
                
            }
        }
        else
        {

            // Check if player is further away than resumePatrollingDistance
            if (player != null && Vector2.Distance(transform.position, player.position) > resumePatrollingDistance)
            {
                ResumePatrollingAfterDelay();
                return; // Stop chasing if player is further away
            }

            // Chase the player
            if (player != null)
            {
                MoveTowards(player.position);

                if (damagescript.PlayerHasBeenDamaged == true) // hvis player har taget skade s[ venter enemy i 2 sek
                {
                    StartCoroutine(HandleDamageStatus());
                }
            }
        }

        rb.freezeRotation = true;

        void MoveTowards(Vector2 targetPosition)
        {
            // Calculate the distance between the object and the target
            float distance = Vector2.Distance(transform.position, targetPosition);

            // Determine the side the enemy is coming from and set the offset
            Vector2 offset;
            if (transform.position.x < targetPosition.x)
            {
                // Enemy is coming from the left, stop on the left side of the player
                offset = new Vector2(-0.5f, 0.0f); // Adjust the values as needed
            }
            else
            {
                // Enemy is coming from the right, stop on the right side of the player
                offset = new Vector2(0.5f, 0.0f); // Adjust the values as needed
            }

            // Calculate the new target position with the offset
            Vector2 offsetTargetPosition = targetPosition + offset;

            // Change the scale based on the distance
            if (distance > 0)
            {
                // Adjust the scale based on the relative position of the target
                if (transform.position.x < targetPosition.x)
                {
                    // Facing right
                    transform.localScale = new Vector3(1.7f, transform.localScale.y, transform.localScale.z);
                }
                else
                {
                    // Facing left
                    transform.localScale = new Vector3(-1.7f, transform.localScale.y, transform.localScale.z);
                }
            }

            // Check if the enemy is on the ground (optional)
            if (IsGrounded())
            {
                // Apply movement force towards the target
                rb.velocity = (offsetTargetPosition - (Vector2)transform.position).normalized * moveSpeed;
            }
        }



    }


    IEnumerator HandleDamageStatus()
    {
        // Stop movement
        rb.velocity = Vector2.zero;
        isChasing = false;

        // Wait for 2 seconds
        yield return new WaitForSeconds(2f);

        // Reset damage status
        damagescript.ResetDamageStatus();

        // Resume chasing if the player is still within detection range
        if (player != null && Vector2.Distance(transform.position, player.position) <= detectionRange)
        {
            isChasing = true;
        }
        else
        {
            // Resume patrolling if the player is out of range
            movementScript.enabled = true;
        }
    }


    bool IsGrounded()
    {
        // Define a raycast origin slightly below the enemy's position
        Vector2 raycastOrigin = transform.position - new Vector3(0f, 1f, 0f);

        // Define the length of the raycast
        float raycastDistance = 0.5f;

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
        yield return new WaitForSeconds(2f); // Wait for 1 second
        isChasing = false;
        movementScript.enabled = true; // Enable Enemymovement script after the delay
    }

   


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("EnemyjumpTrigger"))
        {
            // Apply an impulse force when colliding with the circle collider
            float scaleX = transform.localScale.x;
            Vector2 boostDirection = (scaleX < 0) ? Vector2.left : Vector2.right;

            // Apply an impulse force upwards and in the boost direction
            rb.AddForce(Vector2.up * Jumpforce, ForceMode2D.Impulse);
            rb.AddForce(boostDirection * Jumpforce * 1f, ForceMode2D.Impulse); // Small boost in the boost direction

            Debug.Log("Has jumped");

            // Disable the script temporarily
            iScriptEnabled.enabled = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the enemy has landed on the ground
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // Re-enable the script
            iScriptEnabled.enabled = true;
        }
    }


    public void FaceTarget(Vector2 targetPosition)
    {
        // Calculate the direction towards the target
        Vector2 direction = targetPosition - (Vector2)transform.position;

        // Change the scale based on the direction
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(1.7f, transform.localScale.y, transform.localScale.z);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1.7f, transform.localScale.y, transform.localScale.z);
        }
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
}

