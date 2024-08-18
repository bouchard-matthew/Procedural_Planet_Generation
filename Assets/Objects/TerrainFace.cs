using UnityEngine;

public class TerrainFace
{
    private readonly Mesh _mesh;
    private readonly Vector3 _localUp;
    private readonly Vector3 _tangentAxis;
    private readonly Vector3 _biTangentAxis;
    private readonly ShapeGenerator _shapeGenerator;

    public TerrainFace(Mesh mesh, Vector3 localUp, ShapeGenerator shapeGenerator)
    {
        _mesh = mesh;
        _localUp = localUp;
        _tangentAxis = new Vector3(localUp.y, localUp.z, localUp.x);
        _biTangentAxis = Vector3.Cross(localUp, _tangentAxis);
        _shapeGenerator = shapeGenerator;
    }

    public void ConstructMesh(int resolution, int[] baseTriangles)
    {
        var vertices = MeshUtilities.GenerateVertices(resolution, _localUp, _tangentAxis, _biTangentAxis, _shapeGenerator);
        MeshUtilities.ApplyMeshData(_mesh, vertices, baseTriangles);
    }
}