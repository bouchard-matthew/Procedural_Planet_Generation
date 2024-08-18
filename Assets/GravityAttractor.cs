using System.Collections.Generic;
using UnityEngine;

public class GravityAttractor : MonoBehaviour
{
    [SerializeField] public float planetMass = 1000f;
    [SerializeField] public float gravity = 9.8f;
    [SerializeField] public float gravitationalReach = 100f;
    [SerializeField] public float surfaceOffset = 0.1f;

    private readonly List<GravityBody> affectedBodies = new();
    private Planet planet;
    private MeshCollider[] meshColliders;

    private void Start()
    {
        if (!TryGetComponent<Planet>(out planet))
        {
            Debug.LogError("Planet script not found on the GravityAttractor GameObject.");
            enabled = false;
            return;
        }

        meshColliders = planet.MeshColliders;
        if (meshColliders == null || meshColliders.Length == 0)
        {
            Debug.LogError("No MeshColliders found on the Planet.");
            enabled = false;
            return;
        }
    }

    private void FixedUpdate()
    {
        foreach (var body in affectedBodies)
        {
            AttractBody(body);
        }
    }

    private void AttractBody(GravityBody body)
    {
        Rigidbody rbody = body.Rigidbody;
        Vector3 direction = transform.position - rbody.position;
        float distance = direction.magnitude;

        if (distance <= gravitationalReach)
        {
            if (IsInsideAnyCollider(rbody.position))
            {
                ClampToSurface(rbody);
            }
            else
            {
                ApplyGravitationalForce(rbody, direction, distance);
                RotateBodyTowardsPlanet(body, direction);
            }

            DrawDebugLines(rbody, direction);
        }
    }

    private bool IsInsideAnyCollider(Vector3 position)
    {
        foreach (var collider in meshColliders)
        {
            if (collider.bounds.Contains(position))
            {
                return true;
            }
        }
        return false;
    }

    private void ClampToSurface(Rigidbody rbody)
    {
        Vector3 direction = (rbody.position - transform.position).normalized;
        float closestDistance = float.MaxValue;
        Vector3 closestPoint = Vector3.zero;

        foreach (var collider in meshColliders)
        {
            Vector3 point = collider.ClosestPoint(rbody.position);
            float distance = Vector3.Distance(point, rbody.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = point;
            }
        }

        Vector3 surfacePoint = closestPoint + direction * surfaceOffset;
        rbody.MovePosition(surfacePoint);
        rbody.velocity = Vector3.zero;
    }

    private void ApplyGravitationalForce(Rigidbody rbody, Vector3 direction, float distance)
    {
        Vector3 force = PlanetPhysics.CalculateGravitationalForce(direction, gravity, rbody.mass, planetMass, distance);
        rbody.AddForce(force);
    }

    private void RotateBodyTowardsPlanet(GravityBody body, Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.FromToRotation(body.transform.up, -direction) * body.transform.rotation;
        body.transform.rotation = Quaternion.Slerp(body.transform.rotation, targetRotation, Time.fixedDeltaTime * 3.5f);
    }

    private void DrawDebugLines(Rigidbody rbody, Vector3 direction)
    {
        Debug.DrawLine(rbody.position, rbody.position + direction.normalized * 5f, Color.red);
        Debug.DrawLine(rbody.position, transform.position, Color.yellow);
    }

    public void AddAffectedBody(GravityBody body)
    {
        if (!affectedBodies.Contains(body))
        {
            affectedBodies.Add(body);
        }
    }

    public void RemoveAffectedBody(GravityBody body)
    {
        affectedBodies.Remove(body);
    }
}