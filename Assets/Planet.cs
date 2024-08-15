using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField][Range(2, 256)] int resolution = 14;
    [SerializeField, HideInInspector] MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;
    int numOfFaces = 6;
    [SerializeField] private Material planetMaterial;

    [SerializeField] private float planetRadius = 10f;

    private bool isInitialized = false;

    private void OnValidate()
    {
        if (!isInitialized || meshFilters == null || terrainFaces == null)
        {
            Initialize();
        }

        if (meshFilters != null && terrainFaces != null)
        {
            GenerateMesh();
            CenterMeshes();
        }
    }

    private void Start()
    {
        if (!isInitialized)
        {
            Initialize();
            GenerateMesh();
            CenterMeshes();
        }
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

        isInitialized = true;
    }

    void GenerateMesh()
    {
        foreach (TerrainFace terrainFace in terrainFaces)
        {
            terrainFace.ConstructMesh();
        }

        // Scale the planet to match the desired radius
        transform.localScale = Vector3.one * planetRadius;
    }

    void CenterMeshes()
    {
        // Iterate over each terrain face
        for (int i = 0; i < terrainFaces.Length; i++)
        {
            Mesh mesh = meshFilters[i].sharedMesh;
            Vector3[] vertices = mesh.vertices;

            // Calculate the centroid of the mesh
            Vector3 centroid = Vector3.zero;
            foreach (Vector3 vertex in vertices)
            {
                centroid += vertex;
            }
            centroid /= vertices.Length;

            // Offset the vertices so that the centroid is at the origin
            for (int j = 0; j < vertices.Length; j++)
            {
                vertices[j] -= centroid;
            }

            // Apply the new vertices to the mesh
            mesh.vertices = vertices;
            mesh.RecalculateBounds();

            // Adjust the mesh object position to maintain alignment with the planet's transform
            meshFilters[i].transform.localPosition = centroid;
        }
    }

    public float GetPlanetRadius()
    {
        return planetRadius;
    }

}
