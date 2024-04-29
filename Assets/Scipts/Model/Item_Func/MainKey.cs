using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/KeyItem/MainKey")]
public class MainKey : InventoryItem
{
    public InventorySO inventory;
    public override void Use()
    {
        Collider2D playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();

        if (playerCollider != null)
        {
            Collider2D[] overlappingColliders = Physics2D.OverlapBoxAll(playerCollider.bounds.center, playerCollider.bounds.size, 0f);

            foreach (Collider2D collider in overlappingColliders)
            {
                ItemSlot itemSlot = collider.GetComponent<ItemSlot>();

                if (itemSlot != null)
                {
                    itemSlot.InsertItem(this);
                    return;
                }
            }

            Debug.Log("No item slot found in the player's vicinity.");
        }
        else
        {
            Debug.Log("Player collider not found.");
        }
    }
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