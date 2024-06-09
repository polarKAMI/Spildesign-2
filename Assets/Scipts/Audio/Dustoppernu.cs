using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dustoppernu : MonoBehaviour
{
    public GameObject slotControllerObject;  // Public GameObject to assign in the Inspector
    private SlotController slotController;   // Private variable to hold the SlotController script
    public AudioSource audioSource;

    // Start is called before the first frame update
    public void Start()
    {
        if (slotControllerObject != null)
        {
            // Get the SlotController component from the assigned GameObject
            slotController = slotControllerObject.GetComponent<SlotController>();
            if (slotController == null)
            {
                Debug.LogError("SlotController component is missing on the assigned GameObject.");
            }
        }
        else
        {
            Debug.LogError("slotControllerObject is not assigned in the Inspector.");
        }
    }

    // Update is called once per frame
    public void Update()
    {
        if (slotController != null && slotController.endsongye == true)
        {
            audioSource.Stop();
        }
    }
}
