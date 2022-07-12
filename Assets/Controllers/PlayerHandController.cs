using System.Collections;
using UnityEngine;

namespace Controllers
{
    public class PlayerHandController : MonoBehaviour
    {
        [SerializeField] private PlayerAheadObjectProvider aheadObjectProvider;
        [SerializeField] private ItemInHandController itemInHandController;

        private bool IsItemPicked => itemInHandController.HasItem;

        private void Start()
        {
            StartCoroutine(ObserveHandInteraction());
        }

        private IEnumerator ObserveHandInteraction()
        {
            while (true)
            {
                yield return CoroutineUtils.WaitForKeyDown(KeyCode.F);
                PickOrDropItem();
                yield return null;
            }
        }

        private void PickOrDropItem()
        {
            if (IsItemPicked) DropItem();
            else PickItem();
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
    }
}