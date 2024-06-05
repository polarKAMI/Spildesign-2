using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // Required for accessing UI components

public class MainMenu : MonoBehaviour
{
    public GameObject[] menuBorders;
    private int selectedIndex = 0;
    public bool controlsOpen = false;
    public GameObject controls;

    private void Start()
    {
        ResetOptions();
        SelectOption(0);
        GlobalInputMapping.SetActiveInputMappings(GlobalInputMapping.mainMenuInputMapping);
    }

    private void Update()
    {
        if (Input.GetKeyDown(GlobalInputMapping.activeInputMappings["Up"]))
        {
            if (controlsOpen)
            {
            }
            else
            {
                ChangeSelectedIndex(-1);
            }

        }
        else if (Input.GetKeyDown(GlobalInputMapping.activeInputMappings["Down"]))
        {
            if (controlsOpen)
            {
            }
            else
            {
                ChangeSelectedIndex(1);
            }

        }
        else if (Input.GetKeyDown(GlobalInputMapping.activeInputMappings["Select"]))
        {
            if (controlsOpen)
            {
            }
            else
            {
                MenuOptionSelect();
            }
            
        }
        else if (Input.GetKeyDown(GlobalInputMapping.activeInputMappings["Deselect"]))
        {
            if (controlsOpen)
            {
                CloseControls();
            }
            else
            {
               
            }
        }
    }
    // Method to start the game
    public void PlayGame()
    {  
        StartCoroutine(WaitAndLoadScene("Intro", 1f));
    }

    // Coroutine to wait for a specified amount of time before loading the scene
    private IEnumerator WaitAndLoadScene(string sceneName, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(sceneName);
    }

    public void MenuOptionSelect()
    {
        int selectedOption = selectedIndex; // Assuming selectedIndex holds the current selected option index

        switch (selectedOption)
        {
            case 0: // First option selected
                PlayGame();
                break;
            case 1: // Second option selected
                Controls();
                break;
            case 2: // Third option selected
                QuitGame();
                break;
        }
    }
    void SelectOption(int index)
    {
        // Remove the border highlight from the previously selected option
        menuBorders[index].SetActive(false);

        // Highlight the selected option with a border
        menuBorders[index].SetActive(true);
    }
    public void ResetOptions()
    {
        foreach (var border in menuBorders)
        {
            border.SetActive(false);
        }

        selectedIndex = 0;
    }
    public void ChangeSelectedIndex(int changeAmount)
    {
        int newIndex = selectedIndex + changeAmount;

        // Loop back to the last option if it goes below 0
        if (newIndex < 0)
        {
            newIndex = menuBorders.Length - 1;
        }
        // Loop back to the first option if it exceeds the maximum index
        else if (newIndex >= menuBorders.Length)
        {
            newIndex = 0;
        }

        // Deactivate the previously selected border
        menuBorders[selectedIndex].SetActive(false);

        // Activate the new selected border
        menuBorders[newIndex].SetActive(true);

        // Update the selected index
        selectedIndex = newIndex;
    }

    // Method to quit the game
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game closed");
    }

    public void Controls()
    {
        controlsOpen = true;
        controls.SetActive(true);
    }

    public void CloseControls()
    {
        controlsOpen = false;
        controls.SetActive(false);
    }
}
