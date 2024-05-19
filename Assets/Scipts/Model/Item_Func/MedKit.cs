using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Medkit")]
public class MedKit : InventoryItem
{
    
    public int ammoRestoreAmount = 5;

    public override void Use()
    {
        // Find the player GameObject by tag (assuming it's tagged as "Player")
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // Check if the player GameObject is found
        if (player != null)
        {
            // Get the Health component from the player GameObject
            Health health = player.GetComponent<Health>();

            // Check if the Health component is found
            if (health != null)
            {
                // Set the player's current health to its maximum health
                health.currentHealth = health.MaxHealth;
                Debug.Log("Player's health restored to maximum.");
            }
            else
            {
                Debug.LogWarning("Health component not found on the player GameObject.");
            }
        }
        else
        {
            Debug.LogWarning("Player GameObject not found.");
        }
    }

    public override void Ammo()
    {
        Debug.Log($"Using {itemName} to restore {ammoRestoreAmount} ammo.");

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Shoot attack = player.GetComponent<Shoot>();

        attack.AddAmmo(ammoRestoreAmount);
    }
}