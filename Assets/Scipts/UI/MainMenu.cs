using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // Required for accessing UI components

public class MainMenu : MonoBehaviour
{
    public GameObject controlsPanel;  // Reference to the controls UI panel
    public Button playButton;         // Reference to the Play button
    public Button controlsButton;     // Reference to the Controls button
    public Button quitButton;         // Reference to the Quit button
    public Button exitControlsButton; // Reference to the Exit Controls button
    public GameObject mosefundtext;

    public GameObject mosefundbuttonsound;

    // Method to start the game
    public void PlayGame()
    {
        Instantiate(mosefundbuttonsound);
        StartCoroutine(WaitAndLoadScene("Intro", 1f));
    }

    // Coroutine to wait for a specified amount of time before loading the scene
    private IEnumerator WaitAndLoadScene(string sceneName, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(sceneName);
    }

    // Method to show the controls panel
    public void ShowControls()
    {
        Instantiate(mosefundbuttonsound);
        controlsPanel.SetActive(true);  // Show the controls panel
        playButton.gameObject.SetActive(false);     // Disable the Play button
        controlsButton.gameObject.SetActive(false); // Disable the Controls button
        quitButton.gameObject.SetActive(false);     // Disable the Quit button
        exitControlsButton.gameObject.SetActive(true); // Enable the Exit Controls button
        mosefundtext.gameObject.SetActive(false);
       
    }

    // Method to hide the controls panel
    public void HideControls()
    {
        Instantiate(mosefundbuttonsound);
        controlsPanel.SetActive(false);  // Hide the controls panel
        playButton.gameObject.SetActive(true);     // Enable the Play button
        controlsButton.gameObject.SetActive(true); // Enable the Controls button
        quitButton.gameObject.SetActive(true);     // Enable the Quit button
        exitControlsButton.gameObject.SetActive(false); // Disable the Exit Controls button
        mosefundtext.gameObject.SetActive(true);
        
    }

    // Method to quit the game
    public void QuitGame()
    {
        Instantiate(mosefundbuttonsound);
        Application.Quit();
        Debug.Log("Game closed");
    }
}
