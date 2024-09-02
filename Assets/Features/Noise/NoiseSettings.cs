
using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    public float Strength = 1;
    public float Roughness = 2;
    public float Persistence = 0.5f;
    public float BaseRoughness = 1;
    public float MinValue;
    public Vector3 Center;

    [Range(1,8)]
    public int NumberOfLayers = 1;
}
