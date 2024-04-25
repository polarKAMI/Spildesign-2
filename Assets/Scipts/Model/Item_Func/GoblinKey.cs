using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/KeyItem/GoblinKey")]
public class GoblinKey : InventoryItem
{
    public override void Use()
    {
        // Get the player's collider
        Collider2D playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();

        if (playerCollider != null)
        {
            // Check for overlapping trigger colliders with the player's collider
            Collider2D[] overlappingColliders = Physics2D.OverlapBoxAll(playerCollider.bounds.center, playerCollider.bounds.size, 0f, LayerMask.GetMask("Default"));

            foreach (Collider2D collider in overlappingColliders)
            {
                Door door = collider.gameObject.GetComponent<Door>();

                if (door != null)
                {
                    door.TryOpenDoor(this);
                    return; // Exit after interacting with the first door found
                }
            }

            Debug.Log("No door found in the player's vicinity.");
        }
        else
        {
            Debug.Log("Player collider not found.");
        }
    }
}