using UnityEngine;

public static class TerrainFaceFactory
{
    public class TerrainFaceManager
    {
        public TerrainFace[] TerrainFaces;
        public MeshCollider[] MeshColliders;
    }

    private static readonly Vector3[] Directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
    private static readonly DirectionLookup DirectionLookup = new();

    public static TerrainFaceManager CreateFaces(Material material, Transform parent, MeshFilter[] meshFilters)
    {
        var faces = new TerrainFace[6];
        var meshColliders = new MeshCollider[6];

        for (int i = 0; i < 6; i++)
        {
            faces[i] = CreateFace(material, parent, meshFilters, i);
            meshColliders[i] = AddMeshCollider(meshFilters[i]);
        }

        return new TerrainFaceManager { TerrainFaces = faces, MeshColliders = meshColliders };
    }

    private static TerrainFace CreateFace(Material material, Transform parent, MeshFilter[] meshFilters, int index)
    {
        if (meshFilters[index] != null)
        {
            return new TerrainFace(meshFilters[index].sharedMesh, Directions[index]);
        }

        var meshObj = new GameObject($"{DirectionLookup.GetDirectionName(Directions[index])}");
        meshObj.transform.SetParent(parent, false);
        meshFilters[index] = meshObj.AddComponent<MeshFilter>();
        meshFilters[index].sharedMesh = new Mesh();
        var renderer = meshObj.AddComponent<MeshRenderer>();
        renderer.sharedMaterial = material;

        return new TerrainFace(meshFilters[index].sharedMesh, Directions[index]);
    }

    private static MeshCollider AddMeshCollider(MeshFilter meshFilter)
    {
        return meshFilter.gameObject.AddComponent<MeshCollider>();
    }
}