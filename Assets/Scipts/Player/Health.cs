using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public GameOverMenu gameOverMenu;

    public int MaxHealth = 10;
    public int currentHealth;

    private SpriteRenderer spriteRenderer;
    private Coroutine flickerCoroutine;
    private bool isFlickering = false;

    public GameObject DamageOverlay;

    public AudioManager audioManagerObject;

    public GameObject Avsound;

    public GameObject spillerdørsound;

    void Start()
    {
        currentHealth = MaxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateDamageOverlay();
        if (gameOverMenu == null)
        {
            gameOverMenu = FindObjectOfType<GameOverMenu>();
        }

        
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

            if (currentHealth > 9)
            {
                AudioManager_Playerhurt.instance.StopCurrentSound();
                Debug.Log("Audio stopped");
            }



            if (currentHealth < 8 && currentHealth > 3)
            {
                AudioManager_Playerhurt.instance.Hurt();
                Debug.Log("hurt is playing");
            }

            if (currentHealth < 3 && currentHealth > 1)
            {
                AudioManager_Playerhurt.instance.Reallyhurt();
                Debug.Log("Reallyhurt is playing");
            }


            if (currentHealth < 1)
            {
                AudioManager_Playerhurt.instance.Dead();
                Debug.Log("dead is playing");
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
            

            if (currentHealth > 0)
            {
                Instantiate(Avsound);
            }

            if (currentHealth <= 0)
            {
                Time.timeScale = 0;
                if (gameOverMenu != null)
                {
                    gameOverMenu.GameOver();
                }
                Instantiate(spillerdørsound);

                audioManagerObject.StopAllSoundsExceptPlayerHurt(); //Stop all sounds except player hurt
               
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

