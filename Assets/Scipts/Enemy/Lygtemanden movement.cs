using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lygtemandenmovement : MonoBehaviour
{
    public Transform[] Patrolpoints;
    public float Movespeed;
    public int patroldestination;

    // Reference to Lygtemanden script
    private Lygtemanden lygtemandenScript;

    private Lygteidletrigger idleTriggerScript;

    // Reference to the AudioSource
    private AudioSource audioSource;

    // Track if the player is in the trigger collider
    public bool isPlayerInTrigger = false;

    public bool funderundedone = false;

    void Start()
    {
        lygtemandenScript = GetComponent<Lygtemanden>();

        audioSource = GetComponent<AudioSource>();

        // Find the Lygteidletrigger script
        idleTriggerScript = GetComponentInChildren<Lygteidletrigger>();

       

        // Set the audio clip to loop
        if (audioSource != null)
        {
            audioSource.loop = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (patroldestination == 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, Patrolpoints[0].position, Movespeed * Time.deltaTime);
            lygtemandenScript.FaceTarget(Patrolpoints[0].position); // Ensure facing towards the patrol point
            if (Vector2.Distance(transform.position, Patrolpoints[0].position) < .2f)
            {
                patroldestination = 1;
            }
        }

        if (patroldestination == 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, Patrolpoints[1].position, Movespeed * Time.deltaTime);
            lygtemandenScript.FaceTarget(Patrolpoints[1].position); // Ensure facing towards the patrol point
            if (Vector2.Distance(transform.position, Patrolpoints[1].position) < .2f)
            {
                patroldestination = 0;
            }
        }

       
    }


    // Called by the child trigger script
    public void OnPlayerEnterTrigger()
    {
        

        
        // Check if patrolling and play audio
        if (!audioSource.isPlaying && (isPlayerInTrigger = true) && (funderundedone = true))
        {
            audioSource.Play();
            Debug.Log("Has started the audio on lygtemand idle");
            Debug.Log("OnPlayerentertrigeer er triggered");
        }
    }

    // Called by the child trigger script
    public void OnPlayerExitTrigger()
    {
        isPlayerInTrigger = false;
        

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            Debug.Log("Has stopped the audio on lygtemand idle");
        }

        audioSource.Stop();
        Debug.Log("Audio has stopped");

        // Start coroutine to stop audio after 3 seconds if player is not in the trigger
       
    }


    // Method to stop audio when chase begins
    public void StopAudio()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }


    // Method to start audio when patrolling begins
    public void StartPatrolling()
    {
        if (audioSource != null && !audioSource.isPlaying && (isPlayerInTrigger = true))// skal lave om p denne
        {
            audioSource.Play();
            funderundedone = true;
            Debug.Log("Startpatrolling audio is triggered");
        }
    }

   

   


    void OnDisable()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }





}
   

