using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public InventoryItem requiredKey;

    private bool isOpen = false;
    public PlayerController playerController;

    public GameObject Doorsound;

    public void TryOpenDoor(GoblinKey key)
    {
        if (!isOpen && key == requiredKey && key is GoblinKey)
        {
            OpenDoor();
            key.RemoveFromInventory();
        }
        else if (key == null)
        {
            Debug.Log("No key selected.");
        }
        else if (key != requiredKey)
        {
            Debug.Log("Incorrect key.");
        }
    }

    private void OpenDoor()
    {
        // For now, we'll just make the door disappear.
        gameObject.SetActive(false);
        isOpen = true;
        Debug.Log("Door opened!");
        Instantiate(Doorsound);
    }

    public void Interact()
    {
        if (playerController != null)
        {
            playerController.ToggleInventory(); // Call ToggleInventory method on PlayerController
        }
        else
        {
            Debug.LogWarning("PlayerController reference is not assigned to the door.");
        }
    }

}