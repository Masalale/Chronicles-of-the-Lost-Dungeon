using NUnit.Framework;

public class ProgressionTests
{
    [Test]
    public void GetStatus_LevelAfterIncompleteLevel_IsLockedAndPreviousIsAvailable()
    {
        // Only Silo 0 is done, so Silo 1 opens and Silo 2 stays locked.
        bool[] completed = { true, false, false, false, false };

        Assert.AreEqual(LevelStatus.Locked, Progression.GetStatus(2, completed));
        Assert.AreEqual(LevelStatus.Available, Progression.GetStatus(1, completed));
    }
}
