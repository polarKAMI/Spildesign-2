using UnityEngine;
using UnityEngine.UI;

public class PerspectiveScalingAndOpacity : MonoBehaviour
{
    private Vector3 initialScale;
    private float minZ = 0f;
    private float maxZ = 360f;
    private float minScale = 1f;
    private float maxScale = 0.3f;
    private float minAlpha = 1f;
    private float maxAlpha = 0f;
    private float alphaThreshold = 109f;

    private Image image;

    void Start()
    {
        initialScale = transform.localScale;
        image = GetComponent<Image>();
    }

    void Update()
    {
        // Get the current Z position of the image
        float currentZ = transform.localPosition.z;

        // Calculate the scale factor based on Z position
        float scaleFactor = Mathf.InverseLerp(minZ, maxZ, currentZ);
        float scale = Mathf.Lerp(minScale, maxScale, scaleFactor);

        // Calculate the alpha factor based on Z position
        float alphaFactor = Mathf.Clamp01((currentZ - minZ) / alphaThreshold);
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, alphaFactor);

        // Apply the scale value to the image's scale
        transform.localScale = new Vector3(initialScale.x * scale, initialScale.y * scale, initialScale.z);

        // Apply the alpha value to the image's color
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
}