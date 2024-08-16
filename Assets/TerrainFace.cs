using UnityEngine;

public class TerrainFace
{
    private Mesh _mesh;
    private Vector3 _localUp;
    Vector3 tangentAxis;
    Vector3 biTangentAxis;

    public TerrainFace(Mesh mesh, Vector3 localUp)
    {
        _mesh = mesh;
        _localUp = localUp;
        tangentAxis = new Vector3(_localUp.y, _localUp.z, _localUp.x);
        biTangentAxis = Vector3.Cross(_localUp, tangentAxis);
    }

    public void ConstructMesh(int resolution, int[] baseTriangles)
    {
        Vector3[] vertices = new Vector3[resolution * resolution];

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnitPlanetShape = _localUp
                    + (percent.x - 0.5f) * 2 * tangentAxis
                    + (percent.y - 0.5f) * 2 * biTangentAxis;
                vertices[i] = pointOnUnitPlanetShape.normalized;
            }
        }

        _mesh.Clear();
        _mesh.vertices = vertices;
        _mesh.triangles = baseTriangles;
        _mesh.RecalculateNormals();
    }
}