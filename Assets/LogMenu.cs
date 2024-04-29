using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogMenu : MonoBehaviour
{
    [SerializeField] GameObject logMenu;
    [SerializeField] GameObject pauseMenu;

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        logMenu.SetActive(false);
    }
}
