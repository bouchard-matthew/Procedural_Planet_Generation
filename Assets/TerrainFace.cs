using UnityEngine;

public class TerrainFace
{
    private readonly Mesh _mesh;
    private readonly Vector3 _localUp;
    private readonly Vector3 _tangentAxis;
    private readonly Vector3 _biTangentAxis;

    public TerrainFace(Mesh mesh, Vector3 localUp)
    {
        _mesh = mesh;
        _localUp = localUp;
        _tangentAxis = new Vector3(localUp.y, localUp.z, localUp.x);
        _biTangentAxis = Vector3.Cross(localUp, _tangentAxis);
    }

    public void ConstructMesh(int resolution, int[] baseTriangles)
    {
        var vertices = new Vector3[resolution * resolution];

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                var i = x + y * resolution;
                var percent = new Vector2(x, y) / (resolution - 1);
                var pointOnUnitPlanetShape = _localUp + PlanetPhysics.ReturnUnitRangedValue(percent.x) * _tangentAxis + PlanetPhysics.ReturnUnitRangedValue(percent.y) * _biTangentAxis;

                vertices[i] = pointOnUnitPlanetShape.normalized;
            }
        }

        _mesh.Clear();
        _mesh.vertices = vertices;
        _mesh.triangles = baseTriangles;
        _mesh.RecalculateNormals();
    }
}