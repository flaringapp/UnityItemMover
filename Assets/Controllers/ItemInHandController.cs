using System.Collections;
using UnityEngine;

namespace Controllers
{
    public class ItemInHandController : MonoBehaviour
    {
        public bool HasItem => _itemRigidbody != null;

        [SerializeField] private Camera playerCamera;
        [SerializeField] private Transform handTransform;
        
        [SerializeField] private float moveThreshold;
        [SerializeField] private float moveInterpolation;
        
        [SerializeField] private float maxDropSpeed;

        private Rigidbody _itemRigidbody;
        private Vector3 _lastItemDistanceChange = Vector3.zero;
        private Coroutine _itemMoveCoroutine;

        public bool TakeItem(Rigidbody itemRigidbody)
        {
            if (HasItem) return false;
            
            _itemRigidbody = itemRigidbody;
            _lastItemDistanceChange = Vector3.zero;

            itemRigidbody.useGravity = false;
            
            _itemMoveCoroutine = StartCoroutine(MoveItemWithHand(itemRigidbody));

            print("Item picked: " + itemRigidbody.name);

            return true;
        }

        public bool DropItem()
        {
            if (!HasItem) return false;

            StopCoroutine(_itemMoveCoroutine);
            _itemMoveCoroutine = null;

            _itemRigidbody.useGravity = true;
            
            ApplyForceToItemOnDrop();

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
            var itemPosition = itemRigidbody.position;
            var moveToPosition = FindPositionWhereToMoveItem(itemPosition, distance);

            var distanceFromItemToPosition = Vector3.Distance(itemPosition, moveToPosition);
            if (distanceFromItemToPosition < moveThreshold) return;

            var interpolatedPosition = Vector3.Lerp(itemPosition, moveToPosition, moveInterpolation);

            _lastItemDistanceChange = interpolatedPosition - itemRigidbody.position;

            itemRigidbody.MovePosition(interpolatedPosition);
        }

        private Vector3 FindPositionWhereToMoveItem(Vector3 itemPosition, float distance)
        {
            var screenPoint = new Vector3(ScreenUtils.ScreenCenterPos.x, ScreenUtils.ScreenCenterPos.y, distance);
            var lookAtPoint = playerCamera.ScreenToWorldPoint(screenPoint);

            var moveDirection = itemPosition.DirectionTo(lookAtPoint);
            var moveDistance = Vector3.Distance(itemPosition, lookAtPoint);

            var moveToPositionRay = new Ray(itemPosition, moveDirection);
            var hasSomethingOnMoveWay = Physics.Raycast(moveToPositionRay, out var hit, moveDistance);
            return hasSomethingOnMoveWay ? hit.point : lookAtPoint;
        }

        private void ApplyForceToItemOnDrop()
        {
            if (_lastItemDistanceChange == Vector3.zero) return;
            if (Time.fixedDeltaTime == 0) return;

            var velocity = _lastItemDistanceChange / Time.fixedDeltaTime;
            var limitedSpeedVelocity = velocity.LimitVelocitySpeed(maxDropSpeed);
            _itemRigidbody.AddForce(limitedSpeedVelocity, ForceMode.VelocityChange);
        }
    }
}