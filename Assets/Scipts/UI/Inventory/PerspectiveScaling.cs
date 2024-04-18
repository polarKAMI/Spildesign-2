using UnityEngine;
using UnityEngine.UI;

public class PerspectiveScalingAndOpacity : MonoBehaviour
{
    private Vector3 initialScale;
    private float minZ = 0f;
    private float maxZ = 20f;
    private float minScale = 1f;
    private float maxScale = 0.3f;
    private float minAlpha = 1f;
    private float maxAlpha = 0f;

    private CanvasGroup canvasGroup;

    void Start()
    {
        initialScale = transform.localScale;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        // Get the current Z position of the image
        float currentZ = transform.localPosition.z;

        // Calculate the scale factor based on Z position
        float scaleFactor = Mathf.InverseLerp(minZ, maxZ, currentZ);
        float scale = Mathf.Lerp(minScale, maxScale, scaleFactor);

        // Calculate the alpha based on custom curve
        float alpha = CalculateAlpha(currentZ);

        // Apply the scale value to the image's scale
        transform.localScale = new Vector3(initialScale.x * scale, initialScale.y * scale, initialScale.z);

        // Apply the alpha value to the canvas group's alpha
        canvasGroup.alpha = alpha;
    }

    // Custom function to calculate alpha based on Z position
    private float CalculateAlpha(float z)
    {
        if (z <= 15f)
        {
            // Linear interpolation from 1 to 0.7 between 0 and 3
            return Mathf.Lerp(minAlpha, 0.7f, Mathf.InverseLerp(minZ, 3f, z));
        }
        else
        {
            // Alpha becomes 0 for z greater than 3
            return 0f;
        }
    }
}