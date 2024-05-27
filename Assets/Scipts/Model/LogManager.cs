using System.Collections.Generic;
using UnityEngine;

public class LogManager : MonoBehaviour
{
    public List<LogSO> notesLogs = new List<LogSO>();
    public List<LogSO> itemsLogs = new List<LogSO>();
    public List<LogSO> entitiesLogs = new List<LogSO>();
    public List<LogSO> federationLogs = new List<LogSO>();
    public List<LogSO> miscLogs = new List<LogSO>();
    public List<LogSO> allLogs;
    public List<LogSO> startLogs;
    private void Start()
    {
        foreach (var log in allLogs) 
        { 
        log.ResetCollected();
        }
        foreach (var log in startLogs)
        {
            AddLog(log);
        }
    }

    public static void AddLog(LogSO log)
    {
        LogManager instance = FindObjectOfType<LogManager>(); // Find instance of LogManager
        if (instance == null)
        {
            Debug.LogError("LogManager instance not found!");
            return;
        }
        if (log.isNote)
            instance.notesLogs.Add(log);
        else if (log.isItem)
            instance.itemsLogs.Add(log);
        else if (log.isEntity)
            instance.entitiesLogs.Add(log);
        else if (log.isFederation)
            instance.federationLogs.Add(log);
        else if (log.isMisc)
            instance.miscLogs.Add(log);
    }
    public static List<LogSO> GetLogsForCategory(int categoryIndex)
    {
        LogManager instance = FindObjectOfType<LogManager>(); // Find instance of LogManager
        if (instance == null)
        {
            Debug.LogError("LogManager instance not found!");
            return null;
        }

        switch (categoryIndex)
        {
            case 0: return instance.notesLogs;
            case 1: return instance.itemsLogs;
            case 2: return instance.entitiesLogs;
            case 3: return instance.federationLogs;
            case 4: return instance.miscLogs;
            default: return new List<LogSO>();
        }
    }
}