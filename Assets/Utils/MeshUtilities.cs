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
}