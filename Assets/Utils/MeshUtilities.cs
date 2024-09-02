using System;
using UnityEngine;

public static class MeshUtilities
{
    public static Vector3[] GenerateVertices(int resolution, Vector3 localUp, Vector3 tangentAxis, Vector3 biTangentAxis, ShapeGenerator shapeGenerator)
    {
        var vertices = new Vector3[resolution * resolution];

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;
                var percent = new Vector2(x, y) / (resolution - 1);
                var pointOnUnitPlanetShape = localUp + PlanetPhysics.ReturnUnitRangedValue(percent.x) * tangentAxis + PlanetPhysics.ReturnUnitRangedValue(percent.y) * biTangentAxis;
                vertices[i] = shapeGenerator.CalculatePointOnUnitPlanetShape(pointOnUnitPlanetShape.normalized);
            }
        }

        return vertices;
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

    public static void ApplyMeshData(Mesh mesh, Vector3[] vertices, int[] triangles)
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    public static T GetOrAddComponent<T>(GameObject meshObj) where T : Component
    {
        return meshObj.TryGetComponent(out T component) ? component : meshObj.AddComponent<T>();
    }

    public static T[] EnsureArrayInitialized<T>(T[] array, int length) where T : Component
    {
        return array ?? new T[length];
    }

    public static Mesh ReturnProperFilterMesh(MeshFilter meshFilter)
    {
        return meshFilter.sharedMesh != null ? meshFilter.sharedMesh : new Mesh();
    }

    public static GameObject CreateMeshGameObject(string name, Transform parent, Material material)
    {
        var meshObj = new GameObject(name);
        meshObj.transform.SetParent(parent, false);
        var meshFilter = meshObj.AddComponent<MeshFilter>();
        meshFilter.sharedMesh = new Mesh();
        var meshRenderer = meshObj.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = material;

        return meshObj;
    }
}
