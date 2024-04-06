using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    // Reference to the UI image for the item
    public Image itemImage;

    // Method to set the sprite of the "Item" UI image
    public void SetItemSprite(Sprite sprite)
    {
        // Set the sprite of the item image
        itemImage.sprite = sprite; // Line 13
    }
}