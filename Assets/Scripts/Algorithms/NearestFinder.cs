using System.Collections.Generic;
using UnityEngine;

public static class NearestFinder
{
    public static int FindNearest(Vector2 origin, IList<Vector2> points)
    {
        if (points.Count == 0)
        {
            return -1;
        }

        int nearest = -1;
        float bestDistance = 0f;

        for (int i = 0; i < points.Count; i++)
        {
            // Squared distance is enough to compare, and skips the square root.
            float distance = (points[i] - origin).sqrMagnitude;
            if (nearest == -1 || distance < bestDistance)
            {
                nearest = i;
                bestDistance = distance;
            }
        }

        return nearest;
    }
}
