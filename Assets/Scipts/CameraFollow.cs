using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float damping;
    [SerializeField] private float shakeMagnitude = 0.04f; // Adjusted for subtle shake
    [SerializeField] private float shakeFrequency = 100f; // Frequency of the shake

    public Transform target;
    private bool isShaking = false;
    private Vector3 vel = Vector3.zero;

    private void Update()
    {
        Vector3 targetPosition = target.position + offset;
        targetPosition.z = transform.position.z;

        if (!isShaking)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref vel, damping);
        }
    }

    public void StartShake()
    {
        isShaking = true;
        StartCoroutine(Shake());
    }

    public void StopShake()
    {
        isShaking = false;
        Debug.Log("Shake stopped");
    }

    private IEnumerator Shake()
    {
        while (isShaking) // Continue shaking until explicitly stopped
        {
            float shakeOffsetY = Mathf.Sin(Time.time * shakeFrequency) * shakeMagnitude;
            Vector3 shakeOffset = new Vector3(0f, shakeOffsetY, 0f);

            transform.position = new Vector3(transform.position.x, transform.position.y + shakeOffset.y, transform.position.z);

            yield return null; // Wait for the next frame
        }
    }
}