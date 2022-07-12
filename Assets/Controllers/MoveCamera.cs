using System.Collections;
using UnityEngine;

namespace Controllers
{
    public class MoveCamera : MonoBehaviour
    {
        [SerializeField] private Transform dependency;

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