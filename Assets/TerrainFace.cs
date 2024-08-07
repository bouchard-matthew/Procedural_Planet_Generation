using System;
using UnityEngine;

public class TerrainFace
{
    Mesh mesh;  // The mesh object this face will modify
    int resolution;  // Determines the detail level of the face

    // Vectors defining the orientation of the face
    Vector3 localUp;  // Points from the center of the planet outward
    Vector3 tangentAxis;  // Perpendicular to localUp
    Vector3 biTangentAxis;  // Perpendicular to both localUp and the tangentAxis

    // Constructor to initialize the TerrainFace
    public TerrainFace(Mesh mesh, int resolution, Vector3 localUp)
    {
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;
        // Create tangentAxis by rotating localUp
        tangentAxis = new Vector3(localUp.y, localUp.z, localUp.x);
        // Create axisB perpendicular to both localUp and axisA
        biTangentAxis = Vector3.Cross(localUp, tangentAxis);
    }

    public void ConstructMesh()
    {
        // Create arrays to hold vertex positions and triangle indices
        Vector3[] vertices = new Vector3[resolution * resolution];
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        int triIndex = 0;  // Index for filling the triangles array

        // Loop through a grid defined by the resolution
        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;  // Calculate the current vertex index

                // Calculate the position of this vertex as a percentage of the face
                Vector2 percent = new Vector2(x, y) / (resolution - 1);

                // Calculate the position of the point on a unit cube
                Vector3 pointOnUnitPlanetShape = localUp
                    + (percent.x - 0.5f) * 2 * tangentAxis
                    + (percent.y - 0.5f) * 2 * biTangentAxis;

                // Project the point onto a unit sphere
                vertices[i] = pointOnUnitPlanetShape.normalized;

                // Create triangles, skipping the last row and column
                if (x != resolution - 1 && y != resolution - 1)
                {
                    // Define two triangles to form a quad
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + resolution + 1;
                    triangles[triIndex + 2] = i + resolution;

                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + resolution + 1;
                    triIndex += 6;  // Move to the next set of triangle indices
                }
            }
        }

        // Clear the existing mesh data
        mesh.Clear();
        // Assign the new vertices and triangles to the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        // Recalculate the normals for proper lighting
        mesh.RecalculateNormals();
    }
}