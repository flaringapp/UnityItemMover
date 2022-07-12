using System.Collections;
using UnityEngine;

namespace Controllers
{
    public class PlayerHandController : MonoBehaviour
    {
        public PlayerAheadObjectProvider aheadObjectProvider;

        private GameObject _pickedItem;
        private bool IsItemPicked => _pickedItem != null;

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

        private void PickItem()
        {
            var itemAhead = aheadObjectProvider.ObjectAhead;
            print("Item ahead: " + itemAhead);
            if (ReferenceEquals(itemAhead, null)) return;

            _pickedItem = itemAhead;

            print("Item picked: " + itemAhead);
        }

        private void DropItem()
        {
            var pickedItem = _pickedItem;
            if (ReferenceEquals(pickedItem, null)) return;

            _pickedItem = null;

            print("Item dropped: " + pickedItem);
        }
    }
}