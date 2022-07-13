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
                yield return CoroutineUtils.WaitForKeyUp(KeyCode.Mouse0);
                DropItem();
                yield return null;
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
    }
}