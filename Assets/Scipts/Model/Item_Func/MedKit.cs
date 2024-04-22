using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Medkit")]
public class MedKit : InventoryItem
{
    public int healthRestoreAmount = 10;
    public int ammoRestoreAmount = 5;

    public override void Use()
    {
        // Implement the specific use behavior for consumable items
        Debug.Log($"Using {itemName} to restore {healthRestoreAmount} health.");

        // Here you can implement the health restore logic, or any other behavior
    }

    public override void Ammo()
    {
        Debug.Log($"Using {itemName} to restore {ammoRestoreAmount} ammo.");
    }
}