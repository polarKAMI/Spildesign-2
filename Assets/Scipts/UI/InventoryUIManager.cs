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
    public InventorySO inventorySO;

    private List<GameObject> inventorySlots = new List<GameObject>(); // Keep track of inventory slots

    private bool isMoving = false;

    public GameObject emptyInventoryImage;
    public bool isEmptyInventory = false;

    public GameObject border;

    public TMP_Text itemCounterText;
    public GameObject itemCounter;

    public bool isOpen = false;
    public bool doLogSlots = false;

    public GameObject ammoBar;
    public GameObject ammoBarBoarder;

    public TMP_Text specTXT;

    private PlayerMovement playerMovement;

    private int slotCount = 16;
    private float haloScale = 50f;
    private float startingAngle = 180f;

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
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

        SetSpecs();
    }

    public void CloseInventory()
    {
        OptionsPanelManager optionsPanelManager = FindObjectOfType<OptionsPanelManager>();
        if (optionsPanelManager != null)
        {
            optionsPanelManager.ToggleOptionsPanel(false);
        }

        inventoryPanelObject.SetActive(false);
        ClearInventoryUI();

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
        ClearInventoryUI();
        int inventorySize = inventorySO.Size;
        if (inventorySize == 0)
        {
            emptyInventoryImage.SetActive(true);
            isEmptyInventory = true;
            OptionsPanelManager optionsPanelManager = FindObjectOfType<OptionsPanelManager>();

            if (optionsPanelManager != null)
            {
                optionsPanelManager.ToggleOptionsPanel(false);
            }

            if (border != null)
            {
                border.SetActive(false);
            }
        }
        else
        {
            inventoryPanelObject.SetActive(true);
            emptyInventoryImage.SetActive(false);
            isEmptyInventory = false;

            if (border != null)
            {
                border.SetActive(true);
            }

            float angleStep = -360f / slotCount;
            for (int i = 0; i < slotCount; i++)
            {
                float angle = (startingAngle + i * angleStep) * Mathf.Deg2Rad;
                float x = Mathf.Sin(angle) * haloScale * 2;
                float z = Mathf.Cos(angle) * haloScale + haloScale;

                GameObject slot = Instantiate(inventorySlotPrefab, inventoryPanel);
                slot.transform.localPosition = new Vector3(x, 0f, z);

                int invIndex;
                if (i <= slotCount / 2)
                {
                    invIndex = i % inventorySize;
                }
                else
                {
                    invIndex = (i - slotCount + inventorySize * slotCount) % inventorySize;
                }
                InventoryItem inventoryItem = inventorySO.GetItemAt(invIndex);
                ItemUI itemUI = slot.GetComponentInChildren<ItemUI>();
                itemUI.SetIndex(invIndex);
                itemUI.UpdateItem(inventoryItem);

                inventorySlots.Add(slot);
            }
            Debug.Log("Initialized slots");
            LogDumpSlots();

            ArrangeSlotsByZPosition();
            UpdateItemCounter();
        }
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
        OptionsPanelManager optionsPanelManager = FindObjectOfType<OptionsPanelManager>();
        if (optionsPanelManager != null)
        {
            optionsPanelManager.ResetOptions();
            optionsPanelManager.ToggleOptionsPanel(false);
        }

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

    private InventoryItem[] FindItemsAtZPosition(float zPosition)
    {
        List<InventoryItem> items = new List<InventoryItem>();

        foreach (var slot in inventorySlots)
        {
            ItemUI itemUI = slot.GetComponentInChildren<ItemUI>();
            if (itemUI != null && Mathf.Approximately(itemUI.GetCachedTransform().localPosition.z, zPosition))
            {
                items.Add(itemUI.inventoryItem);
            }
        }

        return items.ToArray();
    }

    private void ArrangeSlotsByZPosition()
    {
        List<GameObject> sortedSlots = inventorySlots.ToList();
        sortedSlots.Sort((a, b) =>
        {
            float zPosA = a.transform.position.z;
            float zPosB = b.transform.position.z;
            return zPosB.CompareTo(zPosA); // Compare in reverse order
        });

        for (int i = 0; i < sortedSlots.Count; i++)
        {
            sortedSlots[i].transform.SetSiblingIndex(i);
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
            ArrangeSlotsByZPosition();

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

    private void LogDumpSlots()
    {
        if (!doLogSlots)
        {
            return;
        }
        int i = 0;
        Debug.Log("--- slots: ---");
        foreach (var slot in inventorySlots)
        {
            ItemUI itemUI = slot.GetComponentInChildren<ItemUI>();
            Debug.LogFormat("- i: {0} Z: {1} item: {2} ({3})", i++, slot.transform.localPosition.z, itemUI.GetIndex(), itemUI.inventoryItem.name);
        }
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
        List<Vector3> initialPositions = new List<Vector3>();
        foreach (var slot in inventorySlots)
        {
            ItemUI itemUI = slot.GetComponentInChildren<ItemUI>();
            Transform transform = itemUI.GetCachedTransform();
            itemUITransforms.Add(transform);
            initialPositions.Add(transform.localPosition);
            itemUIIndices.Add(itemUI.GetIndex());
        }

        int inventorySize = inventorySO.Size;
        int slotCount = inventorySlots.Count;

        List<Vector3> newPositions = new List<Vector3>();
        for (int i = 0; i < slotCount; i++)
        {
            Vector3 targetPosition = initialPositions[(i + direction + slotCount) % slotCount];
            newPositions.Add(targetPosition);

            if (Mathf.Approximately(initialPositions[i].z, haloScale * 2))
            {
                int newIndex = (itemUIIndices[(i + direction + slotCount) % slotCount] - direction + inventorySize) % inventorySize;
                ItemUI targetItemUI = inventorySlots[i].GetComponentInChildren<ItemUI>();
                InventoryItem newItem = inventorySO.GetItemAt(newIndex);
                if (targetItemUI != null)
                {
                    targetItemUI.UpdateItem(newItem);
                    targetItemUI.SetIndex(newIndex);
                    Debug.LogFormat("Updating slot {0} moving from max Z position to display item at index {1}: {2}", i, newIndex, newItem.name);
                }
                else
                {
                    Debug.LogWarning("Slot moving from max Z position not found.");
                }
            }
        }
        Debug.Log("Shifted slots " + direction + " ... positions before smooth moving:");
        LogDumpSlots();

        StartCoroutine(SmoothMoveItems(itemUITransforms, newPositions, 0.13f));
        StartCoroutine(ResetIsMovingFlag(0.14f));
    }

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
            if (itemUI != null && Mathf.Approximately(itemUI.GetCachedTransform().localPosition.z, 0f))
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
