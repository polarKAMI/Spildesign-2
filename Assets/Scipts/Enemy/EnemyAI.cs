using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.Burst.CompilerServices;

public class EnemyAI : MonoBehaviour
{
    [Header("Pathfinding")]
    public Transform target;
    public float activeDistance = 5f;
    public float chaseDistance = 30f;
    public float pathUpdateSeconds = 0.5f;
    public bool isJumping = false;

    [Header("Physics")]
    public float launchAngle = 45f;
    public float minLaunchForce = 10f;
    public float maxLaunchForce = 20f;
    public GameObject tempSkft;
    private Rigidbody2D[] allRigidbodies;

    [Header("Custom Behavior")]
    public Color startColor = Color.grey;
    public Color endColor = Color.white;
    public float colorChangeDuration = 1.0f;
    public SpriteRenderer spriteRenderer;
    public LayerMask groundLayer; // Assign the Ground layer mask in the Inspector
    public LogSO log;
    public NotificationManager notificationManager;

    private bool isConcealed = true;
    private float launchCooldownTime = 2f; // Adjust as needed
    private Seeker seeker;
    private Rigidbody2D rb;
    private float distanceToTarget; // Distance to the target calculated by A* pathfinding

    [Header("Health")]
    public int maxHealth = 10;
    public int currentHealth;

    [Header("Attack")]
    public Health health;
    public int attackDamage = 2;
    public bool hasDealtDamage;

    public Animator animator;


    public GameObject avsound;
    public GameObject attacksound;
    public GameObject dudsound;


    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on child object!");
        }
        else
        {
            spriteRenderer.color = startColor;
        }
    }

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        currentHealth = maxHealth;
        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);

        allRigidbodies = FindObjectsOfType<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (TargetInDistance() && seeker.IsDone())
        {
            if (isConcealed)
            {
                animator.SetBool("IsTransformed", true);
                StartCoroutine(ChangeColorAndActivate());
            }
            else if (TargetInDistance() && seeker.IsDone() && IsGrounded() && !isJumping)
            {
                LaunchEnemy();
                animator.SetBool("IsGrounded", false);
            }
            else if (TargetInDistance() && seeker.IsDone() && IsGrounded() && isJumping)
            {
                animator.SetBool("IsGrounded", true);
            }
        }
        else if (!TargetInDistance() && !isConcealed)
        {
            StartCoroutine(CheckAndConceal()); // Call the function when the player is outside target range
        }
        if (rb.velocity.x > 0.05f)
        {
            transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (rb.velocity.x < -0.05f)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }


    private void UpdatePath()
    {
        if (!isConcealed && seeker.IsDone())
        {
            seeker.StartPath(transform.position, target.position, OnPathComplete);
        }
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            distanceToTarget = p.GetTotalLength();
        }
    }

    private bool IsGrounded()
    {
        // Get the circle collider component
        CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();

        if (circleCollider == null)
        {
            Debug.LogError("CircleCollider2D component not found!");
            return false;
        }

        // Calculate the position of the bottom edge of the circle collider
        Vector2 bottomEdge = (Vector2)transform.position - Vector2.up * circleCollider.radius;
        // Perform overlap circle check at the bottom edge position
        return Physics2D.OverlapCircle(bottomEdge, 0.05f, groundLayer) != null;

    }

    private IEnumerator ChangeColorAndActivate()
    {
        yield return new WaitForSeconds(1f);
        isConcealed = false;
    }

    private IEnumerator CheckAndConceal()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f); // Wait for 2 seconds
            if (!TargetInDistance())
            {
                yield return StartCoroutine(ReverseChangeColorAndDeactivate());
                break; // Exit the loop if target is no longer in chase range
            }
        }
    }

    private IEnumerator ReverseChangeColorAndDeactivate()
    {
        float elapsedTime = 0;
        while (elapsedTime < colorChangeDuration)
        {
            spriteRenderer.color = Color.Lerp(endColor, startColor, elapsedTime / colorChangeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = startColor; // Ensure final color is set
        isConcealed = true; // Set isConcealed back to true
    }

    private bool TargetInDistance()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        return isConcealed ? distance < activeDistance : distance < chaseDistance;
    }

    private Vector2 PredictedLandingPosition(Vector2 launchDirection, float launchForce)
    {
        CircleCollider2D enemyCollider = GetComponent<CircleCollider2D>();
        Rigidbody2D enemyRB = GetComponent<Rigidbody2D>();
        enemyRB.GetComponent<Rigidbody2D>().simulated = false;
        tempSkft.GetComponent<CircleCollider2D>().enabled = false;
        enemyCollider.enabled = false;
        // Create a new GameObject as a child of the enemy GameObject
        GameObject tempObject = Instantiate(tempSkft, transform.position, Quaternion.identity);

        tempObject.GetComponent<CircleCollider2D>().enabled = true;
        tempObject.GetComponent<Rigidbody2D>().simulated = true;


        Rigidbody2D tempRb = tempObject.GetComponent<Rigidbody2D>();

        Debug.Log("tempRb: " + tempRb.transform.position);
        // Apply launch force to the temporary Rigidbody

        tempRb.velocity = Vector2.zero;
        tempRb.AddForce(launchDirection.normalized * launchForce, ForceMode2D.Impulse);

        // Store the current simulation mode
        var prevSimulationMode = Physics2D.simulationMode;

        // Set the simulation mode to Script
        Physics2D.simulationMode = SimulationMode2D.Script;
        Debug.Log("tempRb2: " + tempRb.transform.position);

        foreach (Rigidbody2D rb in allRigidbodies)
        {
            if (rb != null)
            {
                rb.simulated = false;
            }

        }

        int simulationSteps = 200;
        for (int i = 1; i < simulationSteps; i++)
        {
            //using Time.fixedDeltaTime as the time step to match SimulationMode2D.FixedUpdate
            Physics2D.Simulate(Time.fixedDeltaTime);
        }
        enemyCollider.enabled = true;
        tempSkft.GetComponent<CircleCollider2D>().enabled = true;

        Vector2 predictedPosition = tempRb.transform.position;

        foreach (Rigidbody2D rb in allRigidbodies)
        {
            if (rb != null)
            {
                rb.simulated = true;
            }

        }

        Physics2D.simulationMode = prevSimulationMode;
        Debug.Log("tempRb3: " + tempRb.transform.position);

        Destroy(tempObject);
        return predictedPosition;
    }

    private void LaunchEnemy()
    {
        isJumping = true; // Set the flag to indicate that the enemy is jumping
        hasDealtDamage = false;
        float initialYpos = transform.position.y;
        Vector2 directionToTarget = (target.position - transform.position).normalized;
        float adjustedLaunchAngle = launchAngle;
        animator.SetBool("IsJumping", true);

        if (directionToTarget.x < 0)
        {
            adjustedLaunchAngle = 180 - launchAngle;
        }

        Vector2 launchDirection = Quaternion.Euler(0, 0, adjustedLaunchAngle) * Vector2.right;

        // Calculate launch force based on distance to target
        float launchForce = Mathf.Clamp(Mathf.Abs(distanceToTarget) * 2.8f, minLaunchForce, maxLaunchForce);

        Vector2 predictedLandingPos = PredictedLandingPosition(launchDirection, launchForce);
        Debug.Log("AAA predicted x pos: " + predictedLandingPos.x + " predicted y pos: " + predictedLandingPos.y + " calculated launch force: " + launchForce + " launch direction: " + launchDirection);

        // Check if there's ground at the predicted landing position
        RaycastHit2D hit = Physics2D.Raycast(predictedLandingPos, Vector2.down, 10f, groundLayer);
        Debug.Log("BBB y pos: " + initialYpos);
        Debug.Log("CCC predicted y hit: " + hit.point.y + " collider: " + hit.collider);

        if (hit.collider != null && Mathf.Abs(hit.point.y - initialYpos) < .7f)
        {
            Debug.Log("ez jump");
            rb.AddForce(launchDirection.normalized * launchForce, ForceMode2D.Impulse);
        }
        else
        {
            float cand1, cand2;

            Vector2 minLandingPos = PredictedLandingPosition(launchDirection, minLaunchForce);
            Debug.Log("DDD predicted min x pos: " + minLandingPos.x + " predicted min y pos: " + minLandingPos.y);
            Vector2 maxLandingPos = PredictedLandingPosition(launchDirection, maxLaunchForce);
            Debug.Log("EEE predicted max x pos: " + maxLandingPos.x + " predicted max y pos: " + maxLandingPos.y);

            RaycastHit2D minHit = Physics2D.Raycast(minLandingPos, Vector2.down, 10f, groundLayer);
            Debug.Log("FFF predicted min y RAY: " + minHit.point.y + " collider: " + minHit.collider);
            RaycastHit2D maxHit = Physics2D.Raycast(maxLandingPos, Vector2.down, 10f, groundLayer);
            Debug.Log("GGG predicted max y RAY: " + maxHit.point.y + " collider: " + maxHit.collider);
            if (minHit.collider != null && Mathf.Abs(minHit.point.y - initialYpos) < 1f)
            {
                cand1 = findBestBetween(minLaunchForce, launchForce, launchDirection, initialYpos);
            }
            else
            {
                cand1 = -1f;
            }

            if (maxHit.collider != null && Mathf.Abs(maxHit.point.y - initialYpos) < 1f)
            {
                cand2 = findBestBetween(maxLaunchForce, launchForce, launchDirection, initialYpos);
            }
            else
            {
                cand2 = -1f;
            }
            if (cand1 == -1f || Mathf.Abs(cand1 - launchForce) > Mathf.Abs(cand2 - launchForce))
            {
                Debug.Log("cand2 jump " + cand2);
                rb.AddForce(launchDirection.normalized * cand2, ForceMode2D.Impulse);
            }
            else
            {
                Debug.Log("cand1 jump " + cand1);
                rb.AddForce(launchDirection.normalized * cand1, ForceMode2D.Impulse);
            }
        }

        StartCoroutine(LaunchCooldown());
    }

    private float findBestBetween(float hitsGround, float hitsHole, Vector2 launchDirection, float initialYpos)
    {
        Debug.Log("leder efter sted at lande :D" + hitsGround + ", " + hitsHole);
        if (Mathf.Abs(hitsGround - hitsHole) > 1f)
        {
            float mid = hitsHole + (hitsGround - hitsHole) / 2;
            Vector2 midLandPos = PredictedLandingPosition(launchDirection, mid);
            RaycastHit2D midHit = Physics2D.Raycast(midLandPos, Vector2.down, 10f, groundLayer);
            if (midHit.collider != null && Mathf.Abs(midHit.point.y - initialYpos) < 1f)
            {
                return findBestBetween(mid, hitsHole, launchDirection, initialYpos);
            }
            else
            {
                return findBestBetween(hitsGround, mid, launchDirection, initialYpos);
            }
        }
        else
        {
            return hitsGround;
        }
    }


    private IEnumerator LaunchCooldown()
    {
        yield return new WaitUntil(() => IsGrounded()); // Wait until the enemy becomes grounded again
        yield return new WaitForSeconds(launchCooldownTime);
        isJumping = false; // Reset the flag after the cooldown period
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !IsGrounded() && !hasDealtDamage)
        {
            Attack();
            hasDealtDamage = true; // Set the flag to true after dealing damage during a jump
        }
    }
    public void Attack()
    {
        health.TakeDamage(attackDamage);
        Instantiate(attacksound);
    }

    public void TakeDamage(int amount)
    {
        Instantiate(avsound);
        currentHealth -= amount;

    }

    public void Die()
    {
        if (!log.Collected)
        {
            // Add the log to the LogManager
            LogManager.AddLog(log);
            // Set the collected flag to true
            log.Collected = true;
            notificationManager.ShowNotification("new log;");
        }
        Instantiate(dudsound);
        Destroy(gameObject);
    }
}