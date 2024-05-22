using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int MaxHealth = 10;
    public int currentHealth;

    private SpriteRenderer spriteRenderer;
    private Coroutine flickerCoroutine;
    private bool isFlickering = false;

    public GameObject DamageOverlay;

    void Start()
    {
        currentHealth = MaxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateDamageOverlay();
    }


    public void UpdateDamageOverlay()
    {
        if (DamageOverlay != null)
        {
            Image overlayImage = DamageOverlay.GetComponent<Image>();
            if (overlayImage != null)
            {
                float alpha = 1f - (float)currentHealth / MaxHealth;
                overlayImage.color = new Color(overlayImage.color.r, overlayImage.color.g, overlayImage.color.b, alpha);
                Debug.Log($"Updated DamageOverlay: Health={currentHealth}, Alpha={alpha}");
            }
        }
    }


    public void AddHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth); // Ensure health stays within bounds
        Debug.Log(currentHealth);
        UpdateDamageOverlay();

    }

    public void TakeDamage(int amount)
    {
        if (!isFlickering) // Only take damage if not flickering
        {
            currentHealth -= amount;
            Debug.Log(currentHealth);
            UpdateDamageOverlay();


            if (currentHealth <= 0)
            {
                SceneManager.LoadScene("SampleScene");
                Start();
            }
            else
            {
                if (flickerCoroutine != null)
                    StopCoroutine(flickerCoroutine);

                flickerCoroutine = StartCoroutine(FlickerSprite(2f, 0.2f));
            }
        }
    }


    IEnumerator FlickerSprite(float duration, float flickerInterval)
    {
        isFlickering = true;

        // Find all objects tagged "Enemy" and ignore collisions with them
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
                if (enemyCollider != null)
                {
                    Physics2D.IgnoreCollision(GetComponent<Collider2D>(), enemyCollider, true);
                }
            }
        }

        float timer = 0f;

        while (timer < duration)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f);
            yield return new WaitForSeconds(flickerInterval);

            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
            yield return new WaitForSeconds(flickerInterval);

            timer += flickerInterval * 2;
        }

        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f); // Ensure sprite is fully visible at the end
        isFlickering = false;

        // Re-enable collisions with objects tagged as "Enemy"
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
                if (enemyCollider != null)
                {
                    Physics2D.IgnoreCollision(GetComponent<Collider2D>(), enemyCollider, false);
                }
            }
        }
    }
}

