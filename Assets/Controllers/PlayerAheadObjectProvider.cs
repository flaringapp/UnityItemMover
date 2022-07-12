using System.Collections;
using UnityEngine;

namespace Controllers
{
    public class PlayerAheadObjectProvider : MonoBehaviour
    {
        [SerializeField] private LayerMask itemMask;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float maxDistance;

        public GameObject ObjectAhead { get; private set; }

        private void Start()
        {
            StartCoroutine(FindObjectAhead());
        }

        private IEnumerator FindObjectAhead()
        {
            while (true)
            {
                var ray = playerCamera.ScreenPointToRay(ScreenUtils.ScreenCenterPos);

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