using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/KeyItem/GoblinKey")]
public class GoblinKey : InventoryItem
{
    public InventorySO inventory;

    public override void Use()
    {
        // Get the player's collider
        Collider2D playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();

        if (playerCollider != null)
        {
            // Check for overlapping trigger colliders with the player's collider
            Collider2D[] overlappingColliders = Physics2D.OverlapBoxAll(playerCollider.bounds.center, playerCollider.bounds.size, 0f, LayerMask.GetMask("Default"));

            foreach (Collider2D collider in overlappingColliders)
            {
                Door door = collider.gameObject.GetComponent<Door>();

                if (door != null)
                {
                    door.TryOpenDoor(this);
                    return; // Exit after interacting with the first door found

                }
            }

            Debug.Log("No door found in the player's vicinity.");
        }
        else
        {
            Debug.Log("Player collider not found.");
        }
    }

    // Remove the key item from the inventory when it successfully opens a door
    public void RemoveFromInventory()
    {
        // Ensure that the inventory reference is set in the inspector or through code
        if (inventory != null)
        {
            inventory.RemoveItem(this);
            FindObjectOfType<InventoryUIManager>().UpdateInventoryUI();

        }
        else
        {
            Debug.LogError("Inventory reference not set.");
        }
    }
}