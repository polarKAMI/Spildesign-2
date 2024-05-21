using UnityEngine;

public class OutlineController : MonoBehaviour
{
    public Transform player;  // Reference to the player's transform
    public float proximityDistance = 5.0f;  // Distance at which the outline should appear
    private Material material;

    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            material = spriteRenderer.material;
        }
    }

    void Update()
    {
        if (player != null && material != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            material.SetFloat("_DistanceToPlayer", distance);
            material.SetFloat("_ProximityDistance", proximityDistance);
        }
    }
}