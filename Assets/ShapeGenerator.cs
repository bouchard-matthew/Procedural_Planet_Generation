using UnityEngine;

public class ShapeGenerator
{
    ShapeSettings ShapeSettings;
    public ShapeGenerator(ShapeSettings shapeSettings)
    {
        ShapeSettings = shapeSettings;
    }

    public Vector3 CalculatePointOnUnitPlanetShape(Vector3 pointOnUnitPlanetShape)
    {
        return pointOnUnitPlanetShape * ShapeSettings.PlanetRadius;
    }
}