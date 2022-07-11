using System.Collections;
using UnityEngine;

namespace Controllers
{
    public class PlayerHandController : MonoBehaviour
    {
        public Camera playerCamera;
        public float maxDistance;
        
        private GameObject _objectAhead;
        
        private void Start()
        {
            StartCoroutine(FindObjectAhead());
        }

        private IEnumerator FindObjectAhead()
        {
            while (true)
            {
                var position = new Vector2(Screen.width / 2f, Screen.height / 2f);
                var ray = playerCamera.ScreenPointToRay(position, Camera.MonoOrStereoscopicEye.Mono);

                if (Physics.Raycast(ray, out var hitPoint, maxDistance))
                {
                    _objectAhead = hitPoint.collider.gameObject;
                    print("Object ahead: " + _objectAhead.name);
                }

                yield return new WaitForFixedUpdate();
            }
        }
    }
}
