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
    private int size = 0;

    public int Size
    {
        get { return Mathf.Min(size, 16); } // Ensure size doesn't exceed 16
        private set { size = value; }
    }

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
        int itemCount = inventoryItems.Count;
        return inventoryItems[itemIndex % itemCount];
    }

    public bool ContainsItem(ItemSO item)
    {
        foreach (var inventoryItem in inventoryItems)
        {
            if (!inventoryItem.IsEmpty && inventoryItem.Item == item)
            {
                return true; // Item already exists in the inventory
            }
        }
        return false; // Item does not exist in the inventory
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