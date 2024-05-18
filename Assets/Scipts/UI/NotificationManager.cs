using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.UIElements;

public class NotificationManager : MonoBehaviour
{
    
    public TMP_Text notificationText;
    public float displayDuration = 2f;
    public float typingSpeed = 0.1f;
    public float cursorBlinkRate = 0.3f;

    private Coroutine currentNotificationCoroutine;
    private Coroutine cursorBlinkCoroutine;
    private bool isCursorVisible = true;

    public void ShowNotification(string message)
    {
        if (currentNotificationCoroutine != null)
        {
            StopCoroutine(currentNotificationCoroutine);
        }
        currentNotificationCoroutine = StartCoroutine(DisplayNotification(message));
    }

    private IEnumerator DisplayNotification(string message)
    {
        notificationText.text = "";
        
        if (cursorBlinkCoroutine != null)
        {
            StopCoroutine(cursorBlinkCoroutine);
        }
        cursorBlinkCoroutine = StartCoroutine(CursorBlink());

        for (int i = 0; i <= message.Length; i++)
        {
            notificationText.text = message.Substring(0, i) + (isCursorVisible ? "_" : "");
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(displayDuration);

        yield return StartCoroutine(ReverseTyping(message));
        notificationText.text = "";
    }
    private IEnumerator ReverseTyping(string message)
    {
        if (cursorBlinkCoroutine != null)
        {
            StopCoroutine(cursorBlinkCoroutine);
        }
        cursorBlinkCoroutine = StartCoroutine(CursorBlink());

        for (int i = message.Length; i >= 0; i--)
        {
            notificationText.text = message.Substring(0, i) + (isCursorVisible ? "_" : "");
            yield return new WaitForSeconds(typingSpeed);
        }

        if (cursorBlinkCoroutine != null)
        {
            StopCoroutine(cursorBlinkCoroutine);
        }
        notificationText.text = "";
    }
    private IEnumerator CursorBlink()
    {
        while (true)
        {
            isCursorVisible = !isCursorVisible;
            if (notificationText.text.Length > 0)
            {
                if (isCursorVisible)
                {
                    notificationText.text = notificationText.text + "_";
                }
                else
                {
                    notificationText.text = notificationText.text.TrimEnd('_');
                }
            }
            yield return new WaitForSeconds(cursorBlinkRate);
        }
    }
}