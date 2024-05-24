using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float damping;
    [SerializeField] private float shakeMagnitude = 0.04f; // Adjusted for subtle shake
    [SerializeField] private float shakeFrequency = 100f; // Frequency of the shake
    [SerializeField] private float violentShakeMagnitude = 0.2f; // Adjusted for more violent shake
    [SerializeField] private float violentShakeFrequency = 80f; // Frequency of the violent shake

    public Transform target;
    private bool isShaking = false;
    private bool isViolentShaking = false;
    private Vector3 vel = Vector3.zero;

    private void Update()
    {
        Vector3 targetPosition = target.position + offset;
        targetPosition.z = transform.position.z;

        if (!isShaking && !isViolentShaking)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref vel, damping);
        }
    }

    public void StartShake()
    {
        isShaking = true;
        StartCoroutine(Shake(shakeMagnitude, shakeFrequency, false));
    }

    public void StopShake()
    {
        isShaking = false;
        isViolentShaking = false;
        Debug.Log("Shake stopped");
    }

    public void StartViolentShake()
    {
        isViolentShaking = true;
        StartCoroutine(Shake(violentShakeMagnitude, violentShakeFrequency, true));
    }

    private IEnumerator Shake(float magnitude, float frequency, bool isViolent)
    {
        while (isShaking || isViolentShaking) // Continue shaking until explicitly stopped
        {
            float shakeOffsetY = Mathf.Sin(Time.time * frequency) * magnitude;
            float shakeOffsetX = isViolent ? Mathf.Cos(Time.time * frequency) * magnitude * .2f : 0f;
            Vector3 shakeOffset = new Vector3(shakeOffsetX, shakeOffsetY, 0f);

            transform.position = new Vector3(transform.position.x + shakeOffset.x, transform.position.y + shakeOffset.y, transform.position.z);

            yield return null; // Wait for the next frame
        }
    }
}