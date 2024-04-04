using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public GameObject inventoryUIManager;
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
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
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
                uiManager.CreateInventorySlots(16);
                isInventoryOpen = true;
            }
        }
        else
        {
            Debug.LogError("InventoryUIManager is not initialized.");
        }
    }
}