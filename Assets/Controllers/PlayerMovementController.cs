using System.Collections;
using UnityEngine;

namespace Controllers
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private CharacterController playerController;

        [SerializeField] private float movementSpeed;
        [SerializeField] private float jumpHeight;
        [SerializeField] private float gravity = Physics.gravity.y;

        private Vector3 _moveVelocity = Vector3.zero;
        private bool _hasDoubleJumped;

        private void Start()
        {
            StartCoroutine(MovePlayer());
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
            var x = Input.GetAxisRaw("Horizontal") * movementSpeed;
            var z = Input.GetAxisRaw("Vertical") * movementSpeed;

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
            
            _moveVelocity.y = Mathf.Sqrt(jumpHeight * gravity * -2f);
        }
    }
}