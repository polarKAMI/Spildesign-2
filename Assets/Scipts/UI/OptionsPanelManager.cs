using System.Linq;
using UnityEngine;

public class OptionsPanelManager : MonoBehaviour
{
    public GameObject optionsPanel; // Reference to the options panel GameObject
    public GameObject[] optionBorders; // Array of GameObjects representing the borders for each option
    public GameObject slotsPanel; // Reference to the slots panel GameObject
    public InventoryUIManager inventoryUIManager;
    public PlayerController playerController;

    private int selectedIndex = 0; // Index of the currently selected option
    public bool isOptionsPanelActive = false; // Flag to track if the options panel is active

    void Start()
    {
        // Disable all option borders at the start
        foreach (var border in optionBorders)
        {
            border.SetActive(false);
        }

    }

    // Method to toggle the options panel (open or close)
    public void ToggleOptionsPanel(bool open)
    {
        if (slotsPanel.activeSelf)
        {
            optionsPanel.SetActive(open); // Set the options panel GameObject to active or inactive based on the 'open' parameter
            if (open)
            {
                isOptionsPanelActive = true; // Set the flag to indicate that the options panel is now active
                SelectOption(0); // Select the first option when opening the panel
                GlobalInputMapping.SetActiveInputMappings(GlobalInputMapping.optionsInputMapping);
            }
            else
            {
                isOptionsPanelActive = false; // Set the flag to indicate that the options panel is now inactive
                ResetOptions(); // Reset options when closing the panel
                GlobalInputMapping.SetActiveInputMappings(GlobalInputMapping.inventoryInputMapping);
            }
        }
    }

    void RemoveSelectedItemFromInventory()
    {
        // Get the highlighted ItemUI
        ItemUI highlightedItem = GetHighlightedItem();

        if (highlightedItem != null)
        {
            // Get the actual index of the highlighted item in InventorySO
            int actualIndex = highlightedItem.GetIndex();

            // If actualIndex is greater than or equal to the inventory size, loop it back
            while (actualIndex >= inventoryUIManager.inventorySO.Size)
            {
                actualIndex -= inventoryUIManager.inventorySO.Size;
            }

            // Remove the item from the InventorySO
            inventoryUIManager.inventorySO.RemoveItemAt(actualIndex);

            // Update the inventory UI
            inventoryUIManager.UpdateInventoryUI();

            Debug.Log("Inventory size after removal: " + inventoryUIManager.inventorySO.Size); // Debug log
        }
        else
        {
            Debug.LogError("No highlighted item found.");
        }
    }

    void SelectOption(int index)
    {      
        optionBorders[index].SetActive(false);
        optionBorders[index].SetActive(true);
    }

    // Method to reset the selected index and disable all option borders
    public void ResetOptions()
    {
        foreach (var border in optionBorders)
        {
            border.SetActive(false);
        }
    }

    public void SelectHighlightedItem()
    {
        ItemUI highlightedItem = GetHighlightedItem();
        if (highlightedItem != null)
        {
            int selectedOption = selectedIndex; // Assuming selectedIndex holds the current selected option index

            switch (selectedOption)
            {
                case 0: // First option selected
                    HandleOptionOne(highlightedItem);
                    break;
                case 1: // Second option selected
                    HandleOptionTwo(highlightedItem);
                    break;
                case 2: // Third option selected
                    HandleOptionThree(highlightedItem);
                    break;
                default:
                    Debug.LogError("Invalid option selected!");
                    break;
            }
        }
        else
        {
            Debug.LogError("No highlighted item found.");
        }
    }

    private void HandleOptionOne(ItemUI itemUI)
    {
        if (itemUI.inventoryItem.isKey)
        {
            itemUI.inventoryItem.Use(); // Use the key directly without removing it
            playerController.ToggleInventory();
        }
        else
        {
            itemUI.inventoryItem.Use(); // Use the non-key item
            RemoveSelectedItemFromInventory(); // Remove the non-key item from inventory
            playerController.ToggleInventory();
            
        }

        selectedIndex = 0;
    }


    private void HandleOptionTwo(ItemUI itemUI)
    {
        if (!itemUI.inventoryItem.isKey)
        {
            itemUI.inventoryItem.Ammo();
            RemoveSelectedItemFromInventory();
            playerController.ToggleInventory();
        }
        else
        {
            Debug.Log("Cannot remove key item.");
        }
        selectedIndex = 0;
    }

    private void HandleOptionThree(ItemUI itemUI)
    {
        if (!itemUI.inventoryItem.isKey)
        {
            RemoveSelectedItemFromInventory();
            playerController.ToggleInventory();
        }
        else
        {
            Debug.Log("Cannot remove key item.");
        }
        selectedIndex = 0;
    }

    public ItemUI GetHighlightedItem()
    {
        // Get all ItemUI components in the inventory panel
        ItemUI[] itemUIs = inventoryUIManager.GetComponentsInChildren<ItemUI>();

        // Find the highlighted ItemUI
        foreach (ItemUI itemUI in itemUIs)
        {
            if (itemUI.IsHighlighted())
            {
                return itemUI;
            }
        }
        return null; // Return null if no highlighted ItemUI is found
    }

    public void ChangeSelectedIndex(int changeAmount)
    {
        int newIndex = selectedIndex + changeAmount;

        // Loop back to the last option if it goes below 0
        if (newIndex < 0)
        {
            newIndex = optionBorders.Length - 1;
        }
        // Loop back to the first option if it exceeds the maximum index
        else if (newIndex >= optionBorders.Length)
        {
            newIndex = 0;
        }

        // Deactivate the previously selected border
        optionBorders[selectedIndex].SetActive(false);

        // Activate the new selected border
        optionBorders[newIndex].SetActive(true);

        // Update the selected index
        selectedIndex = newIndex;
    }
}
