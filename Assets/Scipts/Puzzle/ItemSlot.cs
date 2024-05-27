using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class ItemSlot : MonoBehaviour, IInteractable
{
    public InventoryItem requiredItem; // The item required to be inserted into this slot

    public InventorySO inventory;

    private InventoryItem currentItem; // The item currently inserted into this slot

    public SpriteRenderer itemRenderer;
    public PlayerController playerController;

    public bool isMatch = false;
    private bool doorOpened = false;

    // Method to insert an item into the slot

    public void Interact()
    {
        OnInteractKeyPressed();
    }
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
        else
        {
            if(playerController != null)
            {
                playerController.ToggleInventory();
            }
        }
    }
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