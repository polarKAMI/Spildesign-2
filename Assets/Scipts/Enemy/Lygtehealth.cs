using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lygtehealth : MonoBehaviour
{
    public int Maxhealth = 30;
    public int currentenemyhealth;

    public GameObject lygtedudsound;

    public GameObject lygteavsound;


    public LogSO log;
    public NotificationManager notificationManager;

    void Start()
    {
        currentenemyhealth = Maxhealth;
    }


    public void Takedamage(int amount)
    {
        currentenemyhealth -= amount;

        if (currentenemyhealth < Maxhealth)
        {
            Instantiate(lygtedudsound);
        }
        else
        {
            Instantiate(lygteavsound);
        }


        currentenemyhealth -= amount;
        Debug.Log("Nisse tog squ skade");




    }


    public void Die()
    {
        if (!log.Collected)
        {
            // Add the log to the LogManager
            LogManager.AddLog(log);
            // Set the collected flag to true
            log.Collected = true;
            notificationManager.ShowNotification("new log;");
        }
        Destroy(gameObject); //kun indtil animationer er klar
    }
}

