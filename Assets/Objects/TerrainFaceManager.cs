using UnityEngine;

public class TerrainFaceManager
{
    public TerrainFace[] TerrainFaces;
    public MeshFilter[] MeshFilters;
    public MeshCollider[] MeshColliders;
    public TerrainFaceManager(TerrainFace[] terrainFaces, MeshFilter[] meshFilters, MeshCollider[] meshColliders)
    {
        TerrainFaces = terrainFaces;
        MeshFilters = meshFilters;
        MeshColliders = meshColliders;
    }
}