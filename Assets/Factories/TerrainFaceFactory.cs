using System.Linq;
using UnityEngine;

public static class TerrainFaceFactory
{
    public class TerrainFaceManager
    {
        public TerrainFace[] TerrainFaces;
        public MeshCollider[] MeshColliders;
    }

    private static readonly Vector3[] Directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
    public static TerrainFaceManager CreateFaces(Material material, Transform parent, MeshFilter[] meshFilters, ShapeGenerator shapeGenerator)
    {
        var faces = new TerrainFace[6];
        var meshColliders = new MeshCollider[6];
        meshFilters = meshFilters == null || meshFilters.Length == 0 ? new MeshFilter[6] : meshFilters;

        for (int i = 0; i < 6; i++)
        {
            faces[i] = CreateOrRetrieveFace(material, parent, meshFilters, meshColliders, shapeGenerator, i);
        }

        return new TerrainFaceManager { TerrainFaces = faces, MeshColliders = meshColliders };
    }

    private static TerrainFace CreateOrRetrieveFace(Material material, Transform parent, MeshFilter[] meshFilters, MeshCollider[] meshColliders, ShapeGenerator shapeGenerator, int index)
    {
        if (meshFilters[index] != null)
        {
            return new TerrainFace(meshFilters[index].sharedMesh, Directions[index], shapeGenerator);
        }

        var meshObj = CreateMeshGameObject($"{DirectionLookup.GetDirectionName(Directions[index])}", parent, material);
        meshFilters[index] = meshObj.GetComponent<MeshFilter>();
        meshColliders[index] = meshObj.GetComponent<MeshCollider>();

        return new TerrainFace(meshFilters[index].sharedMesh, Directions[index], shapeGenerator);
    }

    private static GameObject CreateMeshGameObject(string name, Transform parent, Material material)
    {
        var meshObj = new GameObject(name);
        meshObj.transform.SetParent(parent, false);
        var meshFilter = meshObj.AddComponent<MeshFilter>();
        meshFilter.sharedMesh = new Mesh();
        var renderer = meshObj.AddComponent<MeshRenderer>();
        renderer.sharedMaterial = material;
        meshObj.AddComponent<MeshCollider>().sharedMesh = meshFilter.sharedMesh;

        return meshObj;
    }
}