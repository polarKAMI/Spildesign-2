using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager_Playerhurt : MonoBehaviour
{

    public static AudioManager_Playerhurt instance;

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip hurt;

    [SerializeField] private AudioClip reallyhurt;

    [SerializeField] private AudioClip dead;


    private bool isHurtPlaying = false;
    private bool isReallyhurtPlaying = false;
    private bool isDeadPlaying = false;
    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    


    public void Hurt()
    {
        if (!isHurtPlaying)
        {
            isHurtPlaying=true;
            audioSource.clip = hurt;
            audioSource.loop = true;
            audioSource.Play();
        }

    }


    public void Reallyhurt()
    {
        if (isHurtPlaying)
        {
            isReallyhurtPlaying = true;
            isHurtPlaying = false;
            audioSource.clip = reallyhurt;
            audioSource.loop = true;
            audioSource.Play();
        }

    }

    public void Dead()
    {
        if (isReallyhurtPlaying)
        {
            isHurtPlaying = false;
            isReallyhurtPlaying=false;
            audioSource.clip = dead;
            audioSource.loop = false;
            audioSource.Play();
        }

    }


    public void StopCurrentSound()
    {
        isHurtPlaying = false;
        isReallyhurtPlaying = false;
        isDeadPlaying = false;
        audioSource.loop = false;
        audioSource.Stop();

    }



}
