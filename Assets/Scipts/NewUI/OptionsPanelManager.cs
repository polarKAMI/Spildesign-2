using System.Linq;
using UnityEngine;

public class OptionsPanelManager : MonoBehaviour
{
    public GameObject optionsPanel; // Reference to the options panel GameObject
    public GameObject[] optionBorders; // Array of GameObjects representing the borders for each option
    public GameObject slotsPanel; // Reference to the slots panel GameObject
    public InventoryUIManager inventoryUIManager;

    private int selectedIndex = 0; // Index of the currently selected option
    private bool isOptionsPanelActive = false; // Flag to track if the options panel is active

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
            }
            else
            {
                isOptionsPanelActive = false; // Set the flag to indicate that the options panel is now inactive
                ResetOptions(); // Reset options when closing the panel
            }
        }
    }

    void Update()
    {
        // Check for input to open the options panel (only if it's not already active)
        if (!isOptionsPanelActive && Input.GetKeyDown(KeyCode.C))
        {
            ToggleOptionsPanel(true);
        }

        // Check for input to close the options panel
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            ToggleOptionsPanel(false);
        }

        // Check for input to select the highlighted item (if the options panel is active)
        if (isOptionsPanelActive && Input.GetKeyDown(KeyCode.Return))
        {
            SelectHighlightedItem();

            // Remove the selected item from the inventory
            RemoveSelectedItemFromInventory();
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

            Debug.Log("Actual index of highlighted item: " + actualIndex); // Debug log

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
        // Remove the border highlight from the previously selected option
        optionBorders[index].SetActive(false);

        // Highlight the selected option with a border
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
            HandleSelectedItem(highlightedItem);
        }
        else
        {
            Debug.LogError("No highlighted item found.");
        }
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

    private void HandleSelectedItem(ItemUI itemUI)
    {
        // Get the InventoryItem from the highlighted ItemUI
        InventoryItem inventoryItem = itemUI.inventoryItem;

        if (inventoryItem.Item != null)
        {
            // Here, you can implement the logic for what happens when a highlighted item is selected.
            // For example, you can use the item's data to perform an action, equip an item, etc.
            Debug.Log("Selected Highlighted Item: " + inventoryItem.Item.Name); // Accessing the item's name
        }
        else
        {
            Debug.LogError("No highlighted item found.");
        }
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
