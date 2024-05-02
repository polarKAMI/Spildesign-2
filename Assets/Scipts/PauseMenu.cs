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

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
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
        }
        else
        {
            playerMovement.EnableMovement();
            GlobalInputMapping.SetActiveInputMappings(GlobalInputMapping.inGameInputMapping);
            Resume();
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

   public void Logopen()
    {
        pauseMenu.SetActive(false);
    }
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
}

