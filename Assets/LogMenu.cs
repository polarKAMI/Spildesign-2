using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogMenu : MonoBehaviour
{
    [SerializeField] GameObject logMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject NSLog;
    [SerializeField] GameObject WildLifeLog;
    [SerializeField] GameObject GBLog;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            logMenu.SetActive(false);
            WildLifeLog.SetActive(false);
        }
    }
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        logMenu.SetActive(false);
    }

    public void LogOpen()
    {
        logMenu.SetActive(false);
    }
    public void NSReturn()
    {
        NSLog.SetActive(false);
    }

    public void GbReturn()
    {
        GBLog.SetActive(false);
    }
    public void LogReturn() 
    {
        WildLifeLog.SetActive(false);
        logMenu.SetActive(true);
    }
}
