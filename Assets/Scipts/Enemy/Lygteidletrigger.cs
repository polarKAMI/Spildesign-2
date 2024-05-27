using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lygteidletrigger : MonoBehaviour
{
    private Lygtemandenmovement parentScript;

    private bool hasPlayedAudio = false;

    void Start()
    {
        // Find the parent Lygtemandenmovement script
        parentScript = GetComponentInParent<Lygtemandenmovement>();

       
       
    }

    

   


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && parentScript.funderundedone)
        {
            parentScript.OnPlayerEnterTrigger();
            parentScript.isPlayerInTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && parentScript.funderundedone)
        {
            parentScript.OnPlayerExitTrigger();
            hasPlayedAudio = false;
            parentScript.isPlayerInTrigger = false;
        }
    }
}
