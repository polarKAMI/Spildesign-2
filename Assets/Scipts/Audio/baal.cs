using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baal : MonoBehaviour
{
    public AudioSource audioSource;
    private Coroutine fadeCoroutine;

    void Start()
    {
        // Get the AudioSource component attached to the same GameObject
        audioSource = GetComponent<AudioSource>();

        // Ensure the AudioSource component exists
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource component found on this GameObject. Please add one.");
        }
    }

    public IEnumerator FadeIn(float fadeDuration)
    {
        if (audioSource == null)
        {
            yield break;
        }

        float startVolume = 0f;
        audioSource.volume = startVolume;
        audioSource.Play();

        Debug.Log("Started FadeIn");

        while (audioSource.volume < 1)
        {
            audioSource.volume += Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.volume = 1f;
    }

    public IEnumerator FadeOut(float fadeDuration)
    {
        if (audioSource == null)
        {
            yield break;
        }

        float startVolume = audioSource.volume;

        Debug.Log("Started FadeOut");

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the entering object is the player
        if (collision.CompareTag("Player"))
        {
            if (audioSource != null)
            {
                if (fadeCoroutine != null)
                {
                    StopCoroutine(fadeCoroutine);
                }
                fadeCoroutine = StartCoroutine(FadeIn(2f));
                Debug.Log("Player entered the trigger zone.");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (audioSource != null)
            {
                if (fadeCoroutine != null)
                {
                    StopCoroutine(fadeCoroutine);
                }
                fadeCoroutine = StartCoroutine(FadeOut(3f));
                Debug.Log("Player exited the trigger zone.");
            }
        }
    }
}
