using UnityEngine;

public class SlotController : MonoBehaviour
{
    public ItemSlot[] itemSlots; // Array to store references to all item slots
    public GameObject door;

    private bool[] isSlotMatched; // Array to store the match status of each slot

    private void Start()
    {
        // Initialize the array to store the match status of each slot
        isSlotMatched = new bool[itemSlots.Length];
    }

    private void Update()
    {
        // Check if all item slots have a match
        bool allSlotsMatched = true;
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (!itemSlots[i].isMatch)
            {
                allSlotsMatched = false;
                break;
            }
        }

        // If all slots match, execute the OpenDoor function
        if (allSlotsMatched)
        {
            OpenDoor();
            foreach (var slot in itemSlots)
            {
                slot.SetDoorOpened(true);
            }
        }
    }

    private void OpenDoor()
    {
        // Execute the logic to open the door
        Debug.Log("All slots matched! Opening the door...");

        // Disable the door GameObject
        if (door != null)
        {
            door.SetActive(false);
        }
        else
        {
            Debug.LogError("Door GameObject reference is not set.");
        }
    }
}