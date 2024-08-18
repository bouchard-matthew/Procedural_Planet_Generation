using UnityEngine;

public class MeshGenerationParameters
{
    public MeshGenerationParameters(Planet planet)
    {
        Transform = planet.transform;
        PlanetRadius = planet.PlanetRadius;
        BaseTriangles = planet.BaseTriangles;
        TerrainFaces = planet.TerrainFaces;
        Resolution = planet.Resolution;
        MeshFilters = planet.MeshFilters;
    }

    public Transform Transform { get; set; }
    public int Resolution { get; set; }
    public int[] BaseTriangles { get; set; }
    public TerrainFace[] TerrainFaces { get; set; }
    public float PlanetRadius { get; set; }
    public MeshFilter[] MeshFilters { get; set; }
}