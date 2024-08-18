using System.Linq;
using UnityEngine;

public static class PlanetPhysics
{
    private const float PrecisionBuffer = 0.01f;

    public static float ReturnUnitRangedValue(float value) => (value - 0.5f) * 2;

    public static float CalculateDistanceFromSurface(Vector3 planetPosition, float planetRadius, Vector3 bodyPosition, Collider bodyCollider)
    {
        Vector3 direction = (bodyPosition - planetPosition).normalized;
        float distance = Vector3.Distance(planetPosition, bodyPosition);
        float bodyExtent = GetFaceExtent(bodyCollider, direction);
        return distance - planetRadius - bodyExtent - PrecisionBuffer;
    }

    public static float GetFaceExtent(Collider collider, Vector3 direction)
    {
        return collider switch
        {
            BoxCollider boxCollider => GetBoxColliderExtent(boxCollider, direction),
            SphereCollider sphereCollider => sphereCollider.radius,
            _ => 0f
        };
    }

    public static Vector3 CalculateCentroid(Vector3[] vertices)
    {
        return vertices.Aggregate(Vector3.zero, (current, vertex) => current + vertex) / vertices.Length;
    }

    private static float GetBoxColliderExtent(BoxCollider boxCollider, Vector3 direction)
    {
        Vector3 localDirection = boxCollider.transform.InverseTransformDirection(direction);
        Vector3 halfExtents = boxCollider.size / 2;
        return Mathf.Abs(Vector3.Dot(localDirection, halfExtents));
    }

    public static Vector3 CalculateGravitationalForce(Vector3 direction, float gravity, float bodyMass, float planetMass, float distance)
    {
        float forceMagnitude = (gravity * bodyMass * planetMass) / Mathf.Max(distance * distance, 0.0001f);
        return direction.normalized * forceMagnitude;
    }
}