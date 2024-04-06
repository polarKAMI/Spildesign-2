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
    public int Size { get; private set; } = 16;

    public void Initialize()
    {
        inventoryItems = new List<InventoryItem>();
        for (int i = 0; i < Size; i++)
        {
            inventoryItems.Add(InventoryItem.GetEmptyItem());
        }
    }

    public void AddItem(ItemSO item)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].IsEmpty)
            {
                inventoryItems[i] = new InventoryItem
                {
                    Item = item

                };
            }
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
        return inventoryItems[itemIndex];
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