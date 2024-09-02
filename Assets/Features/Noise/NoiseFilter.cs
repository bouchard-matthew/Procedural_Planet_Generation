using UnityEngine;

public class NoiseFilter
{
    Noise Noise = new();
    NoiseSettings NoiseSettings;
    public NoiseFilter(NoiseSettings noiseSettings)
    {
        NoiseSettings = noiseSettings;
    }

    public float CalculateElevation(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = NoiseSettings.BaseRoughness;
        float amplitude = 1;

        for (int i = 0; i < NoiseSettings.NumberOfLayers; i++) {
            float v = Noise.Evaluate(point * frequency + NoiseSettings.Center);
            noiseValue += (v + 1) * .5f * amplitude;
            frequency *= NoiseSettings.Roughness;
            amplitude *= NoiseSettings.Persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - NoiseSettings.MinValue);
        return noiseValue * NoiseSettings.Strength;
    }
}