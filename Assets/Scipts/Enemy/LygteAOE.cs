using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LygteAOE : MonoBehaviour
{
    public int damageAmount = 1; // Amount of damage to be inflicted every second
    public Health healthScript; // Reference to the Health script attached to the player
    private bool isTakingDamage = false; // Flag to track if damage is being taken
    private Coroutine continuousDamageCoroutine; // Reference to the continuous damage coroutine

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object has a Health script
        Health otherHealth = other.GetComponent<Health>();
        if (otherHealth != null)
        {
            // Deal initial damage to the player
            healthScript.TakeDamage(damageAmount);
            // Start taking continuous damage if not already taking damage
            if (!isTakingDamage)
            {
                continuousDamageCoroutine = StartCoroutine(TakeContinuousDamage());
                isTakingDamage = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // If the player exits the trigger, stop taking continuous damage
        if (isTakingDamage && continuousDamageCoroutine != null)
        {
            StopCoroutine(continuousDamageCoroutine);
            isTakingDamage = false;
        }
    }

    IEnumerator TakeContinuousDamage()
    {
        // Continuously inflict damage to the player every second
        while (true)
        {
            yield return new WaitForSeconds(1f);
            healthScript.TakeDamage(damageAmount);
        }
    }
}
