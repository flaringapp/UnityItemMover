using System.Collections;
using UnityEngine;

namespace Controllers
{
    public class ItemInHandController : MonoBehaviour
    {
        public bool HasItem => _itemMoveCoroutine != null;

        [SerializeField] private Camera playerCamera;
        [SerializeField] private Transform handTransform;
        [SerializeField] private float moveThreshold;
        [SerializeField] private float moveInterpolation;
        [SerializeField] private float maxMoveSpeed;

        private Rigidbody _itemRigidbody;
        private Coroutine _itemMoveCoroutine;

        public bool TakeItem(Rigidbody itemRigidbody)
        {
            if (HasItem) return false;
            _itemRigidbody = itemRigidbody;

            _itemMoveCoroutine = StartCoroutine(MoveItemWithHand(itemRigidbody));

            itemRigidbody.isKinematic = true;

            print("Item picked: " + itemRigidbody.name);

            return true;
        }

        public bool DropItem()
        {
            if (!HasItem) return false;

            StopCoroutine(_itemMoveCoroutine);
            _itemMoveCoroutine = null;

            _itemRigidbody.isKinematic = false;
            print("Item dropped: " + _itemRigidbody.name);
            _itemRigidbody = null;

            return true;
        }

        private IEnumerator MoveItemWithHand(Rigidbody itemRigidbody)
        {
            var distanceToItem = Vector3.Distance(handTransform.position, itemRigidbody.position);

            while (true)
            {
                MoveItemTowardsCursor(itemRigidbody, distanceToItem);
                yield return CoroutineUtils.CoroutineFixedUpdate;
            }
        }

        private void MoveItemTowardsCursor(Rigidbody itemRigidbody, float distance)
        {
            var moveToPosition = FindPositionWhereToMoveItem(itemRigidbody, distance);

            var distanceFromItemToPosition = Vector3.Distance(moveToPosition, itemRigidbody.position);
            if (distanceFromItemToPosition < moveThreshold) return;

            var interpolatedPosition = Vector3.Lerp(itemRigidbody.position, moveToPosition, moveInterpolation);
            interpolatedPosition = LimitPositionMaxSpeed(itemRigidbody.position, interpolatedPosition);
            
            itemRigidbody.MovePosition(interpolatedPosition);
        }

        private Vector3 FindPositionWhereToMoveItem(Rigidbody itemRigidbody, float distance)
        {
            var screenPoint = new Vector3(ScreenUtils.ScreenCenterPos.x, ScreenUtils.ScreenCenterPos.y, distance);
            var lookAtPoint = playerCamera.ScreenToWorldPoint(screenPoint);

            var itemPosition = itemRigidbody.position;
            var moveDirection = itemPosition.DirectionTo(lookAtPoint);
            var moveDistance = Vector3.Distance(itemPosition, lookAtPoint);

            var moveToPositionRay = new Ray(itemPosition, moveDirection);
            var hasSomethingOnMoveWay = Physics.Raycast(moveToPositionRay, out var hit, moveDistance);
            return hasSomethingOnMoveWay ? hit.point : lookAtPoint;
        }

        private Vector3 LimitPositionMaxSpeed(Vector3 currentPosition, Vector3 moveToPosition)
        {
            var moveDistance = Vector3.Distance(currentPosition, moveToPosition);
            var moveSpeed = moveDistance / Time.fixedDeltaTime;
            if (moveSpeed <= maxMoveSpeed) return moveToPosition;

            var moveDirection = currentPosition.DirectionTo(moveToPosition);
            var maxDistance = maxMoveSpeed * Time.fixedDeltaTime;

            return currentPosition + maxDistance * moveDirection;
        }
    }
}