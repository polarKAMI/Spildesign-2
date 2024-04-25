using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Svamp")]
public class Svamp : InventoryItem
{

    public int ammoRestoreAmount = 2;

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
                health.AddHealth(5);
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

        playerattack2 attack = player.GetComponent<playerattack2>();

        attack.AddAmmo(ammoRestoreAmount);
    }
}