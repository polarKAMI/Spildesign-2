using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogMenu : MonoBehaviour
{
    [SerializeField] GameObject logMenu;
    [SerializeField] PauseMenu pauseMenu;
    [SerializeField] GameObject NSLog;
    [SerializeField] GameObject WildLifeLog;
    [SerializeField] GameObject GBLog;

    private void Start()
    {
        pauseMenu = FindObjectOfType<PauseMenu>();
    }
    public void Pause()
    {
        logMenu.SetActive(false);
        pauseMenu.Menu();
        Time.timeScale = 0;       
    }

    public void LogOpen()
    {
        logMenu.SetActive(true);
        GlobalInputMapping.SetActiveInputMappings(GlobalInputMapping.logInputMapping);
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
