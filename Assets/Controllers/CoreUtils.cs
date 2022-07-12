using UnityEngine;

namespace Controllers
{
    public static class CoreUtils
    {
        public static Vector3 DirectionTo(this Vector3 from, Vector3 to)
        {
            return (to - from).normalized;
        }
    }
}