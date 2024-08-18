using System.Linq;
using UnityEngine;

public static class PlanetGenerationHelper
{
    public static void GeneratePlanet(Planet planet)
    {
        GenerateMesh(new MeshGenerationParameters(planet));
        GenerateMeshColliders(planet.MeshFilters, planet.MeshColliders);
        CenterMeshes(planet.TerrainFaces, planet.MeshFilters);
    }

    public static void GenerateMeshColliders(MeshFilter[] meshFilters, MeshCollider[] meshColliders)
    {
        for (int i = 0; i< meshFilters.Length; i++)
        {
            meshColliders[i].sharedMesh = meshFilters[i].sharedMesh;
        }
    }

    public static int[] CalculateBaseTriangles(int resolution)
    {
        var baseTriangles = new int[(resolution - 1) * (resolution - 1) * 6];
        var triIndex = 0;

        for (int y = 0; y < resolution - 1; y++)
        {
            for (int x = 0; x < resolution - 1; x++)
            {
                int i = x + y * resolution;
                baseTriangles[triIndex++] = i;
                baseTriangles[triIndex++] = i + resolution + 1;
                baseTriangles[triIndex++] = i + resolution;
                baseTriangles[triIndex++] = i;
                baseTriangles[triIndex++] = i + 1;
                baseTriangles[triIndex++] = i + resolution + 1;
            }
        }

        return baseTriangles;
    }

    public static void CenterMeshes(TerrainFace[] terrainFaces, MeshFilter[] meshFilters)
    {
        for (int i = 0; i < terrainFaces.Length; i++)
        {
            CenterMesh(meshFilters[i]);
        }
    }

    private static void CenterMesh(MeshFilter filter)
    {
        Mesh mesh = filter.sharedMesh;
        Vector3[] vertices = mesh.vertices;
        Vector3 centroid = CalculateCentroid(vertices);

        for (int j = 0; j < vertices.Length; j++)
        {
            vertices[j] -= centroid;
        }

        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        filter.transform.localPosition = centroid;
    }

    private static Vector3 CalculateCentroid(Vector3[] vertices)
    {
        return vertices.Aggregate(Vector3.zero, (current, vertex) => current + vertex) / vertices.Length;
    }

    public static void GenerateMesh(MeshGenerationParameters parameters)
    {
        foreach (TerrainFace terrainFace in parameters.TerrainFaces)
        {
            terrainFace.ConstructMesh(parameters.Resolution, parameters.BaseTriangles);
        }
        parameters.Transform.localScale = Vector3.one * parameters.PlanetRadius;
    }
}