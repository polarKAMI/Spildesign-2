using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public GameOverMenu gameOverMenu;

    public int MaxHealth = 10;
    public int currentHealth;

    private SpriteRenderer spriteRenderer;
    private Coroutine flickerCoroutine;
    private bool isFlickering = false;

    void Start()
    {
        currentHealth = MaxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (gameOverMenu == null)
        {
            gameOverMenu = FindObjectOfType<GameOverMenu>();
        }
    }

    public void AddHealth(int amount)
    {
        currentHealth += amount;
        Debug.Log(currentHealth);
    }

    public void TakeDamage(int amount)
    {
        if (!isFlickering) // Only take damage if not flickering
        {
            currentHealth -= amount;
            Debug.Log(currentHealth);

            if (currentHealth <= 0)
            {
                Time.timeScale = 0;
                if (gameOverMenu != null) { gameOverMenu.GameOver(); }
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

