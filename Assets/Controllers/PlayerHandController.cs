using System.Collections;
using UnityEngine;

namespace Controllers
{
    public class PlayerHandController : MonoBehaviour
    {
        [SerializeField] private PlayerAheadObjectProvider aheadObjectProvider;
        [SerializeField] private ItemInHandController itemInHandController;

        private bool HasItem => itemInHandController.HasItem;

        private void Start()
        {
            StartCoroutine(ObserveHandInteraction());
        }

        private IEnumerator ObserveHandInteraction()
        {
            while (true)
            {
                yield return CoroutineUtils.WaitForKeyDown(KeyCode.Mouse0);
                if (HasItem) continue;
                PickItem();
                yield return ObserveInHandInteraction();
                yield return null;
            }
        }

        private IEnumerable ObserveInHandInteraction()
        {
            while (true)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                    DropItem();
                else if (Input.GetKeyDown(KeyCode.E))
                    TakeItemIntoInventory();
                else
                    yield return null;

                break;
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void PickItem()
        {
            var itemAhead = aheadObjectProvider.ObjectAhead;
            if (ReferenceEquals(itemAhead, null)) return;

            var itemRigidbody = itemAhead.GetComponent<Rigidbody>();
            itemInHandController.TakeItem(itemRigidbody);
        }

        private void DropItem()
        {
            itemInHandController.DropItem();
        }

        private void TakeItemIntoInventory()
        {
            var item = itemInHandController.CurrentItem;
            item.TryGetComponent<InventoryItemObject>(out var inventoryItemObject);
            inventoryItemObject.HandleBeingPickedUp();
        }
    }
}