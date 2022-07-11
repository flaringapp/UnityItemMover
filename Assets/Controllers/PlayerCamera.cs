using System.Collections;
using UnityEngine;

namespace Controllers
{
    public class PlayerCamera : MonoBehaviour
    {

        public float xSensitivity = 1f;
        public float ySensitivity = 1f;

        public Transform playerRotation;

        public float xRotation;
        public float yRotation;
    
        void Start()
        {
            StartCoroutine(CameraMovementListener());
        }

        private IEnumerator CameraMovementListener()
        {
            while (true)
            {
                var mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * xSensitivity;
                var mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * ySensitivity;

                yRotation += mouseX;
                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            
                transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
                playerRotation.rotation = Quaternion.Euler(0, yRotation, 0);
            
                yield return null;
            }
        }
    }
}
