using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField, HideInInspector] private MeshFilter[] _meshFilters;
    [SerializeField] private Material _planetMaterial;

    private MeshCollider[] _meshColliders;
    private TerrainFace[] _terrainFaces;
    private int[] _baseTriangles;
    private ShapeGenerator _shapeGenerator;

    public ShapeSettings ShapeSettings { get; set; }
    public ColorSettings ColorSettings { get; set; }
    public MeshFilter[] MeshFilters => _meshFilters;
    public MeshCollider[] MeshColliders => _meshColliders;
    public TerrainFace[] TerrainFaces => _terrainFaces;
    public int[] BaseTriangles => _baseTriangles;
    public bool AutoUpdate = true;

    [HideInInspector] public bool IsShapeSettingsFoldoutOpen = true;
    [HideInInspector] public bool IsColorSettingsFoldoutOpen = true;

    private void Start()
    {
        InitializePlanet();
        GenerateTerrain();
        MeshGenerationHelper.GenerateMesh(new MeshGenerationParameters(this));
        MeshGenerationHelper.GenerateMeshColors(MeshFilters, ColorSettings);
        MeshGenerationHelper.CenterMeshes(TerrainFaces, MeshFilters);
    }

    public void GeneratePlanet()
    {
        if (ShapeSettings == null || ColorSettings == null || _shapeGenerator == null)
        {
            ShapeSettings = ShapeSettings != null ? ShapeSettings : ScriptableObject.CreateInstance<ShapeSettings>();
            ColorSettings = ColorSettings != null ? ColorSettings : ScriptableObject.CreateInstance<ColorSettings>();
            _shapeGenerator = new ShapeGenerator(ShapeSettings);
        }
        UpdatePlanetShape();
        MeshGenerationHelper.GenerateMeshColors(_meshFilters, ColorSettings);
    }

    private void InitializePlanet()
    {
        ShapeSettings = ShapeSettings != null ? ShapeSettings : ScriptableObject.CreateInstance<ShapeSettings>();
        ColorSettings = ColorSettings != null ? ColorSettings : ScriptableObject.CreateInstance<ColorSettings>();
        _shapeGenerator = new ShapeGenerator(ShapeSettings);
        _planetMaterial = _planetMaterial != null ? _planetMaterial : new Material(Shader.Find("Standard"));
        _meshFilters ??= new MeshFilter[6];
        _meshColliders ??= new MeshCollider[6];
    }

    private void GenerateTerrain()
    {
        _meshFilters ??= new MeshFilter[6];
        _shapeGenerator = new ShapeGenerator(ShapeSettings);
        _planetMaterial = _planetMaterial != null ? _planetMaterial : new Material(Shader.Find("Standard"));

        var terrainFaceManager = TerrainFaceFactory.CreateFaces(_planetMaterial, transform, _meshFilters, _shapeGenerator);
        _terrainFaces = terrainFaceManager.TerrainFaces;
        _meshColliders = terrainFaceManager.MeshColliders;
    }

    public void UpdatePlanetShape()
    {
        if (AutoUpdate)
        {
            _baseTriangles = MeshUtilities.CalculateBaseTriangles(ShapeSettings.Resolution);

            if (_terrainFaces == null)
            {
                GenerateTerrain();
            }

            MeshGenerationHelper.GenerateMesh(new MeshGenerationParameters(this));
            MeshGenerationHelper.CenterMeshes(_terrainFaces, _meshFilters);
        }
    }

    public void UpdatePlanetColor()
    {
        if (AutoUpdate)
        {
            MeshGenerationHelper.GenerateMeshColors(_meshFilters, ColorSettings);
        }
    }
}
