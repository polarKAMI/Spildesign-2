using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerattack2 : MonoBehaviour
{
    public Transform fireposition;
    public GameObject Projectile;
    public int currentAmmo = 10;
    public int maxAmmo = 10;
    public float pushbackForce = 150f;
    public Rigidbody2D rb;

    public MonoBehaviour PlayerMovement;
    public PlayerMovement playermovement;


    

    void Update()
    {
        
        
        if (Input.GetKeyDown(KeyCode.E) & (currentAmmo > 0))
        {
            Instantiate(Projectile, fireposition.position, fireposition.rotation);
            currentAmmo--;

            PlayerMovement.enabled = false;

            pushback();

            if (currentAmmo == 0)
            {
                Invoke("Reload", 5f);
            }
        }

       
    }

    void Reload()
    {

        currentAmmo = maxAmmo;
    }


    void pushback()
    {


        if (playermovement.isFacingRight == true)
        {
            Vector2 direction = (transform.position + Projectile.transform.position).normalized;
            // hvis - s[ venstre og hvis + s[ h'jre
            // Apply pushback force in the direction away from projectile

            rb.AddForce(direction * pushbackForce, ForceMode2D.Impulse);

            Invoke("Startscript", 0.3f);
        }

        if (playermovement.isFacingRight == false)
        {
            Vector2 direction = (transform.position - Projectile.transform.position).normalized;
            // hvis - s[ venstre og hvis + s[ h'jre
            // Apply pushback force in the direction away from projectile

            rb.AddForce(direction * pushbackForce, ForceMode2D.Impulse);

            Invoke("Startscript", 0.3f);
        }

    }

        
    void Startscript()
    {
        PlayerMovement.enabled = true;
    }
    

   
}
