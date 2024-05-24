using UnityEngine;
using TMPro; // Add the TextMeshPro namespace
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroScript : MonoBehaviour
{
    public TMP_Text uiText; // First TextMeshPro text component
    public TMP_Text timeText; // Second TextMeshPro text component for the time
    public TMP_Text logText; // Third TextMeshPro text component for the second message
    public float typingSpeed = 0.05f; // Speed at which characters are displayed
    [TextArea(3, 10)]
    public string message; // First message to be displayed
    public string newTime = "17:47"; // New time to display after the first message
    [TextArea(3, 10)]
    public string secondaryMessage; // Second message to be displayed
    public float pauseDuration = 0.5f; // Duration for each pause

    void Start()
    {
        if (uiText == null || timeText == null || logText == null)
        {
            Debug.LogError("One or more Text components are not assigned.");
            return;
        }

        // Start the typing effect with the specified message
        StartTyping(message, uiText, () =>
        {
            // Update the timeText after the first message has finished typing
            timeText.text = newTime;

            // Start typing the second message in the secondary text box
            StartTyping(secondaryMessage, logText, () =>
            {
                StartCoroutine(LoadNextSceneAfterDelay(4f));
            });
        });
    }

    public void StartTyping(string message, TMP_Text textComponent, System.Action onComplete)
    {
        StartCoroutine(TypeMessage(message, textComponent, onComplete));
    }

    private IEnumerator TypeMessage(string message, TMP_Text textComponent, System.Action onComplete)
    {
        textComponent.text = ""; // Clear the existing text
        foreach (char letter in message.ToCharArray())
        {
            if (letter == '|')
            {
                yield return new WaitForSeconds(pauseDuration); // Pause when encountering '|'
            }
            else
            {
                textComponent.text += letter; // Add one letter at a time
                yield return new WaitForSeconds(typingSpeed); // Wait before adding the next letter
            }
        }

        onComplete?.Invoke();
    }

    private IEnumerator LoadNextSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("SampleScene");
    }
}
