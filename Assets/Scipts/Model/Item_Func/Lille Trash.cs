using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Lille Trash")]
public class LilleTrash : InventoryItem
{
    public int AmmoRestoreAmount;

    public override void Use()
    {
        // Implement the specific use behavior for consumable items
        Debug.Log($"Using {itemName} to restore {AmmoRestoreAmount} health.");

        // Find the player GameObject by tag (assuming it's tagged as "Player")
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // Check if the player GameObject is found
        if (player != null)
        {
            // Get the Health component from the player GameObject
            playerattack2 attack = player.GetComponent<playerattack2>();

            // Check if the Health component is found
            if (attack != null)
            {
                // Set the player's current health to its maximum health
                attack.AddAmmo(AmmoRestoreAmount);
                Debug.Log("Player's ammo + 10.");
            }
            else
            {
                Debug.LogWarning("Attack component not found on the player GameObject.");
            }
        }
        else
        {
            Debug.LogWarning("Player GameObject not found.");
        }
    }
}
