using UnityEngine;

public class Door : MonoBehaviour
{
    public InventoryItem requiredKey;

    private bool isOpen = false;

    public void TryOpenDoor(GoblinKey key)
    {
        if (!isOpen && key == requiredKey && key is GoblinKey)
        {
            OpenDoor();
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
    }
}