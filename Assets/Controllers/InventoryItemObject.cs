using ScriptableObjects;
using UnityEngine;

namespace Controllers
{
    public class InventoryItemObject : MonoBehaviour
    {
        [SerializeField] private InventoryItemData data;

        public void HandleBeingPickedUp()
        {
            InventoryManager.current.Add(data);
            Destroy(gameObject);
        }
    }
}