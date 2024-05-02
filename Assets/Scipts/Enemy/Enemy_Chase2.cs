using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Chase2 : MonoBehaviour
{
    public Transform player; // Reference to the player object
    public float moveSpeed = 5f; // Movement speed of the enemy
    public float detectionRange = 5f; // Range within which the enemy detects the player
    public Transform[] patrolPoints; // Array of patrol points for the enemy to move between
    public float stoppingDistance = 1f; // Distance from patrol points where the enemy will stop

    private Rigidbody2D rb;
    private Vector3 initialPosition;
    private bool isChasing = false;
    private int currentPatrolIndex = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (!isChasing)
        {
            // Check for player within detection range
            if (player != null && Vector2.Distance(transform.position, player.position) <= detectionRange)
            {
                // Check if player is not obstructed by obstacles
                RaycastHit2D hit = Physics2D.Raycast(transform.position, player.position - transform.position, detectionRange);
                if (hit.collider != null && hit.collider.CompareTag("Player"))
                {
                    isChasing = true;
                }
            }
        }
        else
        {
            // Chase the player
            if (player != null)
            {
                MoveTowards(player.position);

                // Check if player is outside detection range
                if (Vector2.Distance(transform.position, player.position) > detectionRange)
                {
                    isChasing = false;
                }
            }
        }

        // If not chasing, return to patrol points
        if (!isChasing)
        {
            Patrol();
        }
    }

    void MoveTowards(Vector2 targetPosition)
    {
        Vector2 moveDirection = (targetPosition - (Vector2)transform.position).normalized;
        rb.velocity = moveDirection * moveSpeed;
    }

    void Patrol()
    {
        // Move towards the current patrol point
        Vector2 targetPosition = patrolPoints[currentPatrolIndex].position;
        MoveTowards(targetPosition);

        // Check if arrived at the current patrol point
        if (Vector2.Distance(transform.position, targetPosition) < stoppingDistance)
        {
            // Move to the next patrol point
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }
}

