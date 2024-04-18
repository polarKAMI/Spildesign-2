using Inventory.Model;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private UI_InventoryPage inventoryUI;
    [SerializeField] private InventorySO inventoryData;
    public int inventorySize = 20;

    private int currentItemIndex = 0;

    private void Start()
    {
        PrepareUI();
        //inventoryData.Initialize();
    }

    private void PrepareUI()
    {
        
        inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
        inventoryUI.OnItemActionRequested += HandleItemActionRequest;
    }

    private void HandleItemActionRequest(int itemIndex)
    {
        // Implement item action handling here if needed
    }

    private void HandleDescriptionRequest(int itemIndex)
    {
        InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
        if (inventoryItem.IsEmpty)
            return;
        ItemSO item = inventoryItem.Item;
        inventoryUI.UpdateDescription(itemIndex, item.ItemImage, item.name, item.Description);
    }

    private void Update()
    {
        // Handle carousel movement
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveCarousel(1); // Move right
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            MoveCarousel(-1); // Move left
        }

        // Toggle inventory visibility
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryUI.isActiveAndEnabled == false)
            {
                inventoryUI.Show();
                UpdateInventoryUI();
            }
            else
            {
                inventoryUI.Hide();
            }
        }
    }

    private void MoveCarousel(int direction)
    {
        currentItemIndex += direction;

        // Ensure currentItemIndex stays within bounds
        currentItemIndex = Mathf.Clamp(currentItemIndex, 0, inventoryData.Size - 1);

        // Update UI to reflect the current item
        UpdateInventoryUI();
    }

    private void UpdateInventoryUI()
    {
        // Update UI to reflect the current item in the carousel
        List<InventoryItem> items = inventoryData.GetCurrentInventoryState().Values.ToList();
        
    }
}