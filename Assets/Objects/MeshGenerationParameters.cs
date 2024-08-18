using UnityEngine;

public class MeshGenerationParameters
{
    public MeshGenerationParameters(Planet planet)
    {
        Transform = planet.transform;
        ShapeSettings = planet.ShapeSettings;
        BaseTriangles = planet.BaseTriangles;
        TerrainFaces = planet.TerrainFaces;
        MeshFilters = planet.MeshFilters;
    }

    public Transform Transform { get; set; }
    public int[] BaseTriangles { get; set; }
    public TerrainFace[] TerrainFaces { get; set; }
    public MeshFilter[] MeshFilters { get; set; }
    public ShapeSettings ShapeSettings { get; set; }
}