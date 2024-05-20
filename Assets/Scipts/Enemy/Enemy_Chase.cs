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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
      
        movementScript = GetComponent<EnemyMovement>();
        iScriptEnabled = GetComponent<Enemy_Chase>();
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
            }
        }

        rb.freezeRotation = true;

        void MoveTowards(Vector2 targetPosition)
        {
            // Calculate the direction towards the player
            // Calculate the direction towards the player
            Vector2 moveDirection = (targetPosition - (Vector2)transform.position).normalized;

            // Change the scale based on the direction
            if (moveDirection.x > 0)
            {
                // Facing right
                transform.localScale = new Vector3(1.7f, transform.localScale.y, transform.localScale.z);
            }
            else if (moveDirection.x < 0)
            {
                // Facing left
                transform.localScale = new Vector3(-1.7f, transform.localScale.y, transform.localScale.z);
            }

            // Check if the enemy is on the ground (optional)
            if (IsGrounded())
            {
                // Apply movement force in the direction of the player
                rb.velocity = moveDirection * moveSpeed;
            }
            // Check if the enemy is on the ground (optional)

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
        yield return new WaitForSeconds(1f); // Wait for 1 second
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
   
}

