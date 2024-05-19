using Inventory.Model;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InventorySO : ScriptableObject
{
    [SerializeField]
    private List<InventoryItem> inventoryItems;

    [SerializeField]
    private int maxSize = 16; // Max size of the inventory

    public int Size => inventoryItems.Count;
    // Singleton instance
    private static InventorySO instance;
    public static InventorySO Instance => instance ?? (instance = Resources.Load<InventorySO>("Player Inventory"));

    public void Initialize()
    {
        inventoryItems = new List<InventoryItem>();
        for (int i = 0; i < maxSize; i++)
        {
            inventoryItems.Add(GetEmptyItem());
        }
    }

    public void AddItem(InventoryItem item)
    {
        if (Size < maxSize)
        {
            inventoryItems.Add(item);
        }
        else
        {
            Debug.LogWarning("Inventory is at max size.");
        }
    }

    public void RemoveItemAt(int index)
    {
        if (index >= 0 && index < Size)
        {
            inventoryItems.RemoveAt(index);
        }
        else
        {
            Debug.LogError("Invalid index to remove item.");
        }
    }

    public Dictionary<int, InventoryItem> GetCurrentInventoryState()
    {
        Dictionary<int, InventoryItem> returnValue = new Dictionary<int, InventoryItem>();
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].IsEmpty)
                continue;
            returnValue[i] = inventoryItems[i];

        }
        return returnValue;
    }

    internal InventoryItem GetItemAt(int itemIndex)
    {
        int itemCount = inventoryItems.Count;
        return inventoryItems[itemIndex % itemCount];
    }

    private InventoryItem GetEmptyItem()
        => new InventoryItem
        {
            Item = null,
        };
    public void RemoveItem(InventoryItem item)
    {
        inventoryItems.Remove(item);
    }
}
