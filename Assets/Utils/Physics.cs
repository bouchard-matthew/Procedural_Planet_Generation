using UnityEngine;

public static class Physics
{
    // Buffer to avoid precision issues
    private const float PrecisionBuffer = 0.01f;

    // Calculate the distance from the surface of a planet to the gravity body
    public static float CalculateDistanceFromSurface(Vector3 planetPosition, float planetRadius, Vector3 bodyPosition, Collider bodyCollider)
    {
        Vector3 direction = (bodyPosition - planetPosition).normalized;
        float distance = Vector3.Distance(planetPosition, bodyPosition);
        float bodyExtent = GetFaceExtent(bodyCollider, direction);

        // Include a small buffer to avoid precision issues
        float adjustedDistance = distance - planetRadius - bodyExtent - PrecisionBuffer;
        Debug.Log($"Distance: {distance}, Planet Radius: {planetRadius}, Body Extent: {bodyExtent}, Adjusted Distance: {adjustedDistance}");

        return adjustedDistance;
    }

    public static float GetFaceExtent(Collider collider, Vector3 direction)
    {
        if (collider is BoxCollider boxCollider)
        {
            Vector3 localDirection = boxCollider.transform.InverseTransformDirection(direction);
            Vector3 halfExtents = boxCollider.size / 2;
            float extent = Mathf.Abs(localDirection.x * halfExtents.x)
                         + Mathf.Abs(localDirection.y * halfExtents.y)
                         + Mathf.Abs(localDirection.z * halfExtents.z);

            Debug.Log($"LocalDirection: {localDirection}, Extent: {extent}");
            return extent;
        }
        else if (collider is SphereCollider sphereCollider)
        {
            return sphereCollider.radius;
        }

        return 0f;
    }

    // Calculate the gravitational force
    public static Vector3 CalculateGravitationalForce(Vector3 direction, float gravity, float bodyMass, float planetMass, float distance)
    {
        float forceMagnitude = gravity * bodyMass * planetMass / Mathf.Max(distance * distance, 0.0001f); // Avoid division by zero
        return direction.normalized * forceMagnitude;
    }
}
