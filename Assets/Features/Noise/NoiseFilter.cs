using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseFilter
{
    SimplexNoise Noise = new SimplexNoise();

    public float Evaluate(Vector3 point)
    {
        return (Noise.Evaluate(point)+1)*0.5f;
    }
}