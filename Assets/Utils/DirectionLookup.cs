using System.Collections.Generic;
using UnityEngine;

public class DirectionLookup : MonoBehaviour
{
    private readonly Dictionary<Vector3, string> directionMap;

    public DirectionLookup()
    {
        directionMap = new Dictionary<Vector3, string>
        {
            { Vector3.up, "Up" },
            { Vector3.down, "Down" },
            { Vector3.left, "Left" },
            { Vector3.right, "Right" },
            { Vector3.forward, "Forward" },
            { Vector3.back, "Back" }
        };
    }

    public string GetDirectionName(Vector3 direction)
    {
        if (directionMap.TryGetValue(direction, out string directionName))
        {
            return directionName;
        }
        else
        {
            return "Unknown direction";
        }
    }
}
