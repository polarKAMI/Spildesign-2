using System.Collections;
using UnityEngine;

public class Lygtemanden : MonoBehaviour
{
    public Transform player; // Reference to the player object
    public float moveSpeed = 5f; // Movement speed of the enemy
    public float detectionRange = 5f; // Range within which the enemy detects the player
    public LayerMask obstacleLayer; // Layer mask for the obstacles (e.g., bush)
    public Busk busk;

    private Rigidbody2D rb;
    private Vector3 initialPosition;
    private bool isChasing = false;

    // Reference to Lygtemandenmovement script
    private Lygtemandenmovement movementScript;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
        movementScript = GetComponent<Lygtemandenmovement>();
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
            // Chase the player
            if (player != null)
            {
                MoveTowards(player.position);
            }
        }

        rb.freezeRotation = true;
    }

    void MoveTowards(Vector2 targetPosition)
    {
        Vector2 moveDirection = (targetPosition - (Vector2)transform.position).normalized;
        rb.velocity = moveDirection * moveSpeed;
    }

    // Add method to enable Lygtemandenmovement script after a delay
    public void ResumePatrollingAfterDelay()
    {
        StartCoroutine(ResumePatrollingCoroutine());
    }

    IEnumerator ResumePatrollingCoroutine()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second
        isChasing = false;
        movementScript.enabled = true; // Enable Lygtemandenmovement script after the delay
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player entered the trigger of a bush
        if (other.gameObject.CompareTag("Bush") && busk.isin)
        {
            isChasing = false; // Player is under a bush, stop chasing
            movementScript.enabled = true; // Enable Lygtemandenmovement script
        }
    }
}
