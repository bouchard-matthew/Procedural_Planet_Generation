using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField, HideInInspector] private MeshFilter[] _meshFilters;
    [SerializeField] private Material _planetMaterial;

    private MeshCollider[] _meshColliders;
    private TerrainFace[] _terrainFaces;
    private int[] _baseTriangles;
    private ShapeGenerator _shapeGenerator;

    [SerializeField] public ShapeSettings ShapeSettings;
    [SerializeField] public ColorSettings ColorSettings;
    public MeshFilter[] MeshFilters => _meshFilters;
    public MeshCollider[] MeshColliders => _meshColliders;
    public TerrainFace[] TerrainFaces => _terrainFaces;
    public int[] BaseTriangles => _baseTriangles;

    public bool AutoUpdate = true;

    [HideInInspector] public bool hasBeenInitialized = false;

    [HideInInspector] public bool IsShapeSettingsFoldoutOpen = true;
    [HideInInspector] public bool IsColorSettingsFoldoutOpen = true;

    private void Start()
    {
        InitializePlanet();
        GenerateTerrain();
        UpdatePlanetShape();
        UpdatePlanetColor();
    }

    public void GeneratePlanet()
    {
        InitializePlanet();
        UpdatePlanetShape();
        UpdatePlanetColor();
    }

    private void InitializePlanet()
    {
        InitializePlanetShapeColorSettings();

        _meshFilters = _meshFilters == null || _meshFilters.Length == 0 ? new MeshFilter[6] : _meshFilters;
        _meshColliders ??= new MeshCollider[6];
        _baseTriangles = MeshUtilities.CalculateBaseTriangles(ShapeSettings.Resolution);
    }

    private void InitializePlanetShapeColorSettings()
    {
        ShapeSettings = AssetUtility.CreateOrFetchAsset<ShapeSettings>("Shape.asset");
        ColorSettings = AssetUtility.CreateOrFetchAsset<ColorSettings>("Color.asset");
        _planetMaterial = _planetMaterial != null ? _planetMaterial : new Material(Shader.Find("Standard"));
        _shapeGenerator ??= new ShapeGenerator(ShapeSettings);
    }

    private void GenerateTerrain()
    {
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
            _meshFilters = _meshFilters == null || _meshFilters.Length == 0 ? new MeshFilter[6] : _meshFilters;

            if (_terrainFaces == null)
            {
                GenerateTerrain();
            }

            for (int i = 0; i < _meshFilters.Length; i++)
            {
                var mesh = Application.isPlaying ? _meshFilters[i].mesh : _meshFilters[i].sharedMesh;
                mesh ??= new Mesh();

                if (Application.isPlaying)
                {
                    _meshFilters[i].mesh = mesh;
                    return;
                }

                _meshFilters[i].sharedMesh = mesh;
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
