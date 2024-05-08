using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    [Header("Pathfinding")]
    public Transform target;
    public float activeDistance = 5f;
    public float chaseDistance = 30f;
    public float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    public float launchAngle = 45f;
    public float minLaunchForce = 10f;
    public float maxLaunchForce = 20f;

    [Header("Custom Behavior")]
    public Color startColor = Color.grey;
    public Color endColor = Color.white;
    public float colorChangeDuration = 1.0f;
    public SpriteRenderer spriteRenderer;
    public LayerMask groundLayer; // Assign the Ground layer mask in the Inspector

    private bool isConcealed = true;
    private bool isLaunching = false; // Flag to track if the enemy is currently in the launching state
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

    }

    private void FixedUpdate()
    {
        if (TargetInDistance() && seeker.IsDone())
        {
            if (isConcealed)
            {
                StartCoroutine(ChangeColorAndActivate());
            }
            else if (TargetInDistance() && seeker.IsDone() && IsGrounded() && !isLaunching)
            {
                LaunchEnemy();
            }
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
        isConcealed = false;
        yield return StartCoroutine(ChangeColor());
    }

    private IEnumerator ChangeColor()
    {
        float elapsedTime = 0;
        while (elapsedTime < colorChangeDuration)
        {
            spriteRenderer.color = Color.Lerp(startColor, endColor, elapsedTime / colorChangeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = endColor; // Ensure final color is set
    }

    private bool TargetInDistance()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        return isConcealed ? distance < activeDistance : distance < chaseDistance;
    }

    private void LaunchEnemy()
    {
        isLaunching = true; // Set the flag to indicate that the enemy is launching

        // Calculate the direction vector from the enemy to the target
        Vector2 directionToTarget = (target.position - transform.position).normalized;

        // If the target is to the left of the enemy, flip the launch angle horizontally
        float adjustedLaunchAngle = launchAngle;
        if (directionToTarget.x < 0)
        {
            adjustedLaunchAngle = 180 - launchAngle;
        }

        // Calculate the launch direction based on the adjusted launch angle
        Vector2 launchDirection = Quaternion.Euler(0, 0, adjustedLaunchAngle) * Vector2.right;

        // Call your method to launch the enemy using the calculated launch direction
        LaunchLogic(launchDirection);

        StartCoroutine(LaunchCooldown()); // Start the cooldown coroutine
    }
    private void LaunchLogic(Vector2 launchDirection)
    {
        // Convert launch angle to radians
        float angleInRadians = launchAngle * Mathf.Deg2Rad;

        // Calculate launch force based on distance to target
        float launchForce = Mathf.Clamp(distanceToTarget, minLaunchForce, maxLaunchForce);

        // Apply launch force to the enemy
        rb.velocity = launchDirection.normalized * launchForce;
    }

    private IEnumerator LaunchCooldown()
    {
        yield return new WaitUntil(() => IsGrounded()); // Wait until the enemy becomes grounded again
        yield return new WaitForSeconds(launchCooldownTime);
        isLaunching = false; // Reset the flag after the cooldown period
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Attack();
        }
    }
    public void Attack()
    {
        health.TakeDamage(attackDamage);
        DamageCooldown();
    }
    private IEnumerator DamageCooldown()
    {      
        yield return new WaitForSeconds(2f);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}