using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathTrigger : MonoBehaviour
{
    private Vector3 originalPosition = new Vector3(1.28999996f, -1f, 0f); // Set the original position here



    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object has the tag "Enemy"
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Has hit deathtrigger");

            // Teleport the object back to its original position
            other.transform.position = originalPosition;
            // Optional: You can also reset velocity, rotation, etc. if needed
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}
