using UnityEngine;

public static class MeshGenerationHelper
{
    public static void GenerateMeshColors(MeshFilter[] meshFilters, ColorSettings colorSettings)
    {
        foreach (MeshFilter meshFilter in meshFilters)
        {
            ApplyMeshColor(meshFilter, colorSettings);
        }
    }

    private static void ApplyMeshColor(MeshFilter meshFilter, ColorSettings colorSettings)
    {
        if (meshFilter.TryGetComponent(out MeshRenderer renderer))
        {
            renderer.sharedMaterial.color = colorSettings.PlanetColor;
        }
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
        var mesh = filter.sharedMesh;
        var vertices = mesh.vertices;
        var centroid = PlanetPhysics.CalculateCentroid(vertices);

        for (int j = 0; j < vertices.Length; j++)
        {
            vertices[j] -= centroid;
        }

        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        filter.transform.localPosition = centroid;
    }

    public static void GenerateMesh(MeshGenerationParameters parameters)
    {
        if (parameters == null)
        {
            Debug.LogError("MeshGenerationParameters or the Planet object is null.");
            return;
        }

        foreach (var terrainFace in parameters.TerrainFaces)
        {
            terrainFace.ConstructMesh(parameters.ShapeSettings.Resolution, parameters.BaseTriangles);
        }

        parameters.Transform.localScale = Vector3.one * parameters.ShapeSettings.PlanetRadius;
    }
}
