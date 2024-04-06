using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    public GameObject inventorySlotPrefab;
    public Transform inventoryPanel;
    public GameObject inventoryPanelObject;
    public InventorySO inventorySO; // Reference to the InventorySO scriptable object

    private List<GameObject> inventorySlots = new List<GameObject>(); // Keep track of inventory slots

    public void OpenInventory()
    {
        inventoryPanelObject.SetActive(true);
        UpdateInventoryUI();
    }

    public void CloseInventory()
    {
        inventoryPanelObject.SetActive(false);
        ClearInventoryUI();
    }

    private void UpdateInventoryUI()
    {
        // Clear existing inventory UI
        ClearInventoryUI();

        // Create inventory slots based on inventorySO data
        int slotCount = inventorySO.Size;
        float angleStep = 360f / slotCount;
        float radius = 100f; // Adjust as needed
        float startingAngle = 180f; // Adjust as needed

        for (int i = 0; i < slotCount; i++)
        {
            float angle = (startingAngle + i * angleStep) * Mathf.Deg2Rad;
            float x = Mathf.Sin(angle) * radius;
            float z = Mathf.Cos(angle) * radius;

            z += radius;

            GameObject slot = Instantiate(inventorySlotPrefab, inventoryPanel);
            inventorySlots.Add(slot);

            // Set position of inventory slot
            slot.transform.localPosition = new Vector3(x / 1.5f, 0f, z / 2f);


            InventoryItem inventoryItem = inventorySO.GetItemAt(i);

            // Check if the item is not empty
            if (!inventoryItem.IsEmpty && inventoryItem.Item != null)
            {
                // Get the ItemUI component from the inventory slot prefab
                ItemUI itemUI = slot.GetComponentInChildren<ItemUI>();

                // Set the sprite of the "Item" UI image
                itemUI.SetItemSprite(inventoryItem.Item.ItemImage);
            }
        }

        // Sort the instantiated prefabs based on their z-axis position
        SortInventorySlotsByZPosition();
    }

    private void ClearInventoryUI()
    {
        foreach (GameObject slot in inventorySlots)
        {
            Destroy(slot);
        }
        inventorySlots.Clear();
    }

    private void SortInventorySlotsByZPosition()
    {
        // Sort the inventory slots based on their z-axis position in reverse order
        inventorySlots.Sort((a, b) =>
        {
            float zPosA = a.transform.position.z;
            float zPosB = b.transform.position.z;
            return zPosB.CompareTo(zPosA); // Compare in reverse order
        });

        // Reorder the inventory slots based on the sorted list
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            inventorySlots[i].transform.SetSiblingIndex(i);
        }
    }
}