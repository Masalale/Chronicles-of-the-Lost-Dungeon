using NUnit.Framework;

public class InventoryTests
{
    [Test]
    public void AddShard_MarksShardAndRaisesCount()
    {
        // No GameObject needed, Inventory is a plain class.
        Inventory inventory = new Inventory(new bool[5]);

        inventory.AddShard(2);

        Assert.IsTrue(inventory.HasShard(2));
        Assert.IsFalse(inventory.HasShard(0));
        Assert.AreEqual(1, inventory.ShardCount);
    }
}
