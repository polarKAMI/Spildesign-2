using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shoot : MonoBehaviour
{
    public Transform fireposition;
    public GameObject Projectile;
    public int currentAmmo = 10;
    public int maxAmmo = 10;
    public float pushbackForce = 150f;
    public Rigidbody2D rb;

    public MonoBehaviour PlayerMovement;
    public PlayerMovement playermovement;

    private bool canAttack = true;

    public Image ammoBar;
    


    public GameObject reloadobject;
    public GameObject Firesoundobject;

    private void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found. This script requires a Rigidbody2D component.");
        }

        UpdateAmmoUI();
    }
    public void AddAmmo(int amount)
    {
       
        currentAmmo += amount;
        currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo); // Ensure currentAmmo stays within bounds
        UpdateAmmoUI();
        Debug.Log(currentAmmo);
    }
    void Startscript()
    {
        PlayerMovement.enabled = true;

    }
    void attack()
    {

        canAttack = true;
        Debug.Log("Can attack");
    }
    public void Shooting()
    {

        if (currentAmmo != maxAmmo)
        {
            Debug.Log("Cannot shoot: Ammo is not at max");
            return;
        }


        Instantiate(Firesoundobject);

        Instantiate(Projectile, fireposition.position, fireposition.rotation);
        currentAmmo -= 5;
        UpdateAmmoUI();



        PlayerMovement.enabled = false;

        // Get the local scale of the GameObject
        Vector3 localScale = transform.localScale;

        // Check if Scale X is greater than 0
        if (localScale.x > 0)
        {
            // Apply impulse to push the GameObject to the left
            rb.AddForce(Vector2.left * pushbackForce, ForceMode2D.Impulse);
            Invoke("Startscript", 0.3f);
        }
        // Check if Scale X is less than 0
        else if (localScale.x < 0)
        {
            // Apply impulse to push the GameObject to the right
            rb.AddForce(Vector2.right * pushbackForce, ForceMode2D.Impulse);
            Invoke("Startscript", 0.3f);
        }

        canAttack = false;


        Invoke("attack", 5f);
    }


    private void UpdateAmmoUI()
    {
        if (ammoBar != null)
        {
            ammoBar.fillAmount = (float)currentAmmo / maxAmmo;

        }




       if (currentAmmo == maxAmmo)
        {
            ammoBar.color = Color.red;
            Instantiate(reloadobject);
        }

       if(currentAmmo < maxAmmo)
        {
            ammoBar.color = Color.white;
        }
    }
}
