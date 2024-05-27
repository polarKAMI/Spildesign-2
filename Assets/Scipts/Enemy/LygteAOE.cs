using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LygteAOE : MonoBehaviour
{
    public Health healthScript; // Reference to the Health script attached to the player

    private float timeInCollider = 0f; // Time the player has spent in the collider
    private bool isPlayerInCollider = false; // Flag to track if the player is in the collider
    private Coroutine continuousDamageCoroutine; // Reference to the continuous damage coroutine
    private Coroutine audioCoroutine; // Reference to the audio playback coroutine

    public Collider2D lygteCollider;

    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip Lygte1;
    public AudioClip Lygte2;
    public AudioClip Lygte3;
    public AudioClip Lygte4;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (lygteCollider == null)
        {
            lygteCollider = GetComponent<Collider2D>();
            if (lygteCollider == null)
            {
                Debug.LogError("Collider2D is not assigned and not found on the GameObject.");
            }
        }

        timeInCollider = 0f; // Reset time in collider
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object has the tag "Player"
        if (other.CompareTag("Player"))
        {
            // Start tracking time in collider and playing audio if not already doing so
            if (!isPlayerInCollider)
            {
                audioSource.Play();
                isPlayerInCollider = true;
                continuousDamageCoroutine = StartCoroutine(TrackTimeInCollider());
                Debug.Log("Du er i lygte");
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Check if the colliding object has the tag "Player"
        if (other.CompareTag("Player"))
        {
            // If the player exits the trigger, stop tracking time and playing audio
            if (isPlayerInCollider)
            {
                isPlayerInCollider = false;
                if (continuousDamageCoroutine != null)
                {
                    StopCoroutine(continuousDamageCoroutine);
                    continuousDamageCoroutine = null;
                }
                if (audioCoroutine != null)
                {
                    StopCoroutine(audioCoroutine);
                    audioCoroutine = null;
                }
                audioSource.Stop();
                timeInCollider = 0f; // Reset time in collider
                Debug.Log("Du er ikke i lygte");
            }
        }
    }

    IEnumerator TrackTimeInCollider()
    {
        // Continuously track time in collider
        while (isPlayerInCollider)
        {
            yield return new WaitForSeconds(1f);
            timeInCollider += 1f;
            UpdateInstantiatedObject();
        }
    }

    void UpdateInstantiatedObject()
    {
        if (audioCoroutine == null) // Ensure only one audio coroutine is running at a time
        {
            if (timeInCollider >= 31f && audioSource.clip != Lygte4)
            {
                ReplaceInstantiatedObject(Lygte4);
                Debug.Log("Lyd 4 er igang");
            }
            else if (timeInCollider >= 24f && audioSource.clip != Lygte3)
            {
                ReplaceInstantiatedObject(Lygte3);
                Debug.Log("Lyd 3 er igang");
            }
            else if (timeInCollider >= 10f && audioSource.clip != Lygte2)
            {
                ReplaceInstantiatedObject(Lygte2);
                Debug.Log("Lyd 2 er igang");
            }
            else if (timeInCollider >= 0f && audioSource.clip != Lygte1)
            {
                ReplaceInstantiatedObject(Lygte1);
                Debug.Log("Lyd 1 er igang");
            }
        }
    }

    void ReplaceInstantiatedObject(AudioClip newClip)
    {
        if (audioSource.clip != newClip)
        {
            if (audioCoroutine != null)
            {
                StopCoroutine(audioCoroutine);
            }
            audioSource.clip = newClip;
            audioCoroutine = StartCoroutine(PlayAudioClip(newClip));
        }
    }

    IEnumerator PlayAudioClip(AudioClip clip)
    {
        audioSource.Play();
        yield return new WaitForSeconds(clip.length);
        audioSource.Stop();
        audioCoroutine = null; // Reset the coroutine reference to allow the next clip to play

        // Check if the clip is Lygte4 and call KillPlayer if it is
        if (clip == Lygte4)
        {
            KillPlayer();
        }
    }

    void KillPlayer()
    {
        if (healthScript != null)
        {
            healthScript.TakeDamage(healthScript.currentHealth); // Assuming TakeDamage can take the current health to kill the player
            Debug.Log("Player killed by Lygte4 audio clip");
        }
    }
}
