using UnityEditor;
using UnityEngine;

public static class AssetUtility
{
    public static T CreateOrFetchAsset<T>(string assetName) where T : ScriptableObject
    {
        string assetPath = $"Assets/{assetName}";
        T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
        if (asset == null)
        {
            asset = ScriptableObject.CreateInstance<T>();

            if (asset is ShapeSettings shapeSettings)
            {
                InitializeShapeSettings(shapeSettings);
            }

            AssetDatabase.CreateAsset(asset, assetPath);
            AssetDatabase.SaveAssets();
        }
        else if (asset is ShapeSettings shapeSettings && (shapeSettings.NoiseLayers == null || shapeSettings.NoiseLayers.Length == 0))
        {
            InitializeShapeSettings(shapeSettings);
            EditorUtility.SetDirty(shapeSettings);
            AssetDatabase.SaveAssets();
        }

        return asset;
    }

    private static void InitializeShapeSettings(ShapeSettings shapeSettings)
    {
        shapeSettings.NoiseLayers = new ShapeSettings.NoiseLayer[1]
        {
            new()
            {
                Enabled = true,
                NoiseSettings = new NoiseSettings(),
                UseFirstLayerAsMask = false
            }
        };
    }
}