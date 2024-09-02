using System.Linq;
using UnityEngine;

public class MeshUpdater : MonoBehaviour
{
    private Planet _planet;

    public void Initialize(Planet planet)
    {
        _planet = planet;
    }

    private bool AreArraysInitialized()
    {
        return _planet.MeshFilters?.All(filter => filter != null) == true &&
               _planet.TerrainFaces?.All(face => face != null) == true &&
               _planet.MeshColliders?.All(collider => collider != null) == true;
    }

    public void UpdateShape()
    {
        if (!_planet.AutoUpdate) return;

        _planet.BaseTriangles = MeshUtilities.CalculateBaseTriangles(_planet.ShapeSettings.Resolution);

        if (!AreArraysInitialized())
        {
            var terrainFaceManager = TerrainFaceFactory.CreateFaces(
               _planet.PlanetMaterial,
               _planet.transform,
               _planet.MeshFilters,
               _planet.MeshColliders,
               _planet.ShapeGenerator
            );

            _planet.TerrainFaces = terrainFaceManager.TerrainFaces;
            _planet.MeshColliders = terrainFaceManager.MeshColliders;
        }

        UpdateMeshFilters();
        _planet.ShapeGenerator.UpdateSettings(_planet.ShapeSettings);
        MeshGenerationHelper.GenerateMesh(new MeshGenerationParameters(_planet));
        MeshGenerationHelper.CenterMeshes(_planet.TerrainFaces, _planet.MeshFilters);
    }

    public void UpdateColor()
    {
        if (!_planet.AutoUpdate) return;

        MeshGenerationHelper.GenerateMeshColors(_planet.MeshFilters, _planet.ColorSettings);
    }

    private void UpdateMeshFilters()
    {
        foreach (var meshFilter in _planet.MeshFilters)
        {
            var mesh = MeshUtilities.ReturnProperFilterMesh(meshFilter);
            meshFilter.sharedMesh = mesh;

            if (meshFilter.TryGetComponent<MeshCollider>(out var meshCollider))
            {
                meshCollider.sharedMesh = mesh;
            }
        }
    }
}
