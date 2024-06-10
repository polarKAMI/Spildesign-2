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

    public PlayerMovement PlayerMovement;

    public Animator animator;
    public Image ammoBar5;
    public Image ammoBar10;

    public GameObject reloadobject;
    public GameObject Firesoundobject;
    public GameObject addammosound;
    public GameObject emptyclip;
    public PlayerJump playerJump;

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
        currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo); // Ensure currentAmmo stays within bounds
        UpdateAmmoUI();
        Debug.Log(currentAmmo);

        if (currentAmmo < 5)
        {
            Instantiate(addammosound);
        }

        if (currentAmmo >= 5)
        {
            Instantiate(reloadobject);
        }

    }

    IEnumerator StopShootingCoroutine()
    {

        // Wait for the duration of the shooting animation
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(1).length);
        // Reset shooting flag after animation is finished
        animator.SetBool("IsShooting", false);

        yield return new WaitForSeconds(1f);
        PlayerMovement.enabled = true;
        IsShooting = false;




    }

    public void Shooting()
    {
        if (!IsShooting && !playerJump.isJumping && !playerJump.isFalling) // Check if shooting animation is already triggered
        {
            if (currentAmmo < 5)
            {
                Debug.Log("Cannot shoot: Ammo is not at max");
                Instantiate(emptyclip);
                return;
            }

            Instantiate(Firesoundobject);
            Instantiate(Projectile, fireposition.position, fireposition.rotation);
            currentAmmo -= 5;
            UpdateAmmoUI();
            animator.SetBool("IsShooting", true); // Start shooting animation

            Debug.Log("Disabling PlayerMovement");
            PlayerMovement.enabled = false; // Disable PlayerMovement
            Debug.Log($"PlayerMovement enabled state: {PlayerMovement.enabled}");


            // Apply pushback force based on the direction
            Vector3 localScale = transform.localScale;
            Vector2 pushbackDirection = localScale.x < 0 ? Vector2.left : Vector2.right;
            Debug.Log($"Applying pushback force: {pushbackDirection * pushbackForce}");
            rb.AddForce(pushbackDirection * pushbackForce, ForceMode2D.Impulse);


            StartCoroutine(StopShootingCoroutine()); // Start the coroutine to stop shooting animation
            IsShooting = true; // Set shooting flag to true

        }
    }

    private void UpdateAmmoUI()
    {
        if (ammoBar5 != null && ammoBar10 != null)
        {
            // Update the first ammo bar (up to 5 ammo)
            ammoBar5.fillAmount = Mathf.Clamp01((float)currentAmmo / 5);
            ammoBar5.color = currentAmmo >= 5 ? Color.green : Color.white;

            // Update the second ammo bar (up to 10 ammo)
            ammoBar10.fillAmount = Mathf.Clamp01((float)(currentAmmo - 5) / 5);
            ammoBar10.color = currentAmmo == maxAmmo ? Color.green : Color.white;
        }
    }
}