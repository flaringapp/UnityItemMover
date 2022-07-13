using System.Collections;
using UnityEngine;

namespace Controllers
{
    public static class CoroutineUtils
    {
        public static readonly YieldInstruction CoroutineFixedUpdate = new WaitForFixedUpdate();

        public static IEnumerator WaitForKeyDown(KeyCode keyCode)
        {
            while (!Input.GetKeyDown(keyCode)) yield return null;
        }
        
        public static IEnumerator WaitForKeyUp(KeyCode keyCode)
        {
            while (!Input.GetKeyUp(keyCode)) yield return null;
        }
    }
}