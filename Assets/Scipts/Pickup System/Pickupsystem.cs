using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory.Model;

public class Pickupsystem : MonoBehaviour
{
    [SerializeField]
    private InventorySO inventoryData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.GetComponent<Item>();

        if (item != null)
        {
          
            
            
                // Add the item to the inventory data
                inventoryData.AddItem(item.InventoryItem);

            item.DestroyItem();
            
        }
    }
}
