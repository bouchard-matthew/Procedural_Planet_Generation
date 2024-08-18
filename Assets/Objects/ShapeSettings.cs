using UnityEngine;

[CreateAssetMenu()]
public class ShapeSettings : ScriptableObject
{
    public float PlanetRadius = 1f;

    [Range(2, 256)]
    public int Resolution = 15;
}