using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class InventoryUIManager : MonoBehaviour
{
    public GameObject inventorySlotPrefab;
    public Transform inventoryPanel;
    public GameObject inventoryPanelObject;
    public InventorySO inventorySO; // Reference to the InventorySO scriptable object


    private List<GameObject> inventorySlots = new List<GameObject>(); // Keep track of inventory slots

    private bool isMoving = false;
   
    public GameObject emptyInventoryImage; // Reference to the empty inventory image
    public bool isEmptyInventory = false; // Flag to track if the inventory is empty

    public GameObject border;

    public TMP_Text itemCounterText; // Reference to TextMeshPro component
    public GameObject itemCounter; // Reference to ItemCounter GameObject

    public bool isOpen = false;
    private int currentIndex = -1;

    public GameObject ammoBar; // Reference to the AmmoBar UI GameObject
    public GameObject ammoBarBoarder; // Reference to the AmmoBar UI GameObject

    public TMP_Text specTXT;

    private PlayerMovement playerMovement;
    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>(); // Find the PlayerMovement script in the scene
        ammoBar.SetActive(false);
        ammoBarBoarder.SetActive(false);
        if (inventorySO != null)
        {
            inventorySO.WipeInventory();
        }
    }
    public void OpenInventory()
    {

        if (playerMovement != null)
        {
            playerMovement.DisableMovement();
        }
        // Update inventory UI
        UpdateInventoryUI();
        isOpen = true;

        if (ammoBar != null)
        {
            ammoBar.SetActive(true);
        }

        if (ammoBar != null)
        {
            ammoBarBoarder.SetActive(true);
        }

        currentIndex = 0;

        // Update specs text
        SetSpecs();
    }

    public void CloseInventory()
    {
        // Close the options panel if it's active
        OptionsPanelManager optionsPanelManager = FindObjectOfType<OptionsPanelManager>();
        if (optionsPanelManager != null)
        {
            optionsPanelManager.ToggleOptionsPanel(false);
        }

        // Close the inventory panel
        inventoryPanelObject.SetActive(false);
        ClearInventoryUI();

        // Enable player movement when closing inventory
        if (playerMovement != null)
        {
            playerMovement.EnableMovement();
        }

        isOpen = false;

        if (ammoBar != null)
        {
            ammoBar.SetActive(false);
        }

        if (ammoBar != null)
        {
            ammoBarBoarder.SetActive(false);
        }
    }

    public void ScrollInventoryLeft()
    {
        if (!isMoving && !isEmptyInventory)
        {
            ShiftInventorySlots(1);
        }
    }

    public void ScrollInventoryRight()
    {
        if (!isMoving && !isEmptyInventory)
        {
            ShiftInventorySlots(-1);
        }
    }

    public void UpdateInventoryUI()
    {
        // Clear existing inventory UI
        ClearInventoryUI();

        // Get the current inventory size
        int inventorySize = inventorySO.Size;

        // Check if the inventory is empty
        if (inventorySize == 0)
        {
            // Show the empty inventory image
            emptyInventoryImage.SetActive(true);


            // Set the flag to indicate empty inventory
            isEmptyInventory = true;

            // Disable options panel
            OptionsPanelManager optionsPanelManager = FindObjectOfType<OptionsPanelManager>();
            if (optionsPanelManager != null)
            {
                optionsPanelManager.ToggleOptionsPanel(false);
            }
            
            if (border != null)
            {
                border.SetActive(false);
            }
            // Return without creating inventory slots if the inventory is empty
            return;
        }
        else
        {
            inventoryPanelObject.SetActive(true);
            // Hide the empty inventory image
            emptyInventoryImage.SetActive(false);

            // Reset the flag
            isEmptyInventory = false;

            if (border != null)
            {
                border.SetActive(true);
            }

            // Always show 16 slots
            int slotCount = 16;
            float angleStep = -360f / slotCount;
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
                slot.transform.localPosition = new Vector3(x, 0f, z / 2f);

                InventoryItem inventoryItem = inventorySO.GetItemAt(i);

                // Get the ItemUI component from the inventory slot prefab
                ItemUI itemUI = slot.GetComponentInChildren<ItemUI>();
                itemUI.SetIndex(i);
                itemUI.SetActualIndex(i);
                
                    // Update the item image


                // Store the transform of the ItemUI instance
                itemUITransforms.Add(itemUI.GetCachedTransform());
            }
            for (int i = 0; i <= 8; i++)
            {
                InventoryItem inventoryItem = inventorySO.GetItemAt(i);
                ItemUI itemUI = inventorySlots[i].GetComponentInChildren<ItemUI>();
                itemUI.SetActualIndex(i);
                itemUI.UpdateItem(inventoryItem);
            }

            int reverseIndex = inventorySize - 1; // Start from the last item
            for (int i = slotCount - 1; i >= 9; i--)
            {
                if (reverseIndex < 0)
                    reverseIndex = inventorySize - 1;

                InventoryItem inventoryItem = inventorySO.GetItemAt(reverseIndex);
                ItemUI itemUI = inventorySlots[i].GetComponentInChildren<ItemUI>();
                itemUI.SetActualIndex(reverseIndex);
                itemUI.UpdateItem(inventoryItem);

                reverseIndex--; // Move to the previous item
            }

            SortInventorySlotsByZPosition();
            UpdateItemCounter();
        }
        SortInventorySlotsByZPosition();
            UpdateItemCounter();
    }

    
    private void UpdateItemCounter()
    {
        if (itemCounterText != null && inventorySO != null)
        {
            itemCounterText.text = inventorySO.Size.ToString();
        }
    }


    private void ClearInventoryUI()
    {
        // Call ResetOptions from OptionsPanelManager script if it's available
        OptionsPanelManager optionsPanelManager = FindObjectOfType<OptionsPanelManager>();
        if (optionsPanelManager != null)
        {
            optionsPanelManager.ResetOptions();
            optionsPanelManager.ToggleOptionsPanel(false); // Toggle options panel off
        }

        // Destroy inventory slots
        foreach (GameObject slot in inventorySlots)
        {
            Destroy(slot);
        }
        inventorySlots.Clear();

        if (emptyInventoryImage != null)
        {
            emptyInventoryImage.SetActive(false);
        }

    }
    public void UpdateInventoryItems(int direction)
    {
        // Find the items in the slots with a Z position approximately equal to 30.86583
        InventoryItem[] itemsAtZ30_8 = FindItemsAtZPosition(30.86583f);

        // Log the count of items found at Z position 30.86583
        Debug.Log("Items found at Z position 30.86583 count: " + itemsAtZ30_8.Length);

        // Ensure that itemsAtZ30_8 array is not empty before accessing its elements
        if (itemsAtZ30_8.Length > 0)
        {
            // Get the current item displayed in the slots with Z = 30.86583
            InventoryItem currentItem = itemsAtZ30_8[0];

            // Find the index of the current item in the inventory
            int currentIndex = inventorySO.GetCurrentInventoryState().Keys.FirstOrDefault(index => inventorySO.GetCurrentInventoryState()[index] == currentItem);

            // Calculate the index of the next item based on the direction
            int nextIndex = (currentIndex + direction + inventorySO.Size) % inventorySO.Size;

            // Get the next inventory item to display
            InventoryItem nextItem = inventorySO.GetItemAt(nextIndex);

            // Update the item image in the UI with the next inventory item
            foreach (var item in itemsAtZ30_8)
            {
 
            }
        }
        else
        {
            Debug.LogWarning("No items found at Z position 30.86583");
        }
    }

    private InventoryItem[] FindItemsAtZPosition(float zPosition)
    {
        List<InventoryItem> items = new List<InventoryItem>();

        foreach (var slot in inventorySlots)
        {
            ItemUI itemUI = slot.GetComponentInChildren<ItemUI>();
            if (itemUI != null && Mathf.Approximately(slot.transform.localPosition.z, zPosition))
            {
                items.Add(itemUI.inventoryItem);
            }
        }

        return items.ToArray();
    }


    private int GetHighestZIndex()
    {
        float highestZ = float.MinValue;
        int highestZIndex = -1;

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].transform.localPosition.z > highestZ)
            {
                highestZ = inventorySlots[i].transform.localPosition.z;
                highestZIndex = i;
            }
        }

        return highestZIndex;
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
            if (itemUITransforms == null)
            {
                yield return null;
            }
            else
            {
                for (int i = 0; i < itemUITransforms.Count; i++)
                {
                    if (itemUITransforms[i] != null)
                    {
                        itemUITransforms[i].localPosition = Vector3.Lerp(itemUITransforms[i].localPosition, targetPositions[i], elapsedTime / duration);
                    }
                }
            }
            elapsedTime += Time.deltaTime;
            SortInventorySlotsByZPosition();
            
            yield return null;
        }

        // Ensure items reach their final positions
        for (int i = 0; i < itemUITransforms.Count; i++)
        {
            if (itemUITransforms[i] != null)
            {
                itemUITransforms[i].localPosition = targetPositions[i];
            }
        }

        SetSpecs();
    }

    private void ShiftInventorySlots(int direction)
    {
        if (isMoving)
        {
            return;
        }

        isMoving = true;

        List<Transform> itemUITransforms = new List<Transform>();
        List<int> itemUIIndices = new List<int>();
        foreach (var slot in inventoryPanel.GetComponentsInChildren<ItemUI>())
        {
            itemUITransforms.Add(slot.GetCachedTransform());
            itemUIIndices.Add(slot.GetIndex());
        }

        List<Vector3> initialPositions = new List<Vector3>();
        foreach (var transform in itemUITransforms)
        {
            initialPositions.Add(transform.localPosition);
        }

        int itemCount = itemUITransforms.Count;

        List<Vector3> newPositions = new List<Vector3>();
        for (int i = 0; i < itemCount; i++)
        {
            int currentIndex = itemUIIndices[i];
            int targetIndex = (currentIndex + direction + itemCount) % itemCount;

            if (targetIndex < 0)
            {
                targetIndex = itemCount - 1;
            }
            else if (targetIndex >= itemCount)
            {
                targetIndex = 0;
            }

            Vector3 targetPosition = initialPositions[itemUIIndices.IndexOf(targetIndex)];
            newPositions.Add(targetPosition);
        }

        StartCoroutine(SmoothMoveItems(itemUITransforms, newPositions, 0.13f));

        UpdateInventoryItems(direction);

        StartCoroutine(ResetIsMovingFlag(0.14f));

        int specIndex = (currentIndex + direction + itemCount) % itemCount;
        if (specIndex < 0)
        {
            specIndex = itemCount - 1;
        }
    }

    // Coroutine to reset the isMoving flag after a delay
    private IEnumerator ResetIsMovingFlag(float delay)
    {
        yield return new WaitForSeconds(delay);
        isMoving = false; // Reset the flag to indicate movement has finished
        
    }

    private void SetSpecs()
    {
        foreach (var slot in inventorySlots)
        {
            ItemUI itemUI = slot.GetComponentInChildren<ItemUI>();
            if (itemUI != null && Mathf.Approximately(slot.transform.localPosition.z, 0f))
            {
                InventoryItem item = itemUI.inventoryItem;
                if (item != null && specTXT != null)
                {
                    specTXT.text = item.Specs;
                    Debug.Log($"SetSpecs: Index = {itemUI.GetIndex()}, Item Name = {item.name}, Specs = {item.Specs}");
                }
                else
                {
                    specTXT.text = string.Empty;
                    Debug.Log("SetSpecs: Item or specTXT is null");
                }
                return; // Exit once we've found the item at z=0
            }
        }
        // In case no item is found at z=0
        specTXT.text = string.Empty;
        Debug.Log("SetSpecs: No item found at z=0");
    }
}
