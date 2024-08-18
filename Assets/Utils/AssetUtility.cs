

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
            AssetDatabase.CreateAsset(asset, assetPath);
            AssetDatabase.SaveAssets();
        }

        return asset;
    }
}
