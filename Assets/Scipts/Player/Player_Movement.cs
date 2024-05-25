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



    // Stamina variables
    public float maxStamina = 100f; // Maximum stamina
    public float currentStamina; // Current stamina
    public float staminaDecreaseRate = 10f; // Rate at which stamina decreases when sprinting
    public float staminaRecoveryRate = 2f; // Rate at which stamina recovers when not sprinting
   
   

    public GameObject breathingGameObject;

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

        

        // Initialize stamina
        currentStamina = maxStamina;

    }

    public void Sprinting()
    {
       

        if (isSprinting)
        {

        }
        else
        {
            isSprinting = true;
        }
    }


    public void NotSprinting()
    {
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

        float sprintMultiplier = 1.2f;


        if (horizontal != 0f)
        {
            if (isSprinting && currentStamina > 0f) // løber
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed * sprintMultiplier, acceleration * Time.deltaTime);
                currentStamina -= staminaDecreaseRate * Time.deltaTime;
                Debug.Log($"Current Stamina: {currentStamina}");
                if (currentStamina <= 0f)
                {
                    StartCoroutine(HandleStaminaDepletion());
                }
            }
            else
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime); // Går
                RecoverStamina();
            }
        }
        else
        {
            currentSpeed = 0f;
            RecoverStamina();
        }

        Flip();
        
    }


    private void FixedUpdate()
    {
        if (movementEnabled && playerJump.isGrounded) {
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

    private void Flip()
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


}