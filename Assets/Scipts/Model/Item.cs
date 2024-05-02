using UnityEngine;
using System.Collections;
public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] private InventoryItem inventoryItem;
    [SerializeField] private InventorySO inventoryData; // Reference to the InventorySO
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float pickupDuration = 0.3f;

    public InventoryItem InventoryItem => inventoryItem;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = inventoryItem.itemIcon;
    }

    public void Interact()
    {
        PickUp();
    }

    private void PickUp()
    {
        // Add the item to the inventory data
        inventoryData.AddItem(inventoryItem);

        StartCoroutine(AnimatePickup());
    }

    private IEnumerator AnimatePickup()
    {
        // Play pickup sound
        if (audioSource != null)
            audioSource.Play();

        // Disable collider to prevent further interactions
        GetComponent<Collider2D>().enabled = false;

        // Scale down the item
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float currentTime = 0;
        while (currentTime < pickupDuration)
        {
            currentTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, currentTime / pickupDuration);
            yield return null;
        }

        // Destroy the item after pickup animation
        Destroy(gameObject);
    }
}