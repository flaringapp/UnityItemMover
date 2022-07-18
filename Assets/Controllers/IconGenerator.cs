using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ScriptableObjects;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

namespace Controllers
{
    public class IconGenerator : MonoBehaviour
    {
        [SerializeField] private Camera captureCamera;
        [SerializeField] private int iconResolution;
        [SerializeField] private TextureDepth iconDepth;
        [SerializeField] private TextureFormat iconTextureFormat;
        [SerializeField] private string iconDirectoryRelativePath;
        [SerializeField] private Preset iconImportPreset;

        [SerializeField] private List<ItemMapping> items;

        private string _assetsIconDirectoryPath;

        private void Awake()
        {
            Physics.autoSimulation = false;

            _assetsIconDirectoryPath = Path.Combine("Assets", iconDirectoryRelativePath);

            var isValidFolder = AssetDatabase.IsValidFolder(_assetsIconDirectoryPath);
            if (!isValidFolder)
                Debug.LogError($"Icon generator directory path is invalid! {_assetsIconDirectoryPath}");
        }

        public void GenerateIcons()
        {
            Awake();
            StartCoroutine(GenerateAllIcons());
        }

        private IEnumerator GenerateAllIcons()
        {
            items.ForEach(item => item.ItemObject.SetActive(false));
            yield return null;

            yield return items.Select(GenerateSingleIcon).GetEnumerator();
            
            AssetDatabase.Refresh();
        }

        private IEnumerator GenerateSingleIcon(ItemMapping item)
        {
            item.ItemObject.SetActive(true);
            yield return null;

            var iconFilePath = Path.Combine(_assetsIconDirectoryPath, $"{item.Data.id}.png");
            TakeScreenshot(iconFilePath);
            yield return null;

            item.ItemObject.SetActive(false);

            var iconSprite = AssetDatabase.LoadAssetAtPath<Sprite>(iconFilePath);
            if (ReferenceEquals(iconSprite, null))
            {
                Debug.LogError($"Cannot generate icon sprite for icon {item.Data.id}");
                yield break;
            }

            item.Data.icon = iconSprite;
            EditorUtility.SetDirty(item.Data);

            yield return null;
        }

        private void TakeScreenshot(string filePath)
        {
            var cameraTexture = new RenderTexture(iconResolution, iconResolution, iconDepth.Value());
            captureCamera.targetTexture = cameraTexture;
            captureCamera.Render();

            var iconTexture = new Texture2D(iconResolution, iconResolution, iconTextureFormat, false);
            RenderTexture.active = cameraTexture;
            iconTexture.ReadPixels(new Rect(0, 0, iconResolution, iconResolution), 0, 0);

            captureCamera.targetTexture = null;
            RenderTexture.active = null;

            cameraTexture.DestroyImmediateInEditor();

            var bytes = iconTexture.EncodeToPNG();
            File.WriteAllBytes(filePath, bytes);

            AssetDatabase.Refresh();

            var iconImporter = (TextureImporter) AssetImporter.GetAtPath(filePath);
            iconImportPreset.ApplyTo(iconImporter);
            iconImporter.SaveAndReimport();
        }

        [Serializable]
        private class ItemMapping
        {
            [field: SerializeField] public GameObject ItemObject { get; private set; }
            [field: SerializeField] public InventoryItemData Data { get; private set; }
        }
    }
}