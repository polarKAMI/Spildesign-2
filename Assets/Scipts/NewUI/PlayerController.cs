using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject inventoryUIManager;
    private bool isInventoryOpen = false; // Flag to track inventory state

    void Start()
    {
        // Ensure inventoryUIManager is assigned before using it
        if (inventoryUIManager == null)
        {
            // Try to find the GameObject with the InventoryUIManager script
            inventoryUIManager = GameObject.Find("Inventory"); // Change "Inventory" to match the name of your GameObject
            if (inventoryUIManager == null)
            {
                Debug.LogError("InventoryUIManager GameObject not found in the scene.");
            }
        }
    }

    void Update()
    {
        // Toggle inventory when the I key is pressed
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }

        // Scroll inventory left when the A key is pressed
        if (isInventoryOpen && Input.GetKeyDown(KeyCode.A))
        {
            ScrollInventoryLeft();
        }

        // Scroll inventory right when the D key is pressed
        if (isInventoryOpen && Input.GetKeyDown(KeyCode.D))
        {
            ScrollInventoryRight();
        }
    }

    void ToggleInventory()
    {
        if (inventoryUIManager != null)
        {
            // Get the InventoryUIManager component
            InventoryUIManager uiManager = inventoryUIManager.GetComponent<InventoryUIManager>();

            // Toggle the inventory state based on the flag
            if (isInventoryOpen)
            {
                uiManager.CloseInventory();
                isInventoryOpen = false;
            }
            else
            {
                uiManager.OpenInventory();
                isInventoryOpen = true;
            }
        }
        else
        {
            Debug.LogError("InventoryUIManager is not initialized.");
        }
    }

    void ScrollInventoryLeft()
    {
        if (inventoryUIManager != null)
        {
            // Get the InventoryUIManager component
            InventoryUIManager uiManager = inventoryUIManager.GetComponent<InventoryUIManager>();

            // Call the ScrollInventoryLeft function
            uiManager.ScrollInventoryLeft();
        }
    }

    void ScrollInventoryRight()
    {
        if (inventoryUIManager != null)
        {
            // Get the InventoryUIManager component
            InventoryUIManager uiManager = inventoryUIManager.GetComponent<InventoryUIManager>();

            // Call the ScrollInventoryRight function
            uiManager.ScrollInventoryRight();
        }
    }
}
