using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    public InventoryItem requiredItem; // The item required to be inserted into this slot

    public InventorySO inventory;

    private InventoryItem currentItem; // The item currently inserted into this slot

    public SpriteRenderer itemRenderer;

    private bool isPlayerInside = false; // Flag to track if the player is inside the slot area

    public bool isMatch = false;
    private bool doorOpened = false;

    // Method to insert an item into the slot
    public void InsertItem(InventoryItem item)
    {
        if (item == null)
        {
            Debug.LogWarning("Cannot insert null item.");
            return;
        }

        if (currentItem == null)
        {
            currentItem = item;
            Debug.Log($"Inserted {item.itemName} into the slot.");

            if (itemRenderer != null && item.itemIcon != null)
            {
                itemRenderer.sprite = item.itemIcon;
            }

            if (item is MainKey)
            {
                MainKey mainKey = item as MainKey;
                mainKey.RemoveFromInventory();
            }

            CheckCombination();
        }
        else
        {
            Debug.LogWarning("Slot is already occupied.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            Debug.Log("Player entered the slot area.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            Debug.Log("Player exited the slot area.");
        }
    }

    private void Update()
    {
        // Check if the player is inside and interacts with the slot
        if (isPlayerInside && !doorOpened && currentItem != null && Input.GetKeyDown(KeyCode.Q))
        {
            OnInteractKeyPressed();
        }
    }

    private void OnInteractKeyPressed()
    {
        if (currentItem != null)
        {
            Debug.Log($"Interacting with {currentItem.itemName}.");

            if (inventory != null)
            {
                inventory.AddItem(currentItem);
            }
            else
            {
                Debug.LogError("PlayerInventory reference not set.");
            }
            // Clear the currentItem and update the SpriteRenderer
            currentItem = null;
            isMatch = false;
            if (itemRenderer != null)
            {
                itemRenderer.sprite = null;
            }
        }
    }

    // Function to check combination after inserting item
    private void CheckCombination()
    {
        if (requiredItem != null && currentItem != null)
        {
            if (requiredItem == currentItem)
            {
                Debug.Log($"Slot {gameObject.name} matches the required item: {requiredItem.itemName}.");
                isMatch = true; // Set the boolean flag to true if the items match
            }
            else
            {
                Debug.Log($"Slot {gameObject.name} does not match the required item: {requiredItem.itemName}.");
                isMatch = false; // Set the boolean flag to false if the items do not match
            }
        }
    }
    public void SetDoorOpened(bool opened)
    {
        doorOpened = opened;
    }
}