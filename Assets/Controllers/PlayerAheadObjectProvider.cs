using System.Collections;
using UnityEngine;

namespace Controllers
{
    
    public class PlayerAheadObjectProvider : MonoBehaviour
    {
        public LayerMask itemMask;
        public Camera playerCamera;
        public float maxDistance;
        
        private readonly Vector2 _screenCenterPos = new(Screen.width / 2f, Screen.height / 2f);
        
        public GameObject ObjectAhead { get; private set; }

        private void Start()
        {
            StartCoroutine(FindObjectAhead());
        }

        private IEnumerator FindObjectAhead()
        {
            while (true)
            {
                var ray = playerCamera.ScreenPointToRay(_screenCenterPos);

                if (Physics.Raycast(ray, out var hitPoint, maxDistance, itemMask))
                {
                    ProcessFoundObject(hitPoint.collider.gameObject);
                }
                else if (!ReferenceEquals(ObjectAhead, null))
                {
                    print("Object ahead lost: " + ObjectAhead.name);
                    ObjectAhead = null;
                }

                yield return new WaitForFixedUpdate();
            }
        }

        private void ProcessFoundObject(GameObject foundObjectAhead)
        {
            if (ReferenceEquals(ObjectAhead, foundObjectAhead)) return;
            ObjectAhead = foundObjectAhead;
            
            print("Object ahead: " + ObjectAhead.name);
        }
    }
}
