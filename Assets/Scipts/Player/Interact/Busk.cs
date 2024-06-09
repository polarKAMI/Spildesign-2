using System.Collections;
using UnityEngine;

public class Busk : MonoBehaviour, IInteractable
{
    public KeyCode enterKey = KeyCode.F; // Key to press to enter the layer
    public LayerMask targetLayer; // Layer to enter
    public bool canEnter = false; // Flag to check if the player can enter the layer
    public bool isin = false;
    private Vector3 originalPosition; // Store the original position
    public SpriteRenderer spriteRenderer; // Reference to the sprite renderer component

    private AudioSource audioSource;
    public AudioClip Busklyd;

    void Start()
    {
        originalPosition = transform.position; // Store the original position
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the sprite renderer component
        audioSource = GetComponent<AudioSource>();
    }
    public void Interact()
    {
        EnterLayer();
        PlayIndexEnterSound();
        Debug.Log("Har trykket E for busk");
    }

    void EnterLayer()
    {
       
       // er inde 
        isin = true;
        Debug.Log("Er inde i busk");

        // Change the z value of the position busken kommer foran
        Vector3 newPosition = originalPosition;
        newPosition.z = -0.6f;
        transform.position = newPosition;

        // Set the alpha level to 100 den bliver gennemsigtig
        Color color = spriteRenderer.color;
        color.a = 0.5f;
        spriteRenderer.color = color;
    }

    void OnTriggerEnter2D(Collider2D other) // ved ikke hvad g'r
    {
        // Check if the player enters the trigger
        if (other.CompareTag("Player"))
        {
            canEnter = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) // ved ikke gvad g'r
    {
        // Check if the player exits the trigger
        if (other.CompareTag("Player"))
        {
            canEnter = false;
            
            isin = false;
            Debug.Log("Er ude af busk");

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


    private void PlayIndexEnterSound()
    {


        audioSource.PlayOneShot(Busklyd);

    }
    // bush er forkert
}
