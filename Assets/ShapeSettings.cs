using UnityEngine;

[CreateAssetMenu(fileName = "ShapeSettings", menuName = "Settings/ShapeSettings")]
public class ShapeSettings : ScriptableObject
{
    public float PlanetRadius = 5f;

    [Range(2, 100)]
    public int Resolution = 15;

    public NoiseLayer[] NoiseLayers;

    [System.Serializable]
    public class NoiseLayer
    {
        public bool Enabled = true;
        public NoiseSettings NoiseSettings;
        public bool UseFirstLayerAsMask;
    }
}