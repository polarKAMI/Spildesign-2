using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Musicintromain : MonoBehaviour
{
    private static Musicintromain instance;

    // Name or index of the scene where you don't want the music to play
    [SerializeField] private string sceneToStopMusic = "SampleScene"; // or use int if using scene index

    void Awake()
    {
        // If an instance of MusicManager already exists, destroy the new one
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Set the instance to this instance
        instance = this;

        // Don't destroy this GameObject when loading new scenes
        DontDestroyOnLoad(gameObject);

        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Optional: Ensure the AudioSource plays immediately
        GetComponent<AudioSource>().Play();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == sceneToStopMusic)
        {
            StartCoroutine(FadeOut(2f));
        }
        else if (!GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().Play();
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    public IEnumerator FadeOut(float fadeDuration)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
