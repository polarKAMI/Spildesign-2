using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lygtehealth : MonoBehaviour
{
    public int Maxhealth = 30;
    public int currentenemyhealth;

    public GameObject lygtedudsound;
    public GameObject lygteavsound;
    public GameObject triangle;
    public Lygtemanden lygtemanden;
    public Lygtemandenmovement lygtemandenmovement;
    public Animator animator;

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

        if (currentenemyhealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        lygtemanden.isChasing = false;
        if (!log.Collected)
        {
            // Add the log to the LogManager
            LogManager.AddLog(log);
            // Set the collected flag to true
            log.Collected = true;
            notificationManager.ShowNotification("new log;");
        }

        lygtemanden.enabled = false;
        lygtemandenmovement.enabled = false;
        triangle.SetActive(false);
        animator.SetBool("IsDead", true);
        animator.SetBool("HasCaught", false);
        animator.SetBool("IsSpawned", false);

        // Start the UnDie coroutine
        StartCoroutine(UnDie());
    }

    private IEnumerator UnDie()
    {
        // Wait for 30 seconds
        yield return new WaitForSeconds(30);

        // Re-enable the components and set the necessary states
        lygtemanden.enabled = true;
        lygtemandenmovement.enabled = true;
        triangle.SetActive(true);
        animator.SetBool("IsDead", false);

        // Reset health or any other state if necessary
        currentenemyhealth = Maxhealth;
    }
}