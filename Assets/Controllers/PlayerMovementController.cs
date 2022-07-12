using System.Collections;
using UnityEngine;

namespace Controllers
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private Transform rotation;
        [SerializeField] private Vector3 moveDirection = Vector3.zero;

        private Rigidbody _rigidbody;

        private float _horizontalInput;
        private float _verticalInput;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();

            StartCoroutine(ObserveMovementKeys());
            StartCoroutine(MovePlayer());
        }

        private IEnumerator ObserveMovementKeys()
        {
            while (true)
            {
                _horizontalInput = Input.GetAxisRaw("Horizontal");
                _verticalInput = Input.GetAxisRaw("Vertical");

                yield return null;
            }
        }

        private IEnumerator MovePlayer()
        {
            while (true)
            {
                PerformMovement();
                LimitCurrentSpeed();

                yield return CoroutineUtils.CoroutineFixedUpdate;
            }
        }

        private void PerformMovement()
        {
            moveDirection = rotation.forward * _verticalInput + rotation.right * _horizontalInput;
            _rigidbody.AddForce(moveDirection.normalized * speed, ForceMode.Force);
        }

        private void LimitCurrentSpeed()
        {
            var velocity = _rigidbody.velocity;
            var flatVelocity = new Vector3(velocity.x, 0f, velocity.z);

            if (flatVelocity.magnitude <= speed) return;

            var limitedVelocity = flatVelocity.normalized * speed;
            _rigidbody.velocity = new Vector3(limitedVelocity.x, velocity.y, limitedVelocity.z);
        }
    }
}