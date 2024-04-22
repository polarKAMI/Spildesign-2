using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public int index = 0;
    public Image itemImage;
    public InventoryItem inventoryItem; // Store the reference to the InventoryItem
    private Transform cachedTransform; // Store the transform reference

    private void Awake()
    {
        Image itemImage = transform.Find("Item").GetComponent<Image>();
        cachedTransform = transform; // Cache the transform reference
    }

    // Method to set the index of the item
    public void SetIndex(int newIndex)
    {
        index = newIndex;
    }

    // Method to get the index of the item
    public int GetIndex()
    {
        return index;
    }

    public void UpdateItem(InventoryItem inventoryItem)
    {
        this.inventoryItem = inventoryItem; // Store the reference
        itemImage.sprite = inventoryItem.Item?.ItemImage;
    }

    // Method to get the cached transform
    public Transform GetCachedTransform()
    {
        return cachedTransform;
    }

    public void UpdateCachedTransform()
    {
        cachedTransform = transform;
    }

    public bool IsHighlighted()
    {
        return Mathf.Approximately(cachedTransform.localPosition.z, 0f);
    }
}