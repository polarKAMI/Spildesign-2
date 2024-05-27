using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lygteidletrigger : MonoBehaviour
{
    private Lygtemandenmovement parentScript;

    void Start()
    {
        // Find the parent Lygtemandenmovement script
        parentScript = GetComponentInParent<Lygtemandenmovement>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            parentScript.OnPlayerEnterTrigger();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            parentScript.OnPlayerExitTrigger();
        }
    }
}
