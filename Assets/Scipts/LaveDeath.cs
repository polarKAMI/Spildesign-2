using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaveDeath : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            Destroy(other.gameObject);
        Debug.Log("if (You.died == SkillDiff)");
    }
}