using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    public GameObject inventorySlotPrefab;
    public Transform inventoryPanel;
    public GameObject inventoryPanelObject;


    public void OpenInventory()
    {
        Debug.Log("Opening inventory."); // Add this line to check if the method is being called
        inventoryPanelObject.SetActive(true);
    }

    public void CloseInventory()
    {
        Debug.Log("Closing inventory."); // Add this line to check if the method is being called
        inventoryPanelObject.SetActive(false);
    }
    public void CreateInventorySlots(int slotCount)
    {
        // Calculate positions for the slots (example: circular layout)
        float angleStep = 360f / slotCount;
        float radius = 100f; // Adjust as needed

        for (int i = 0; i < slotCount; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            float x = Mathf.Sin(angle) * radius;
            float z = Mathf.Cos(angle) * radius;

            // Instantiate the slot prefab
            GameObject ItemUI = Instantiate(inventorySlotPrefab, inventoryPanel);
            ItemUI.transform.localPosition = new Vector3(x, 0, z - 180);
        }
    }
}