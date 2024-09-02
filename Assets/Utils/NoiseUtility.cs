using UnityEngine;

public static class NoiseUtility
{
    public static float CalculateElevation(Vector3 point, Noise noise, NoiseSettings noiseSettings)
    {
        float noiseValue = 0;
        float frequency = noiseSettings.BaseRoughness;
        float amplitude = 1;

        for (int i = 0; i < noiseSettings.NumberOfLayers; i++)
        {
            float v = noise.Evaluate(point * frequency + noiseSettings.Center);
            noiseValue += (v + 1) * 0.5f * amplitude;
            frequency *= noiseSettings.Roughness;
            amplitude *= noiseSettings.Persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - noiseSettings.MinValue);
        return noiseValue * noiseSettings.Strength;
    }
}
