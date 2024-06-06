using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderMovement : MonoBehaviour, IInteractable
{
    private Rigidbody2D rb;
    private PlayerMovement playerMovement;
    private PlayerJump playerJump;
    public bool isClimbing = false;
    private Transform ladderTransform;
    private float vertical;
    private float ladderTopY;
    public float ladderTopYOffSet = 0.7f;
    private float pushOffVelocityX = 0f;
    private bool isPushingOff = false;
    private Collider2D platformCollider;
    private Collider2D ladderCollider;

    public Animator animator;

    [SerializeField] private float climbSpeed = 3f;

    // Footstep sound variables

    public AudioClip[] soundEffects; // Array of sound effects to play
    public float minPitch = 0.8f; // Minimum pitch for randomization
    public float maxPitch = 1.2f; // Maximum pitch for randomization
    public float footstepDelay = 0.5f; // Delay between footstep sounds
    public AudioSource audioSource2;

    public AudioClip startClimbSound; // Sound to play when starting to climb
    public AudioClip stopClimbSound; // Sound to play when stopping to climb

    private bool isPlayingFootsteps = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        playerJump = GetComponent<PlayerJump>();

        // Get the second AudioSource component
        AudioSource[] audioSources = GetComponents<AudioSource>();
        if (audioSources.Length > 1)
        {
            audioSource2 = audioSources[1]; // Use the second AudioSource
        }
        else
        {
            Debug.LogError("Not enough AudioSource components found. Ensure there are at least two AudioSources on the GameObject.");
        }
    }

    public void Interact()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f); // Adjust radius as needed
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Ladder"))
            {
                StartClimbing(collider.transform);
            }
        }
    }

    public void StartClimbing(Transform ladder)
    {
        playerMovement.DisableMovement();
        GlobalInputMapping.SetActiveInputMappings(GlobalInputMapping.climbInputMapping);
        isClimbing = true;
        playerJump.isFalling = false;
        playerJump.isJumping = false;
        playerJump.isSliding = false;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(0, 0);
        ladderTransform = ladder;
        SnapToLadder();
        Debug.Log("Started climbing");
        Debug.Log("AnimationClimbing");
        ladderCollider = ladder.GetComponent<Collider2D>();
        if (ladderCollider != null)
        {
            Debug.Log("Climbing on ladder collider: " + ladderCollider.name);
            ladderTopY = ladderCollider.bounds.max.y;
        }

        platformCollider = ladder.Find("ladderPlatform")?.GetComponent<Collider2D>();
        if (platformCollider != null)
        {
            Debug.Log("Platform collider detected: " + platformCollider.name);
            platformCollider.isTrigger = true;
            ladderTopY = platformCollider.bounds.max.y;
        }

        animator.SetBool("IsClimbing", true);
        Debug.Log("Ladder top Y: " + ladderTopY);

        // Play start climbing sound
        PlaySound(startClimbSound);

    }

    public void StopClimbing()
    {
        isClimbing = false;
        rb.gravityScale = 2f;
        playerMovement.EnableMovement();
        GlobalInputMapping.SetActiveInputMappings(GlobalInputMapping.inGameInputMapping);
        Debug.Log("Stopped climbing");

        isPushingOff = false;
        pushOffVelocityX = 0f;

        if (platformCollider != null)
        {
            platformCollider.isTrigger = false;
        }
        animator.SetBool("IsClimbing", false);


        // Play stop climbing sound
        PlaySound(stopClimbSound);

        // Stop playing footstep sounds
        if (isPlayingFootsteps)
        {
            StopCoroutine(PlayFootstepSounds());
            isPlayingFootsteps = false;
        }
    }

    public void PushOffLadder(int direction)
    {
        isPushingOff = true;
        pushOffVelocityX = direction * climbSpeed * 1.3f;
        rb.AddForce(new Vector2(pushOffVelocityX, 0f), ForceMode2D.Impulse);
        StopClimbing();
    }

    private void SnapToLadder()
    {
        Vector3 targetPosition = new Vector3(ladderTransform.position.x, transform.position.y, transform.position.z);
        rb.MovePosition(targetPosition);
    }

    public void Climb(float input)
    {
        if (isClimbing)
        {
            vertical = input;
            Debug.Log("vertical is " + vertical);
        }
        else
        {
            vertical = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (isClimbing)
        {
            if (ladderCollider != null)
            {
                Debug.Log("Currently climbing on ladder collider: " + ladderCollider.name);
            }

            rb.velocity = new Vector2(0, vertical * climbSpeed);
            Debug.Log("Player Y Position: " + transform.position.y);
            Debug.Log("ladderTopY + ladderTopYOffSet: " + (ladderTopY + ladderTopYOffSet));

            if (transform.position.y >= ladderTopY + ladderTopYOffSet)
            {
                StopClimbing();
            }

            // Start playing footstep sounds
            if (!isPlayingFootsteps)
            {
                StartCoroutine(PlayFootstepSounds());
            }

        }
        else if (isPushingOff)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }
    }

    IEnumerator PlayFootstepSounds()
    {
        isPlayingFootsteps = true;

        while (isClimbing)
        {
            if (Mathf.Abs(vertical) > 0.1f)
            {
                PlayRandomSound();
                Debug.Log("One climbing sound");
                yield return new WaitForSeconds(footstepDelay);
            }
            else
            {
                yield return null; // wait until next frame
            }
        }

        isPlayingFootsteps = false;
    }

    void PlayRandomSound()
    {
        AudioClip soundEffect = soundEffects[Random.Range(0, soundEffects.Length)];
        float randomPitch = Random.Range(minPitch, maxPitch);
        audioSource2.pitch = randomPitch;
        audioSource2.clip = soundEffect;
        audioSource2.Play();
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource2 != null && clip != null)
        {
            audioSource2.clip = clip;
            audioSource2.pitch = 1.0f; // Reset pitch to normal for these specific sounds
            audioSource2.Play();
            Debug.Log("Playing sound: " + clip.name);
        }
        else
        {
            Debug.LogError("audioSource2 or audio clip is not assigned.");
        }
    }

}
