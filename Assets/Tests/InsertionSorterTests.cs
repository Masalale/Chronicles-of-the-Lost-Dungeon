using System.Collections.Generic;
using NUnit.Framework;

public class InsertionSorterTests
{
    [Test]
    public void SortBrightestFirst_OrdersByTorchAndKeepsTies()
    {
        List<SiloResult> results = new List<SiloResult>
        {
            new SiloResult { silo = 0, torch = 2 },
            new SiloResult { silo = 1, torch = 5 },
            new SiloResult { silo = 2, torch = 3 },
            new SiloResult { silo = 3, torch = 5 }
        };

        InsertionSorter.SortBrightestFirst(results);

        // Silos 1 and 3 tie on 5, so 1 must stay ahead of 3.
        Assert.AreEqual(1, results[0].silo);
        Assert.AreEqual(3, results[1].silo);
        Assert.AreEqual(2, results[2].silo);
        Assert.AreEqual(0, results[3].silo);
    }
}
