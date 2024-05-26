using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagescript : MonoBehaviour
{
    public Health health;

    public bool PlayerHasBeenDamaged = false;

    public GameObject Bitesound;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Instantiate(Bitesound);
            health.TakeDamage(1);
           PlayerHasBeenDamaged=true;
            Debug.Log("Player has taken damage from enemy");
        }
    }



    public void ResetDamageStatus()
    {
        PlayerHasBeenDamaged = false;
        Debug.Log("Damage wait has been reset");
    }


}
