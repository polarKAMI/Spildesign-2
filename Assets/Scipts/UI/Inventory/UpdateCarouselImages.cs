using UnityEngine;
using UnityEngine.UI;
using Inventory.Model; // Assuming you have access to your inventory model

public class UpdateCarouselImages : MonoBehaviour
{
    public InventorySO inventory; // Reference to your inventory scriptable object

    void Update()
    {
        // Loop through each placeholder image in the carousel panel
        for (int i = 0; i < transform.childCount; i++)
        {
            // Get the image component of the current placeholder image
            Image image = transform.GetChild(i).GetComponent<Image>();

            // Check if the item exists at the current index in the inventory
            if (i < inventory.Size)
            {
                // Get the item at the current index in the inventory
                InventoryItem inventoryItem = inventory.GetItemAt(i);

                // Check if the item is not empty and has a valid sprite
                if (!inventoryItem.IsEmpty && inventoryItem.Item.ItemImage != null)
                {
                    // Update the image component of the current placeholder image with the item sprite
                    image.sprite = inventoryItem.Item.ItemImage;
                    image.color = Color.white; // Ensure the image is visible
                }
                else
                {
                    // If the item is empty or has no sprite, clear the image in the placeholder
                    image.sprite = null;
                    image.color = Color.clear; // Make the image transparent
                }
            }
            else
            {
                // If there is no corresponding item in the inventory, clear the image in the placeholder
                image.sprite = null;
                image.color = Color.clear; // Make the image transparent
            }
        }
    }
}