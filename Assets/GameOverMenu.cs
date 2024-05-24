using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] GameObject GameOverScreen;
    private PlayerMovement playerMovement;
    [SerializeField] private InventorySO InventorySO;
    public GameObject[] menuBorders;
    private int selectedIndex = 0;
    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();

        foreach (var border in menuBorders)
        {
            border.SetActive(false);
        }
    }
    public void GameOver()
    {
        if (!GameOverScreen.activeSelf)
        {
            if (playerMovement != null)
            {
                playerMovement.DisableMovement();
            }
            if (InventorySO != null)
            {
                InventorySO.WipeInventory();
            }
            ResetAllLogs();
            GameOverScreen.SetActive(true);
            Time.timeScale = 0;
            GlobalInputMapping.SetActiveInputMappings(GlobalInputMapping.GameOverInputMapping);
            SelectOption(0);
        }
    }
    void SelectOption(int index)
    {
        // Remove the border highlight from the previously selected option
        menuBorders[index].SetActive(false);

        // Highlight the selected option with a border
        menuBorders[index].SetActive(true);
    }
    public void MenuOptionSelect()
    {
        int selectedOption = selectedIndex; // Assuming selectedIndex holds the current selected option index

        switch (selectedOption)
        {
            case 0: 
                Restart();
                break;
            case 1:
                MainMenu();
                break;
            default:
                Debug.LogError("Invalid option selected!");
                break;
        }
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
    public void Restart()
    {
        ResetAllLogs();
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
    private void ResetAllLogs()
    {
        LogSO[] allLogs = Resources.LoadAll<LogSO>("");
        foreach (LogSO log in allLogs)
        {
            log.ResetCollected();
            Debug.Log($"{log.Name} collected reset to {log.Collected}"); // Add logging to verify
        }
    }
}
