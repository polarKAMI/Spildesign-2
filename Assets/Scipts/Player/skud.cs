using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skud : MonoBehaviour
{
    public enemyhealth health;
    private void Start()
    {
       
        Invoke("DestroyObject", 0.5f);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            health.Takedamage(5);

            if (health.currentenemyhealth <= 0)
            {
                Destroy(collision.gameObject);
                health.currentenemyhealth = health.Maxhealth;
            }
        }
    }

    private void DestroyObject()
    {
        
        Destroy(gameObject);
    }
}
