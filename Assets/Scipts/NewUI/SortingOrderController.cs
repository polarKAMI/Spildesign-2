using UnityEngine;

public class SortingOrderController : MonoBehaviour
{
    public float zThreshold = 0f; // Threshold value to determine hierarchy position

    public void SortItemUIByZPosition()
    {
        // Get all UI elements with the ItemUI component
        ItemUI[] itemUIs = FindObjectsOfType<ItemUI>();

        foreach (ItemUI itemUI in itemUIs)
        {
            RectTransform rectTransform = itemUI.GetComponent<RectTransform>();

            if (rectTransform != null)
            {
                // Determine hierarchy position based on Z position
                if (rectTransform.position.z > zThreshold)
                {
                    // Move UI element to the back
                    rectTransform.SetAsFirstSibling();
                }
                else
                {
                    // Move UI element to the front
                    rectTransform.SetAsLastSibling();
                }
            }
        }
    }
}