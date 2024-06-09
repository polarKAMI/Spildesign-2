using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public bool isFacingRight = true;
    private float horizontal;
    public float currentSpeed = 0f; // Current movement speed
    public float baseSpeed = 1.5f; // Base movement speed
    public float maxSpeed = 3f; // Maximum movement speed
    public float acceleration = 3f; // Acceleration rate
    public float deceleration = 4f; // Deceleration rate
    public float sprintSpeedMultiplier = 1.3f; // Speed multiplier when sprinting
    public bool isSprinting = false;
    [SerializeField] private Rigidbody2D rb;
    public InventoryUIManager inventoryUIManager;
    private PlayerJump playerJump;
    public bool PlayerHidden = false; // her gemmer
    public bool movementEnabled = true; // Flag to track if movement is enabled

    Lygtemanden lygtemandenscript; // reference til lygtemanden script iforhold til busk collider
    private Busk currentBusk; // Reference to the current Busk script the player is interacting with

    private bool isSlowed = false;
    private float slowDuration;
    private float slowAmount;
    private float slowTimer;

    // Stamina variables
    public float maxStamina = 100f; // Maximum stamina
    public float currentStamina; // Current stamina
    public float staminaDecreaseRate = 10f; // Rate at which stamina decreases when sprinting
    public float staminaRecoveryRate = 5f; // Rate at which stamina recovers when not sprinting
    public GameObject breathingGameObject;

    public AudioSource audioSource;
    public AudioClip Sprintsclip;
    public Animator animator;
    private bool isRecoveringStamina = true;



    private void Start()
    {
        // Set the initial currentSpeed to the baseSpeed
        currentSpeed = baseSpeed;   
        playerJump = GetComponent<PlayerJump>();

        // Find and assign references to Lygtemanden and Busk scripts
        lygtemandenscript = FindObjectOfType<Lygtemanden>();
       
        if (lygtemandenscript == null)
        {
            Debug.LogError("Lygtemanden script not found in the scene.");
        }

        audioSource = GetComponent<AudioSource>();

        // Initialize stamina
        currentStamina = maxStamina;

    }

    public void ApplySlow(float duration, float amount)
    {
        slowDuration = duration;
        slowAmount = amount;
        slowTimer = duration;
        isSlowed = true;
    }

    public void Sprinting()
    {
        PlaySprintSound();

        if (isSprinting)
        {
           
            
            Debug.Log("Audioclip burde virke");
        }
        else
        {
            isSprinting = true;
        }
    }


    public void NotSprinting()
    {
        StopSprintSound();

        if (!isSprinting)
        {
            
           
        }
        else
        {
            isSprinting = false;
        }
    }

    public void Move(float input)
    {
        if (!movementEnabled)
            return; // Exit Move() if movement is disabled

        // Assign input value to horizontal
        horizontal = input;

        // Apply slow effect if active
        float speedFactor = 1f;
        if (isSlowed)
        {
            slowTimer -= Time.deltaTime;
            if (slowTimer <= 0)
            {
                isSlowed = false;
            }
            else
            {
                speedFactor = Mathf.Lerp(1f, slowAmount, slowTimer / slowDuration);
            }
        }

        // Calculate effective max speed
        float effectiveMaxSpeed = maxSpeed * speedFactor;
        if (horizontal != 0f)
        {
            if (isSprinting && currentStamina > 0f)
            {
                effectiveMaxSpeed *= sprintSpeedMultiplier;
                currentSpeed = Mathf.MoveTowards(currentSpeed, effectiveMaxSpeed, acceleration * Time.deltaTime);
                currentStamina -= staminaDecreaseRate * Time.deltaTime;
                Debug.Log($"Current Stamina: {currentStamina}");
                if (currentStamina <= 0f)
                {
                    StartCoroutine(HandleStaminaDepletion());
                }
            }
            else
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, effectiveMaxSpeed, acceleration * Time.deltaTime);
                RecoverStamina();
            }
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
            RecoverStamina();
        }

        Flip();
    }

    private void FixedUpdate()
    {
        animator.SetFloat("Speed", Mathf.Abs(currentSpeed));
        if (movementEnabled && playerJump.isGrounded && !playerJump.isSliding)
        {
            rb.velocity = new Vector2(horizontal * currentSpeed, rb.velocity.y);
        }

        if (currentBusk != null && currentBusk.isin) // Player is hiding
        {
            Debug.Log("Player hidden");
            PlayerHidden = true;
           
        }
        else
        {
            PlayerHidden = false;
        }

        if (currentBusk != null && currentBusk.isin && !lygtemandenscript.isHidingHandled && (lygtemandenscript.isChasing || lygtemandenscript.isStopped)) // Player is hiding
        {
            Debug.Log("Player hidden from lygtemanden");
            lygtemandenscript.isHidingHandled = true;
            StartCoroutine(lygtemandenscript.HandleBushCollision());
        }
    }
    public void Flip()
    {
        if ((horizontal > 0 && !isFacingRight) || (horizontal < 0 && isFacingRight))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;

            // Reset current speed to baseSpeed when flipping
            currentSpeed = baseSpeed;
        }
    }

    public void EnableMovement()
    {
        movementEnabled = true;
    }

    public void DisableMovement()
    {
        movementEnabled = false;
        Move(0f);
        currentSpeed = 0f;
        horizontal = 0f;
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bush"))
        {
            currentBusk = other.GetComponent<Busk>();
            if (currentBusk != null)
            {
                Debug.Log("Entered bush");
               
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bush"))
        {
            if (currentBusk != null && currentBusk.gameObject == other.gameObject)
            {
                Debug.Log("Exited bush");
                currentBusk = null;
            }
        }
    }


    private IEnumerator HandleStaminaDepletion()
    {
        isRecoveringStamina = false; // Pause stamina recovery
        Instantiate(breathingGameObject);



        yield return new WaitForSeconds(6f);

        currentStamina = maxStamina;
        isRecoveringStamina = true; // Pause stamina recovery
        Debug.Log($"Stamina reset to: {currentStamina}"); // Log the reset stamina
    }

    private void RecoverStamina()
    {
        if (isRecoveringStamina && currentStamina < maxStamina)
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
        }
    }

    private void PlaySprintSound()
    {
        if (Sprintsclip != null && audioSource != null)
        {
            audioSource.clip = Sprintsclip;
            audioSource.Play();
        }
    }

    private void StopSprintSound()
    {
        if (audioSource != null && audioSource.clip == Sprintsclip)
        {
            audioSource.Stop();
        }
    }


}