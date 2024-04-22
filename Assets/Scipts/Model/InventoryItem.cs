using Inventory.Model;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/InventoryItem")]
public class InventoryItem : ScriptableObject, IUsable
{
    public ItemSO Item;

    public bool IsEmpty => Item == null;
    public string itemName => Item?.Name;
    public Sprite itemIcon => Item?.ItemImage;

    // Implement Use() from IUsable
    public virtual void Use()
    {
        // Default behavior for using the item
        Debug.Log($"Using {itemName}.");
    }

    public virtual void Ammo()
    {
        Debug.Log($"Using {itemName}.");
    }
}