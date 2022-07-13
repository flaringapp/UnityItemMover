using System.Collections;
using UnityEngine;

namespace Controllers
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private float speed;

        [SerializeField] private Transform playerTransform;
        [SerializeField] private CharacterController playerController;

        private void Start()
        {
            StartCoroutine(MovePlayer());
        }

        private IEnumerator MovePlayer()
        {
            while (true)
            {
                var x = Input.GetAxisRaw("Horizontal");
                var z = Input.GetAxisRaw("Vertical");

                 var motion = playerTransform.right * x + playerTransform.forward * z;
                playerController.Move(motion * (speed * Time.deltaTime));

                yield return null;
            }
        }
    }
}