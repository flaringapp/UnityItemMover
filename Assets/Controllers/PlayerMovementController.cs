using System.Collections;
using UnityEngine;

namespace Controllers
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private CharacterController playerController;

        [SerializeField] private float movementSpeed;
        [SerializeField] private float sprintSpeed;
        [SerializeField] private float jumpHeight;
        [SerializeField] private float gravity = Physics.gravity.y;

        private float _jumpVelocity;

        private Vector3 _moveVelocity = Vector3.zero;

        private float _currentSpeed;
        private bool _isSprinting;
        private bool _hasDoubleJumped;

        private void Start()
        {
            _jumpVelocity = Mathf.Sqrt(jumpHeight * gravity * -2f);
            _currentSpeed = movementSpeed;

            StartCoroutine(MovePlayer());
            StartCoroutine(ObserveSprint());
            StartCoroutine(SprintSpeedAdjustment());
        }

        private IEnumerator MovePlayer()
        {
            while (true)
            {
                PerformMovement();
                yield return null;
            }
        }

        private void PerformMovement()
        {
            ProcessGravityForce();
            ProcessSurfaceMovement();
            ProcessJump();

            playerController.Move(_moveVelocity * Time.deltaTime);
            
            
        }

        private void ProcessGravityForce()
        {
            _moveVelocity.y += gravity * Time.deltaTime;
        }

        private void ProcessSurfaceMovement()
        {
            if (!playerController.isGrounded) return;
            
            var x = Input.GetAxisRaw("Horizontal") * _currentSpeed;
            var z = Input.GetAxisRaw("Vertical") * _currentSpeed;

            var move = playerTransform.right * x + playerTransform.forward * z;
            move.y += _moveVelocity.y;
            _moveVelocity = move;
        }

        private void ProcessJump()
        {
            if (!Input.GetButtonDown("Jump")) return;

            var isGrounded = playerController.isGrounded;
            if (_hasDoubleJumped && !isGrounded) return;

            _hasDoubleJumped = !isGrounded;

            _moveVelocity.y = _jumpVelocity;
        }

        private IEnumerator ObserveSprint()
        {
            while (true)
            {
                yield return CoroutineUtils.WaitForKeyDown(KeyCode.LeftShift);
                _isSprinting = true;
                yield return CoroutineUtils.WaitForKeyUp(KeyCode.LeftShift);
                _isSprinting = false;
                yield return null;
            }
        }

        private IEnumerator SprintSpeedAdjustment()
        {
            while (true)
            {
                var targetSpeed = _isSprinting ? sprintSpeed : movementSpeed;
                while (Mathf.Abs(targetSpeed - _currentSpeed) > 0.01f)
                {
                    _currentSpeed = Mathf.Lerp(_currentSpeed, targetSpeed, 0.1f);
                    yield return null;
                }
                yield return null;
            }
        }
    }
}