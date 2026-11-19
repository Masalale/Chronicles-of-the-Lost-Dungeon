public enum LevelStatus { Locked, Available, Completed }

// Static with no Unity types, so the tests can call it directly.
public static class Progression
{
    public static bool IsUnlocked(int levelIndex, bool[] completed)
    {
        if (levelIndex < 0 || levelIndex >= completed.Length)
        {
            return false;
        }

        if (levelIndex == 0)
        {
            return true;
        }

        return completed[levelIndex - 1];
    }

    public static LevelStatus GetStatus(int levelIndex, bool[] completed)
    {
        if (completed[levelIndex])
        {
            return LevelStatus.Completed;
        }

        if (IsUnlocked(levelIndex, completed))
        {
            return LevelStatus.Available;
        }

        return LevelStatus.Locked;
    }
}
