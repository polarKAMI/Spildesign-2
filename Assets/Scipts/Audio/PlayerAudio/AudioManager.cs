using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    

    public AudioSource playerHurtSource;
    public List<AudioSource> allAudioSources;

    

    public void StopAllSoundsExceptPlayerHurt()
    {
        foreach (AudioSource audioSource in allAudioSources)
        {
            if (audioSource != playerHurtSource)
            {
                audioSource.Stop();
                Debug.Log("all audio stopped");
            }
        }
    }
}
