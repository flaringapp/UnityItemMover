using System.Collections;
using UnityEngine;

namespace Controllers
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private float xSensitivity = 1f;
        [SerializeField] private float ySensitivity = 1f;

        [SerializeField] private Transform playerRotation;

        private float _xRotation;
        private float _yRotation;

        private void Start()
        {
            StartCoroutine(CameraMovementListener());
        }

        private IEnumerator CameraMovementListener()
        {
            while (true)
            {
                var mouseX = Input.GetAxis("Mouse X") * xSensitivity;
                var mouseY = Input.GetAxis("Mouse Y") * ySensitivity;

                _yRotation += mouseX;
                _xRotation -= mouseY;
                _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

                transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
                playerRotation.rotation = Quaternion.Euler(0, _yRotation, 0);

                yield return null;
            }
        }
    }
}