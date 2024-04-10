using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    public GameObject inventorySlotPrefab;
    public Transform inventoryPanel;
    public GameObject inventoryPanelObject;
    public InventorySO inventorySO; // Reference to the InventorySO scriptable object

    private List<GameObject> inventorySlots = new List<GameObject>(); // Keep track of inventory slots

    private bool isMoving = false;

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

    public void ScrollInventoryLeft()
    {
        if (!isMoving)
        {
            ShiftInventorySlots(1);
        }
    }

    public void ScrollInventoryRight()
    {
        if (!isMoving)
        {
            ShiftInventorySlots(-1);
        }
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

        // List to store the transforms of ItemUI instances
        List<Transform> itemUITransforms = new List<Transform>();

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

            // Get the ItemUI component from the inventory slot prefab
            ItemUI itemUI = slot.GetComponentInChildren<ItemUI>();
            itemUI.SetIndex(i);

            // Update the item image
            itemUI.UpdateItem(inventoryItem.Item.ItemImage);

            // Store the transform of the ItemUI instance
            itemUITransforms.Add(itemUI.GetCachedTransform());
        }
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
        // Sort the inventory slots based on their z-axis position
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
    private IEnumerator SmoothMoveItems(List<Transform> itemUITransforms, List<Vector3> targetPositions, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            for (int i = 0; i < itemUITransforms.Count; i++)
            {
                itemUITransforms[i].localPosition = Vector3.Lerp(itemUITransforms[i].localPosition, targetPositions[i], elapsedTime / duration);
            }
            elapsedTime += Time.deltaTime;
            SortInventorySlotsByZPosition();
            yield return null;
        }

        // Ensure items reach their final positions
        for (int i = 0; i < itemUITransforms.Count; i++)
        {
            itemUITransforms[i].localPosition = targetPositions[i];
        }
    }

    private void ShiftInventorySlots(int direction)
    {
        // Don't proceed if movement is already in progress
        if (isMoving)
        {
            return;
        }

        isMoving = true; // Set the flag to indicate movement has started

        // Gather the transforms and indices of ItemUI instances
        List<Transform> itemUITransforms = new List<Transform>();
        List<int> itemUIIndices = new List<int>();
        foreach (var slot in inventoryPanel.GetComponentsInChildren<ItemUI>())
        {
            itemUITransforms.Add(slot.GetCachedTransform()); // Use cached transform
            itemUIIndices.Add(slot.GetIndex());
        }

        // Gather initial positions of all ItemUIs
        List<Vector3> initialPositions = new List<Vector3>();
        foreach (var transform in itemUITransforms)
        {
            initialPositions.Add(transform.localPosition);
        }

        int itemCount = itemUITransforms.Count;

        // Calculate new positions based on direction
        List<Vector3> newPositions = new List<Vector3>();
        for (int i = 0; i < itemCount; i++)
        {
            int currentIndex = itemUIIndices[i];
            int targetIndex = (currentIndex + direction + itemCount) % itemCount;

            // Adjust for wrapping around when reaching the end of the list
            if (targetIndex < 0)
            {
                targetIndex = itemCount - 1; // Set target index to last index
            }
            else if (targetIndex >= itemCount)
            {
                targetIndex = 0; // Wrap around to index 0 if targetIndex exceeds itemCount
            }

            // Find the initial position of the ItemUI with the target index
            Vector3 targetPosition = initialPositions[itemUIIndices.IndexOf(targetIndex)];

            // Store the new position
            newPositions.Add(targetPosition);
        }

        // Smoothly move items to their new positions
        StartCoroutine(SmoothMoveItems(itemUITransforms, newPositions, 0.13f)); // Adjust duration as needed

        // Update the cached transform for all ItemUI instances
        foreach (var slot in inventoryPanel.GetComponentsInChildren<ItemUI>())
        {
            slot.UpdateCachedTransform();
        }

        // Sort the inventory slots based on their z-axis position
        SortInventorySlotsByZPosition();

        // Set a coroutine to reset the isMoving flag after a delay
        StartCoroutine(ResetIsMovingFlag(0.14f)); // Adjust delay to match the animation duration
    }

    // Coroutine to reset the isMoving flag after a delay
    private IEnumerator ResetIsMovingFlag(float delay)
    {
        yield return new WaitForSeconds(delay);
        isMoving = false; // Reset the flag to indicate movement has finished
    }
}
