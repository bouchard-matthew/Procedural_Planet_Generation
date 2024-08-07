using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField]
    [Range(2, 256)]
    int resolution = 14;

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;
    int numOfFaces = 6;

    [SerializeField]
    private Material planetMaterial;

    private void OnValidate()
    {
        Initialize();
        GenerateMesh();
    }

    void Initialize()
    {
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[numOfFaces];
        }
        terrainFaces = new TerrainFace[numOfFaces];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < numOfFaces; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new("mesh");
                meshObj.transform.parent = transform;
                meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            terrainFaces[i] = new TerrainFace(meshFilters[i].sharedMesh, resolution, directions[i]);
        }
    } 

    void GenerateMesh()
    {
        foreach(TerrainFace terrainFace in terrainFaces)
        {
            terrainFace.ConstructMesh();
        }
    }
}
