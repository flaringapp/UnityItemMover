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

        private Vector3 _jumpVelocity = Vector3.zero;

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
            ProcessSurfaceMovement();
            ProcessJumpAndGravity();
        }

        private void ProcessSurfaceMovement()
        {
            var x = Input.GetAxisRaw("Horizontal") * movementSpeed;
            var z = Input.GetAxisRaw("Vertical") * movementSpeed;
            
            if (x == 0 && z == 0) return;

            var move = playerTransform.right * x + playerTransform.forward * z;

            playerController.Move(move * Time.deltaTime);
        }

        private void ProcessJumpAndGravity()
        {
            var isJump = Input.GetButtonDown("Jump");
            
            if (_jumpVelocity.y <= 0 && playerController.isGrounded)
            {
                _jumpVelocity.y = Input.GetButtonDown("Jump") ? Mathf.Sqrt(jumpHeight * gravity * -2f) : -2f;
            }
            else
            {
                if (isJump)
                {
                    print("is grounded: " + playerController.isGrounded + ", vel: " + _jumpVelocity.y);
                }
            }

            _jumpVelocity.y += gravity * Time.deltaTime;

            playerController.Move(_jumpVelocity * Time.deltaTime);
        }
    }
}