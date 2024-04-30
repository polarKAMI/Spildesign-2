using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadSoundObject : MonoBehaviour
{
    public AudioSource audioSourceToPlay2; // Reference to your AudioSource

    void Start()
    {

        // Invoke the PlayAudio method after a delay of 3 seconds
        Invoke("PlayAudio2", 5f);
    }

    void PlayAudio2()
    {
        // Play the audio clip
        audioSourceToPlay2.Play();

        Destroy(gameObject, audioSourceToPlay2.clip.length);
    }
}
