using UnityEngine;
using static UnityEngine.Object;

namespace Controllers
{
    public static class EditorUtils
    {
        
        public static void DestroyImmediateInEditor(this Object gameObject)
        {
            if (Application.isEditor) DestroyImmediate(gameObject);
            else Destroy(gameObject);
        }
    }
}