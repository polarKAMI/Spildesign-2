using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footstepsground : MonoBehaviour
{
    public LayerMask groundLayer; // Layer mask for the ground
    public AudioClip[] soundEffects; // Array of sound effects to play
    public float minPitch = 0.8f; // Minimum pitch for randomization
    public float maxPitch = 1.2f; // Maximum pitch for randomization
    public float footstepDelay = 0.5f; // Delay between footstep sounds

    public AudioSource audioSource;
    public Rigidbody2D rb;

    private bool isPlayingFootsteps = false;

    void Start()
    {
        // Get AudioSource component or add one if not present
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Get Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Check if the player is moving on the ground layer
        if (IsMovingOnGround() && rb.velocity.magnitude > 0.1f)
        {
            // Start playing sound effects if not already playing
            if (!isPlayingFootsteps)
            {
                StartCoroutine(PlayFootstepSounds());
            }
        }
        else
        {
            // Stop playing sound effects if not moving on the ground
            StopCoroutine(PlayFootstepSounds());
            isPlayingFootsteps = false;
            audioSource.Stop();
        }
    }

    bool IsMovingOnGround()
    {
        // Raycast downward to detect if the object is on the ground layer
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);
        return hit.collider != null;
    }

    IEnumerator PlayFootstepSounds()
    {
        isPlayingFootsteps = true;

        while (IsMovingOnGround() && rb.velocity.magnitude > 0.1f)
        {
            if (!audioSource.isPlaying)
            {
                PlayRandomSound();
            }
            yield return new WaitForSeconds(footstepDelay);
        }

        isPlayingFootsteps = false;
    }

    void PlayRandomSound()
    {
        // Randomly select a sound effect from the array
        AudioClip soundEffect = soundEffects[Random.Range(0, soundEffects.Length)];

        // Randomize pitch
        float randomPitch = Random.Range(minPitch, maxPitch);

        // Set the pitch and play the sound effect
        audioSource.pitch = randomPitch;
        audioSource.clip = soundEffect;
        audioSource.Play();
    }
}
