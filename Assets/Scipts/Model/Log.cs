using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : MonoBehaviour, IInteractable
{
    public LogSO log;
    public NotificationManager notificationManager;
    [SerializeField] private float pickupDuration = 0.3f;
    [SerializeField] private AudioSource audioSource;

    public void Interact()
    {
        PickUp();
    }

    private void PickUp()
    {
        if (!log.Collected)
        {
            LogManager.AddLog(log);
            log.Collected = true;
            notificationManager.ShowNotification("new log;");
        }

        StartCoroutine(AnimatePickup());
    }

    private IEnumerator AnimatePickup()
    {
        // Play pickup sound
        if (audioSource != null)
            audioSource.Play();

        // Disable collider to prevent further interactions
        GetComponent<Collider2D>().enabled = false;

        // Scale down the item
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float currentTime = 0;
        while (currentTime < pickupDuration)
        {
            currentTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, currentTime / pickupDuration);
            yield return null;
        }

        // Destroy the item after pickup animation
        Destroy(gameObject);
    }
}
