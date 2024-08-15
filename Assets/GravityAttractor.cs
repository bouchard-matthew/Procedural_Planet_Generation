using UnityEngine;
using System.Collections.Generic;
public class GravityAttractor : MonoBehaviour
{
    public float gravity = 9.8f;
    public float gravitationalReach = 100f;
    public float stopDistance = 0.1f; // Distance from the surface where attraction stops
    [SerializeField]
    public float planetMass = 1000f;

    private List<GravityBody> affectedBodies = new List<GravityBody>();
    private Planet planetScript; // Reference to the Planet script

    private void Start()
    {
        // Get the Planet script component from the same GameObject
        planetScript = GetComponent<Planet>();
        if (planetScript == null)
        {
            Debug.LogError("Planet script not found on the same GameObject.");
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
        Rigidbody rbody = body.GetComponent<Rigidbody>();
        Vector3 direction = (transform.position - rbody.position);
        float distance = direction.magnitude;

        // Use the planetRadius from the Planet script
        float planetRadius = planetScript != null ? planetScript.GetPlanetRadius() : 6f; // Default to 6f if Planet script is not found

        // Calculate the distance from the planet's surface
        float distanceFromSurfaceWithBody = Physics.CalculateDistanceFromSurface(transform.position, planetRadius, rbody.position, body.GetComponent<Collider>());

        if (distanceFromSurfaceWithBody > stopDistance && distance <= gravitationalReach)
        {
            Vector3 force = Physics.CalculateGravitationalForce(direction, gravity, rbody.mass, planetMass, distance);
            rbody.AddForce(force);

            // Align the body with the planet's surface
            Quaternion targetRotation = Quaternion.FromToRotation(body.transform.up, -direction) * body.transform.rotation;
            body.transform.rotation = Quaternion.Slerp(body.transform.rotation, targetRotation, Time.fixedDeltaTime * 3.5f);

            // Debug visualization
            Debug.DrawLine(rbody.position, rbody.position + force.normalized * 5f, Color.red);
            Debug.DrawLine(rbody.position, transform.position, Color.yellow);
        }
        else if (distanceFromSurfaceWithBody <= stopDistance)
        {
            rbody.velocity = Vector3.zero;
            rbody.angularVelocity = Vector3.zero;
        }
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
