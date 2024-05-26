using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    public Health health;

    // This method will be called when another collider enters the trigger collider attached to the object where this script is attached
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered the trigger has the tag "Player"
        if (other.gameObject.CompareTag("Player"))
        {
            // Access the Health component on the player and apply damage
            health.TakeDamage(10);
        }
    }
}
