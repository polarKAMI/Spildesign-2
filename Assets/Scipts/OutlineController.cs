using UnityEngine;

public class OutlineController : MonoBehaviour
{
    public Transform player;  // Reference to the player's transform
    public float proximityDistance = 1.5f;  // Distance at which the outline should appear
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
            float distance = Vector3.Distance(player.position, transform.position);
            material.SetFloat("_DistanceToPlayer", distance);
            material.SetFloat("_ProximityDistance", proximityDistance);
        }
    }
}