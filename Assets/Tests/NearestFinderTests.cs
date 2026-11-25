using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class NearestFinderTests
{
    [Test]
    public void FindNearest_FromOrigin_ReturnsIndexOfClosestPoint()
    {
        List<Vector2> points = new List<Vector2>
        {
            new Vector2(5f, 0f),
            new Vector2(1f, 1f),
            new Vector2(-3f, 0f)
        };

        // (1, 1) is about 1.4 away, closer than the other two.
        int index = NearestFinder.FindNearest(Vector2.zero, points);

        Assert.AreEqual(1, index);
    }
}
