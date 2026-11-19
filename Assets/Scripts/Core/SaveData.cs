using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelRecord
{
    public bool completed;
    public int torchLeft;
}

// JsonUtility only saves public fields, not properties.
[System.Serializable]
public class SaveData
{
    public List<LevelRecord> levels = new List<LevelRecord>();
    public int torch;
    public int enemiesDefeated;

    public SaveData(int levelCount)
    {
        for (int i = 0; i < levelCount; i++)
        {
            levels.Add(new LevelRecord());
        }
    }

    public bool[] CompletedFlags()
    {
        bool[] flags = new bool[levels.Count];
        for (int i = 0; i < levels.Count; i++)
        {
            flags[i] = levels[i].completed;
        }
        return flags;
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public static SaveData FromJson(string json)
    {
        return JsonUtility.FromJson<SaveData>(json);
    }
}
