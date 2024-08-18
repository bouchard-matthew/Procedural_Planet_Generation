using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField, HideInInspector] public MeshFilter[] MeshFilters;
    [SerializeField, Range(2, 256)] private int resolution = 30;
    [SerializeField] private float planetRadius = 5;
    [SerializeField] private Material planetMaterial;
    public TerrainFace[] TerrainFaces { get; private set; }
    public int[] BaseTriangles { get; private set; }
    public MeshCollider[] MeshColliders { get; private set; }
    public int Resolution
    {
        get => resolution;
        set
        {
            resolution = Mathf.Clamp(value, 2, 256);
            RegeneratePlanet();
        }
    }

    public float PlanetRadius
    {
        get => planetRadius;
        set
        {
            planetRadius = value;
            RegeneratePlanet();
        }
    }

    private void Start()
    {
        Initialize();
        RegeneratePlanet();
    }

    private void Initialize()
    {
        planetMaterial ??= new Material(Shader.Find("Standard"));
        MeshFilters ??= MeshFilters ?? new MeshFilter[6];
        MeshColliders ??= MeshColliders ?? new MeshCollider[6];

        var terrainFaceProperties = TerrainFaceFactory.CreateFaces(planetMaterial, transform, MeshFilters);
        TerrainFaces = terrainFaceProperties.TerrainFaces;
        MeshColliders = terrainFaceProperties.MeshColliders;
    }

    private void RegeneratePlanet()
    {
        BaseTriangles = PlanetGenerationHelper.CalculateBaseTriangles(Resolution);
        PlanetGenerationHelper.GeneratePlanet(this);
    }

    private void OnValidate()
    {
        Initialize();
        RegeneratePlanet();
    }
}