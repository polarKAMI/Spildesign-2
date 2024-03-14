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
    }
}