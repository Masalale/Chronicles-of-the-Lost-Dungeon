using System.Collections.Generic;
using UnityEngine;

public static class GridPathfinder
{
    static readonly Vector2Int[] Directions =
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

    public static Vector2Int NextStep(HashSet<Vector2Int> blocked, RectInt bounds, Vector2Int start, Vector2Int goal)
    {
        if (start == goal)
        {
            return start;
        }

        Queue<Vector2Int> frontier = new Queue<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();

        frontier.Enqueue(start);
        cameFrom[start] = start;

        // Breadth-first search: every step costs the same, so the first
        // time we reach the goal is already the shortest path.
        while (frontier.Count > 0)
        {
            Vector2Int current = frontier.Dequeue();
            if (current == goal)
            {
                break;
            }

            for (int i = 0; i < Directions.Length; i++)
            {
                Vector2Int next = current + Directions[i];
                if (!bounds.Contains(next) || blocked.Contains(next) || cameFrom.ContainsKey(next))
                {
                    continue;
                }

                cameFrom[next] = current;
                frontier.Enqueue(next);
            }
        }

        if (!cameFrom.ContainsKey(goal))
        {
            return start;
        }

        Vector2Int step = goal;
        // Walk back from the goal until the cell right after start.
        while (cameFrom[step] != start)
        {
            step = cameFrom[step];
        }

        return step;
    }
}
