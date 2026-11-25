using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class GridPathfinderTests
{
    [Test]
    public void NextStep_WhenWallBlocksDirectPath_StepsAroundIt()
    {
        // 3x3 grid with a wall between start and goal: the way is up or down.
        HashSet<Vector2Int> blocked = new HashSet<Vector2Int> { new Vector2Int(1, 0) };
        RectInt bounds = new RectInt(0, -1, 3, 3);

        Vector2Int step = GridPathfinder.NextStep(blocked, bounds, new Vector2Int(0, 0), new Vector2Int(2, 0));

        Assert.AreNotEqual(new Vector2Int(1, 0), step);
        Assert.AreEqual(0, step.x);
        Assert.AreEqual(1, Mathf.Abs(step.y));
    }
}
