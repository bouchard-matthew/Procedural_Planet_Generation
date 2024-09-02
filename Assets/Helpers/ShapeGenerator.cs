using UnityEngine;

public class ShapeGenerator
{
    ShapeSettings ShapeSettings;
    Noise Noise;
    public ShapeGenerator(ShapeSettings settings)
    {
        ShapeSettings = settings;
        Noise = new();
    }

    public void UpdateSettings(ShapeSettings shapeSettings)
    {
        ShapeSettings = shapeSettings;
    }

    public Vector3 CalculatePointOnUnitPlanetShape(Vector3 pointOnUnitSphere)
    {
        float firstLayerValue = 0;
        float elevation = 0;

        if (ShapeSettings.NoiseLayers.Length > 0)
        {
            firstLayerValue = NoiseUtility.CalculateElevation(pointOnUnitSphere, Noise, ShapeSettings.NoiseLayers[0].NoiseSettings);
            if (ShapeSettings.NoiseLayers[0].Enabled)
            {
                elevation = firstLayerValue;
            }
        }

        for (int i = 0; i < ShapeSettings.NoiseLayers.Length; i++)
        {
            if (ShapeSettings.NoiseLayers[i].Enabled)
            {
                float mask = (ShapeSettings.NoiseLayers[i].UseFirstLayerAsMask) ? firstLayerValue : 1;
                elevation += NoiseUtility.CalculateElevation(pointOnUnitSphere, Noise, ShapeSettings.NoiseLayers[i].NoiseSettings) * mask;
            }
        }
        return (1 + elevation) * ShapeSettings.PlanetRadius * pointOnUnitSphere;
    }
}