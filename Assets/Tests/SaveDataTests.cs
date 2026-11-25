using NUnit.Framework;

public class SaveDataTests
{
    [Test]
    public void FromJson_AfterToJson_KeepsValues()
    {
        SaveData data = new SaveData(5);
        data.levels[1].torchLeft = 4;
        data.torch = 3;
        data.levels[1].completed = true;

        // Same round trip as a real save and load.
        SaveData loaded = SaveData.FromJson(data.ToJson());

        Assert.AreEqual(4, loaded.levels[1].torchLeft);
        Assert.AreEqual(3, loaded.torch);
        Assert.IsTrue(loaded.levels[1].completed);
    }
}
