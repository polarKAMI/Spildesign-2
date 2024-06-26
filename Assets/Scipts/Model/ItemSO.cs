using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class ItemSO : ScriptableObject, IUsable
    {
        public int ID => GetInstanceID();

        [field: SerializeField]
        public string Name { get; set; }

        [field: SerializeField]
        [field: TextArea]
        public string Description { get; set; }

        [field: SerializeField]
        public Sprite ItemImage { get; set; }

        public virtual void Use()
        {
            // Default behavior for using the item
            Debug.Log($"Using {Name}.");
        }
    }
}