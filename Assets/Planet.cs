using UnityEngine;

[ExecuteInEditMode]
public class Planet : MonoBehaviour
{
    private PlanetInitializer _planetInitializer;

    [HideInInspector] public bool isShapeSettingsFoldoutOpen = true;
    [HideInInspector] public bool isColorSettingsFoldoutOpen = true;
    public MeshUpdater MeshUpdater { get; private set; }
    public bool HasBeenInitialized { get; private set; }
    public bool AutoUpdate { get; set; } = true;


    [SerializeField, HideInInspector] public MeshFilter[] MeshFilters;
    public MeshCollider[] MeshColliders;
    public TerrainFace[] TerrainFaces;
    public int[] BaseTriangles;


    public ShapeGenerator ShapeGenerator;
    [SerializeField] public Material PlanetMaterial;
    [SerializeField] public ShapeSettings ShapeSettings;
    [SerializeField] public ColorSettings ColorSettings;

    void Awake()
    {
        InitializePlanet();
    }

    void Start()
    {
        if (!HasBeenInitialized)
        {
            GeneratePlanet();
        }
    }

    private void InitializePlanet()
    {
        _planetInitializer ??= new PlanetInitializer(this);
        _planetInitializer.Initialize();
        MeshUpdater = MeshUpdater != null ? MeshUpdater : this.gameObject.AddComponent<MeshUpdater>();
        MeshUpdater.Initialize(this);
    }

    public void GeneratePlanet()
    {
        _planetInitializer.Initialize();
        MeshUpdater.UpdateShape();
        MeshUpdater.UpdateColor();
        HasBeenInitialized = true;
    }
}
