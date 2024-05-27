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

    // Reference to the AudioSource
    private AudioSource audioSource;

    void Start()
    {
        lygtemandenScript = GetComponent<Lygtemanden>();

        audioSource = GetComponent<AudioSource>();

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
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    // Called by the child trigger script
    public void OnPlayerExitTrigger()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
   

