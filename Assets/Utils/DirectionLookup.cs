using System.Collections.Generic;
using UnityEngine;

public static class DirectionLookup
{
    public static string GetDirectionName(Vector3 direction)
    {
        var directionMap = new Dictionary<Vector3, string>
        {
            { Vector3.up, "Up" },
            { Vector3.down, "Down" },
            { Vector3.left, "Left" },
            { Vector3.right, "Right" },
            { Vector3.forward, "Forward" },
            { Vector3.back, "Back" }
        };

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
