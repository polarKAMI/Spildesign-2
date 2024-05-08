using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skud : MonoBehaviour
{
    private void Start()
    {
        Invoke("DestroyObject", 0.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Check if the collided object has the EnemyAI script
            EnemyAI enemyAI = collision.gameObject.GetComponent<EnemyAI>();
            enemyhealth enemyHealth = collision.gameObject.GetComponent<enemyhealth>();
            if (enemyAI != null)
            {
                // If the collided object has the EnemyAI script, apply damage
                enemyAI.TakeDamage(5);
            }
            if (enemyHealth != null)
            {
                enemyHealth.Takedamage(5);

                if (enemyHealth.currentenemyhealth <= 0)
                {
                    Destroy(collision.gameObject);
                    enemyHealth.currentenemyhealth = enemyHealth.Maxhealth;
                }
            }

        
        // Destroy the projectile regardless of whether damage was applied
        Destroy(gameObject);
        }
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}