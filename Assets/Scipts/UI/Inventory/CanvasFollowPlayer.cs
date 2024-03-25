using UnityEngine;

public class CanvasFollowPlayer : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 offset;

    void LateUpdate()
    {
        // Ensure that playerTransform is assigned
        if (playerTransform == null)
        {
            Debug.LogError("Player Transform is not assigned in the CanvasFollowPlayer script.");
            return;
        }

        // Set the canvas position to match the player's position with an offset
        transform.position = playerTransform.position + offset;
    }
}