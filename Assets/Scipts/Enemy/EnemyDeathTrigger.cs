using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathTrigger : MonoBehaviour
{
    public Transform originalPositionTransform; // Public Transform to set the original position in the Inspector
    private Vector3 cachedOriginalPosition;
    private Quaternion cachedOriginalRotation;

    void Start()
    {
        // Cache the initial position and rotation at the start of the scene
        if (originalPositionTransform != null)
        {
            cachedOriginalPosition = originalPositionTransform.position;
            cachedOriginalRotation = originalPositionTransform.rotation;
        }
        else
        {
            Debug.LogWarning("Original position transform is not set.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object has the tag "Enemy"
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Has hit death trigger");

            // Teleport the object back to its cached original position
            other.transform.position = cachedOriginalPosition;
            other.transform.rotation = cachedOriginalRotation;

            // Optional: You can also reset velocity, rotation, etc. if needed
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }
    }
}
