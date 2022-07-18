using UnityEditor;
using UnityEngine;

namespace Controllers
{
    [CustomEditor(typeof(IconGenerator))]
    public class IconGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var iconGenerator = (IconGenerator)target;
            if (GUILayout.Button("Generate Icons"))
            {
                iconGenerator.GenerateIcons();
            }
        }
    }
}