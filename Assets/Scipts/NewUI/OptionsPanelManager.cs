using UnityEngine;

public class OptionsPanelManager : MonoBehaviour
{
    public GameObject optionsPanel; // Reference to the options panel GameObject
    public GameObject[] optionBorders; // Array of GameObjects representing the borders for each option
    public GameObject slotsPanel; // Reference to the slots panel GameObject

    private int selectedIndex = 0; // Index of the currently selected option
    private bool isOptionsPanelActive = false; // Flag to track if the options panel is active

    void Start()
    {
        // Disable all option borders at the start
        foreach (var border in optionBorders)
        {
            border.SetActive(false);
        }
    }

    // Method to toggle the options panel (open or close)
    public void ToggleOptionsPanel(bool open)
    {
        if (slotsPanel.activeSelf)
        {
            // Check if the options panel is already active and trying to be opened again
            if (isOptionsPanelActive && open)
            {
                return; // Exit the method without further action
            }

            optionsPanel.SetActive(open); // Set the options panel GameObject to active or inactive based on the 'open' parameter
            if (open)
            {
                isOptionsPanelActive = true; // Set the flag to indicate that the options panel is now active
                SelectOption(0); // Select the first option when opening the panel
            }
            else
            {
                isOptionsPanelActive = false; // Set the flag to indicate that the options panel is now inactive
                ResetOptions(); // Reset options when closing the panel
            }
        }
    }

    void Update()
    {
        // Check for input to open the options panel (only if it's not already active)
        if (!isOptionsPanelActive && Input.GetKeyDown(KeyCode.C))
        {
            ToggleOptionsPanel(true);
        }

        // Check for input to close the options panel
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            ToggleOptionsPanel(false);
        }

        // Check for input to scroll through options (if the options panel is active)
        if (isOptionsPanelActive)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                // Decrement the selected index and loop back to the last option if it goes below 0
                if (selectedIndex == 0)
                {
                    SelectOption(optionBorders.Length - 1);
                }
                else
                {
                    SelectOption(selectedIndex - 1);
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                // Increment the selected index and loop back to the first option if it exceeds the maximum index
                if (selectedIndex == optionBorders.Length - 1)
                {
                    SelectOption(0);
                }
                else
                {
                    SelectOption(selectedIndex + 1);
                }
            }
        }
    }

    // Method to select an option by index
    void SelectOption(int index)
    {
        // Remove the border highlight from the previously selected option
        optionBorders[selectedIndex].SetActive(false);

        // Set the new selected index (clamping within the bounds of the options array)
        selectedIndex = Mathf.Clamp(index, 0, optionBorders.Length - 1);

        // Highlight the selected option with a border
        optionBorders[selectedIndex].SetActive(true);
    }

    // Method to reset the selected index and disable all option borders
    public void ResetOptions()
    {
        selectedIndex = 0;
        foreach (var border in optionBorders)
        {
            border.SetActive(false);
        }
    }
}
