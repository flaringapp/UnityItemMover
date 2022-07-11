using System.Collections;
using UnityEngine;

namespace Controllers
{
    public class MoveCamera : MonoBehaviour
    {

        public Transform dependency;

        private void Start()
        {
            StartCoroutine(MoveCameraCoroutine());
        }

        private IEnumerator MoveCameraCoroutine()
        {
            while (true)
            {
                transform.position = dependency.position;
                yield return null;
            }
        }
    }
}
