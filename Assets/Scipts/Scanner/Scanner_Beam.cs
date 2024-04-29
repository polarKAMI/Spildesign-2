using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner_Beam : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ScanObject"))
        {
            Debug.Log("Scan ya later animator");
        }
        if (other.CompareTag("ScanObject2"))
        {
            Debug.Log("This is getting out of hand, now there are 2 of them");
        }
    }
}