using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagescript : MonoBehaviour
{
    public Health health;
    
   

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            health.TakeDamage(1);
        }
    }

    
}
