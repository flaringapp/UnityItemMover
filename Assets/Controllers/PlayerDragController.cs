using System.Collections;
using UnityEngine;

namespace Controllers
{
    public class PlayerDragController : MonoBehaviour
    {
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float playerHeight;
        [SerializeField] private float groundDrag;

        private float _groundRayLength;
        private bool _isGrounded;

        private Rigidbody _rigidbody;

        // Start is called before the first frame update
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _groundRayLength = playerHeight / 2f + 0.2f;

            StartCoroutine(ObserveIsGrounded());
        }

        private IEnumerator ObserveIsGrounded()
        {
            _isGrounded = Physics.Raycast(transform.position, Vector3.down, _groundRayLength, groundLayer);
            _rigidbody.drag = _isGrounded ? groundDrag : 0f;

            yield return new WaitForFixedUpdate();
        }
    }
}