using UnityEngine;

public class PlanetInitializer
{
    private readonly Planet _planet;

    public PlanetInitializer(Planet planet)
    {
        _planet = planet;
    }

    public void Initialize()
    {
        InitializeSettings();
        InitializeArrays();
        InitializeShapeGenerator();
        InitializeMaterial();
    }

    private void InitializeSettings()
    {
        _planet.ShapeSettings = _planet.ShapeSettings != null ? _planet.ShapeSettings : AssetUtility.CreateOrFetchAsset<ShapeSettings>("Shape.asset");
        _planet.ColorSettings = _planet.ColorSettings != null ? _planet.ColorSettings : AssetUtility.CreateOrFetchAsset<ColorSettings>("Color.asset");
    }

    private void InitializeArrays()
    {
        _planet.MeshFilters = MeshUtilities.EnsureArrayInitialized(_planet.MeshFilters, 6);
        _planet.MeshColliders = MeshUtilities.EnsureArrayInitialized(_planet.MeshColliders, 6);
        _planet.BaseTriangles = MeshUtilities.CalculateBaseTriangles(_planet.ShapeSettings.Resolution);
    }

    private void InitializeShapeGenerator()
    {
        _planet.ShapeGenerator = new ShapeGenerator(_planet.ShapeSettings);
    }

    private void InitializeMaterial()
    {
        if (_planet.PlanetMaterial == null)
        {
            _planet.PlanetMaterial = new Material(Shader.Find("Standard"));
        }
    }
}
