using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    private PlayerMovement playerMovement;
    public GameObject[] menuBorders;
    public InventoryUIManager inventoryManager;
    private OptionsPanelManager optionsPanelManager;
    private LogMenu logMenu;
    private LadderMovement ladderMovement;

    private int selectedIndex = 0;



    private AudioSource audioSource;
    private AudioClip indexChangeClip;
    private AudioClip indexEnterClip;

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        inventoryManager = FindObjectOfType<InventoryUIManager>();
        optionsPanelManager = FindObjectOfType<OptionsPanelManager>();
        ladderMovement = FindObjectOfType<LadderMovement>();
        logMenu = FindObjectOfType<LogMenu>();
        
        foreach (var border in menuBorders)
        {
            border.SetActive(false);
        }
    }
    public void Menu()
    {
        if (!pauseMenu.activeSelf)
        {
            if (playerMovement != null)
            {
                playerMovement.DisableMovement();
            }
            GlobalInputMapping.SetActiveInputMappings(GlobalInputMapping.menuInputMapping);
            Pause();
            ResetOptions();
            SelectOption(0);
        }
        else
        {
            Resume();
        }

    }

    public void MenuOptionSelect()
    {
        int selectedOption = selectedIndex; // Assuming selectedIndex holds the current selected option index

        switch (selectedOption)
        {
            case 0: // First option selected
                Resume();
               
                break;
            case 1: // Second option selected
                Logopen();
               
                break;
            case 2: // Third option selected
                Restart();
               
                break;
            case 3:
                MainMenu();
               
                break;
            default:
                Debug.LogError("Invalid option selected!");
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

    public void Pause()
    {
        PlayIndexEnterSound();
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void MainMenu()
    {
        PlayIndexEnterSound();
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

   public void Logopen()
    {
        PlayIndexEnterSound();
        pauseMenu.SetActive(false);
        ResetOptions();
        logMenu.LogOpen();
    }
    public void Resume()
    {
        PlayIndexEnterSound();
        pauseMenu.SetActive(false);
        Time.timeScale = 1;

        if (optionsPanelManager.isOptionsPanelActive)
        {
            GlobalInputMapping.SetActiveInputMappings(GlobalInputMapping.optionsInputMapping);
        }
        else if (inventoryManager.isOpen)
        {
            GlobalInputMapping.SetActiveInputMappings(GlobalInputMapping.inventoryInputMapping);
        }
        else if (ladderMovement.isClimbing)
        {
            GlobalInputMapping.SetActiveInputMappings(GlobalInputMapping.climbInputMapping);
        }
        else
        {
            GlobalInputMapping.SetActiveInputMappings(GlobalInputMapping.inGameInputMapping);
            playerMovement.EnableMovement();
        }
    }

    public void Restart()
    {
        PlayIndexEnterSound();
        GlobalInputMapping.SetActiveInputMappings(GlobalInputMapping.inGameInputMapping);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void ChangeSelectedIndex(int changeAmount)
    {
        int newIndex = selectedIndex + changeAmount;

        // Loop back to the last option if it goes below 0
        if (newIndex < 0)
        {
            newIndex = menuBorders.Length - 1;
            PlayIndexChangeSound();
        }
        // Loop back to the first option if it exceeds the maximum index
        else if (newIndex >= menuBorders.Length)
        {
            newIndex = 0;
            PlayIndexChangeSound();
        }

        // Deactivate the previously selected border
        menuBorders[selectedIndex].SetActive(false);

        // Activate the new selected border
        menuBorders[newIndex].SetActive(true);

        // Update the selected index
        selectedIndex = newIndex;
    }


    private void PlayIndexChangeSound()
    {
        if (audioSource != null && indexChangeClip != null)
        {
            audioSource.PlayOneShot(indexChangeClip);
        }
    }


    private void PlayIndexEnterSound()
    {
        if (audioSource != null && indexEnterClip != null)
        {
            audioSource.PlayOneShot(indexEnterClip);
        }
    }
}

