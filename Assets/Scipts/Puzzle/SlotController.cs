using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class SlotController : MonoBehaviour
{
    public ItemSlot[] itemSlots; // Array to store references to all item slots
    private bool[] isSlotMatched; // Array to store the match status of each slot
    public Animator animator;
    public PlayerController playerController;
    private bool once;
    public GameObject endScreen;
    private void Start()
    {
        // Initialize the array to store the match status of each slot
        isSlotMatched = new bool[itemSlots.Length];
        
    }

    private void Update()
    {
        // Check if all item slots have a match
        bool allSlotsMatched = true;
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (!itemSlots[i].isMatch)
            {
                allSlotsMatched = false;
                break;
            }
        }


        // If all slots match, execute the OpenDoor function
        if (allSlotsMatched && !once)
        {
            once = true;
            animator.SetBool("KeyPlaced", true);
            foreach (var slot in itemSlots)
            {
                slot.SetDoorOpened(true);
            }
            PlayerMovement playermovement = FindAnyObjectByType<PlayerMovement>();
            if (playermovement != null)
            {
                playermovement.DisableMovement();
                StartCoroutine(EndGame());
            }
        }
    }

    private IEnumerator EndGame()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        CanvasGroup endScreenCanvasGroup = endScreen.GetComponent<CanvasGroup>();
        float startAlpha = 0f;
        // Final alpha value
        float targetAlpha = 1f;
        // Duration of the transition
        float duration = 1f;
        // Time elapsed during the transition
        float elapsedTime = 0f;
        // Close inventory if open
        if (playerController.isInventoryOpen)
        {
            playerController.ToggleInventory();
        }
        yield return new WaitForSeconds(5f);
        while (elapsedTime < duration)
        {
            // Calculate the current alpha value based on the lerp function
            float currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);

            // Set the alpha value of the CanvasGroup component
            endScreenCanvasGroup.alpha = currentAlpha;

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;
        }
        endScreenCanvasGroup.alpha = targetAlpha;
        yield return new WaitForSeconds(8f); // Adjust the delay duration as needed
        // Load the specified scene
        LoadScene("MainMenu");
    }
    private void LoadScene(string sceneName)
    {
        // Load the specified scene
        SceneManager.LoadScene(sceneName);
    }
}