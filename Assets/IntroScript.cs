using UnityEngine;
using TMPro; // Add the TextMeshPro namespace
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroScript : MonoBehaviour
{
    public TMP_Text uiText; // Use TMP_Text for TextMeshPro
    public TMP_Text timeText; // Reference to the second TMP_Text for the time
    public float typingSpeed = 0.05f; // Speed at which characters are displayed
    [TextArea(3, 10)]
    public string message; // Message to be displayed, editable in the Inspector
    public string newTime = "17:47"; // New time to display after typing

    void Start()
    {
        if (uiText == null)
        {
            Debug.LogError("Text component is not assigned.");
            return;
        }

        if (timeText == null)
        {
            Debug.LogError("Time text component is not assigned.");
            return;
        }


        // Start the typing effect with the specified message
        StartTyping(message);
    }

    public void StartTyping(string message)
    {
        StartCoroutine(TypeMessage(message));
    }

    private IEnumerator TypeMessage(string message)
    {
        uiText.text = ""; // Clear the existing text
        foreach (char letter in message.ToCharArray())
        {
            uiText.text += letter; // Add one letter at a time
            yield return new WaitForSeconds(typingSpeed); // Wait before adding the next letter
        }

        // Update the timeText after the message has finished typing
        timeText.text = newTime;

        yield return new WaitForSeconds(4f);

        // Load the next scene
        SceneManager.LoadScene("SampleScene");


    }

   
}
