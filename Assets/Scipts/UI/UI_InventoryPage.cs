using Inventory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_InventoryPage : MonoBehaviour
{
    [SerializeField]
    private RectTransform contentPanel;

    [SerializeField]
    private UIInventoryDescription itemDescription;

    List<Sprite> itemSprites = new List<Sprite>(); // Store sprites of items

    public event Action<int> OnDescriptionRequested,
        OnItemActionRequested;

    private void Awake()
    {
        Hide();
        itemDescription.ResetDescription();
    }

    // Method to load items into the carousel menu
    public void LoadItems(List<Sprite> sprites)
    {
        itemSprites = sprites;

        // Update carousel images with item sprites
        for (int i = 0; i < itemSprites.Count; i++)
        {
            Transform slot = contentPanel.GetChild(i);
           
           
        }

        // Clear remaining slots if item count is less than total slots
        for (int i = itemSprites.Count; i < contentPanel.childCount; i++)
        {
            Transform slot = contentPanel.GetChild(i);
            
            
        }
    }

    // Handle other UI interactions as before...

    public void Show()
    {
        gameObject.SetActive(true);
        itemDescription.ResetDescription();
        ResetSelection();
    }

    private void ResetSelection()
    {
        itemDescription.ResetDescription();
        // DeselectAllItems(); // No need to deselect items in this context
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    internal void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
    {
        itemDescription.SetDescription(itemImage, name, description);
        // DeselectAllItems(); // No need to deselect items in this context
    }
}
