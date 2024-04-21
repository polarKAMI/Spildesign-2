using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupsystem : MonoBehaviour
{
    [SerializeField]
    private InventorySO inventoryData; // Reference to the InventorySO scriptable object

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.GetComponent<Item>(); // Get the Item component attached to the collided object

        if (item != null)
        {
            // Check if the inventory already contains the item
            if (!inventoryData.ContainsItem(item.InventoryItem))
            {
                // Add the item to the inventory data
                inventoryData.AddItem(item.InventoryItem);

                // Now, you might want to destroy the item after picking it up
                item.DestroyItem();
            }
        }
    }
}
