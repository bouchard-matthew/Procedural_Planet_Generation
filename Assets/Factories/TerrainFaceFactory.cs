using System.Xml.Linq;
using UnityEngine;

public static class TerrainFaceFactory
{
    private static readonly Vector3[] Directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
    public static TerrainFaceManager CreateFaces(Material material, Transform parent, MeshFilter[] meshFilters, MeshCollider[] meshColliders, ShapeGenerator shapeGenerator)
    {
        var faces = new TerrainFace[Directions.Length];
        meshColliders = MeshUtilities.EnsureArrayInitialized(meshColliders, Directions.Length);
        meshFilters = MeshUtilities.EnsureArrayInitialized(meshFilters, Directions.Length);

        for (int i = 0; i < faces.Length; i++)
        {
            faces[i] = CreateOrRetrieveFace(material, parent, meshFilters, meshColliders, shapeGenerator, i);
        }

        return new TerrainFaceManager(faces, meshFilters, meshColliders);
    }

    private static TerrainFace CreateOrRetrieveFace(Material material, Transform parent, MeshFilter[] meshFilters, MeshCollider[] meshColliders, ShapeGenerator shapeGenerator, int index)
    {
        if (meshFilters[index] != null)
        {
            return new TerrainFace(meshFilters[index].sharedMesh, Directions[index], shapeGenerator);
        }

        var meshObj = MeshUtilities.CreateMeshGameObject($"{DirectionLookup.GetDirectionName(Directions[index])}", parent, material);
        meshFilters[index] = MeshUtilities.GetOrAddComponent<MeshFilter>(meshObj);
        meshColliders[index] = MeshUtilities.GetOrAddComponent<MeshCollider>(meshObj);
        meshColliders[index].sharedMesh = meshFilters[index].sharedMesh;

        if (meshColliders[index] == null)
        {
            meshColliders[index] = meshFilters[index].gameObject.AddComponent<MeshCollider>();
            meshColliders[index].sharedMesh = MeshUtilities.ReturnProperFilterMesh(meshFilters[index]);
        }

        return new TerrainFace(meshFilters[index].sharedMesh, Directions[index], shapeGenerator);
    }
}
