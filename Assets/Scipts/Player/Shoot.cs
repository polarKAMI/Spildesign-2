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
    public Animator animator;
    public Image ammoBar;

    public GameObject reloadobject;
    public GameObject Firesoundobject;

   private bool IsShooting = false; // Define IsShooting variable

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
        UpdateAmmoUI();
    }

    void Startscript()
    {
        PlayerMovement.enabled = true;
    }

    IEnumerator StopShootingCoroutine()
    {
       
        // Wait for the duration of the shooting animation
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(1).length);
        // Reset shooting flag after animation is finished
        animator.SetBool("IsShooting", false);
        IsShooting = false;
    }

    public void Shooting()
    {
        if (!IsShooting) // Check if shooting animation is already triggered
        {
            Instantiate(Firesoundobject);
            Instantiate(Projectile, fireposition.position, fireposition.rotation);
            currentAmmo -= 5;
            UpdateAmmoUI();
            animator.SetBool("IsShooting", true); // Start shooting animation
            PlayerMovement.enabled = false;

            // Apply pushback force based on the direction
            Vector3 localScale = transform.localScale;
            Vector2 pushbackDirection = localScale.x > 0 ? Vector2.left : Vector2.right;
            rb.AddForce(pushbackDirection * pushbackForce, ForceMode2D.Impulse);

            Instantiate(reloadobject);

            StartCoroutine(StopShootingCoroutine()); // Start the coroutine to stop shooting animation
            IsShooting = true; // Set shooting flag to true
        }
    }

    private void UpdateAmmoUI()
    {
        if (ammoBar != null)
        {
            ammoBar.fillAmount = (float)currentAmmo / maxAmmo;
            ammoBar.color = currentAmmo == maxAmmo ? Color.red : Color.white;
        }
    }
}