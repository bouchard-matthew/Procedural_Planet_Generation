using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField][Range(2, 256)] int resolution = 30;
    [SerializeField, HideInInspector] MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;
    int numOfFaces = 6;
    [SerializeField] private Material planetMaterial;
    [SerializeField] private float planetRadius = 10f;
    private bool isInitialized = false;
    private int[] baseTriangles;
    private int previousResolution;

    private void OnValidate()
    {
        if (!isInitialized || meshFilters == null || terrainFaces == null)
        {
            Initialize();
        }

        if (meshFilters != null && terrainFaces != null)
        {
            if (resolution != previousResolution)
            {
                Debug.Log("Resolution changed from " + previousResolution + " to " + resolution);
                CalculateBaseTriangles();
                previousResolution = resolution;
            }

            GenerateMesh();
            CenterMeshes();
        }
    }


    private void Start()
    {
        previousResolution = resolution;
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

        CalculateBaseTriangles();

        for (int i = 0; i < numOfFaces; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new("mesh");
                meshObj.transform.parent = transform;
                meshObj.AddComponent<MeshRenderer>().sharedMaterial = planetMaterial;
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }
            terrainFaces[i] = new TerrainFace(meshFilters[i].sharedMesh, directions[i]);
        }
        isInitialized = true;
    }

    void CalculateBaseTriangles()
    {
        baseTriangles = new int[(resolution - 1) * (resolution - 1) * 6];
        int triIndex = 0;
        for (int y = 0; y < resolution - 1; y++)
        {
            for (int x = 0; x < resolution - 1; x++)
            {
                int i = x + y * resolution;
                baseTriangles[triIndex] = i;
                baseTriangles[triIndex + 1] = i + resolution + 1;
                baseTriangles[triIndex + 2] = i + resolution;
                baseTriangles[triIndex + 3] = i;
                baseTriangles[triIndex + 4] = i + 1;
                baseTriangles[triIndex + 5] = i + resolution + 1;
                triIndex += 6;
            }
        }
    }

    void GenerateMesh()
    {
        foreach (TerrainFace terrainFace in terrainFaces)
        {
            terrainFace.ConstructMesh(resolution, baseTriangles);
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
