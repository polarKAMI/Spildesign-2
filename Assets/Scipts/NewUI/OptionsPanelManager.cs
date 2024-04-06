using UnityEngine;

public class OptionsPanelManager : MonoBehaviour
{
    public GameObject optionsPanel; // Reference to the options panel GameObject
    public GameObject slotsPanel; // Reference to the slots panel GameObject

    // Method to open the options panel
    public void OpenOptionsPanel()
    {
        if (slotsPanel.activeSelf)
        {
            optionsPanel.SetActive(true); // Set the options panel GameObject to active
        }
    }

    // Method to close the options panel
    public void CloseOptionsPanel()
    {
        if (slotsPanel.activeSelf)
        {
            optionsPanel.SetActive(false); // Set the options panel GameObject to inactive
        }
    }

    void Update()
    {
        // Check for input to open the options panel
        if (Input.GetKeyDown(KeyCode.C) && slotsPanel.activeSelf)
        {
            OpenOptionsPanel(); // Call the OpenOptionsPanel method
        }

        // Check for input to close the options panel
        if (Input.GetKeyDown(KeyCode.Backspace) && slotsPanel.activeSelf)
        {
            CloseOptionsPanel(); // Call the CloseOptionsPanel method
        }
    }
}