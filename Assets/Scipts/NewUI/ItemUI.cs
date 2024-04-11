using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public int index = 0;
    public Image itemImage;
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

    // Method to update the item image
    public void UpdateItem(Sprite sprite)
    {
        itemImage.sprite = sprite;
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

    // Other methods and properties...
}