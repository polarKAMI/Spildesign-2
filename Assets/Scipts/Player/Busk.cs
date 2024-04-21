using System.Collections;
using UnityEngine;

public class Busk : MonoBehaviour
{
    public KeyCode enterKey = KeyCode.F; // Key to press to enter the layer
    public LayerMask targetLayer; // Layer to enter
    public bool canEnter = false; // Flag to check if the player can enter the layer
    public bool isin = false;
    private Vector3 originalPosition; // Store the original position
    public SpriteRenderer spriteRenderer; // Reference to the sprite renderer component

    void Start()
    {
        originalPosition = transform.position; // Store the original position
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the sprite renderer component
    }

    void Update()
    {
        if (Input.GetKeyDown(enterKey) && canEnter)
        {
            EnterLayer();
        }
    }

    void EnterLayer()
    {
        // Change the layer of the GameObject to the target layer
        gameObject.layer = LayerMask.NameToLayer("Bush");
        // Disable the script so the player can't re-enter the layer
        enabled = false;
        isin = true;

        // Change the z value of the position
        Vector3 newPosition = originalPosition;
        newPosition.z = -0.6f;
        transform.position = newPosition;

        // Set the alpha level to 100
        Color color = spriteRenderer.color;
        color.a = 0.5f;
        spriteRenderer.color = color;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player enters the trigger
        if (other.CompareTag("Player"))
        {
            canEnter = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Check if the player exits the trigger
        if (other.CompareTag("Player"))
        {
            canEnter = false;
            enabled = true;
            isin = false;

            // Change the z value of the position
            Vector3 newPosition = originalPosition;
            newPosition.z = 0.6f;
            transform.position = newPosition;

            // Set the alpha level to 255
            Color color = spriteRenderer.color;
            color.a = 1f;
            spriteRenderer.color = color;
        }
    }
}
