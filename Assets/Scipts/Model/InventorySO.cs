using Inventory.Model;
using System;
using System.Collections;
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

    public void Initialize()
    {
        inventoryItems = new List<InventoryItem>();
        for (int i = 0; i < maxSize; i++)
        {
            inventoryItems.Add(InventoryItem.GetEmptyItem());
        }
    }

    public void AddItem(ItemSO item)
    {
        // Increase the size of the inventory if it's not at max size
        if (Size < maxSize)
        {
            inventoryItems.Add(new InventoryItem { Item = item });
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
}

[Serializable]
public struct InventoryItem
{
    public ItemSO Item;

    public bool IsEmpty => Item == null;

    public InventoryItem ChangeItem(int newQuantity)
    {
        return new InventoryItem
        {
            Item = this.Item,
        };
    }

    public static InventoryItem GetEmptyItem()
        => new InventoryItem
        {
            Item = null,
        };
}