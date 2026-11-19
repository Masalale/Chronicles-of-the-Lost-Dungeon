using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] int levelCount = 5;

    public static SaveManager Instance;

    public SaveData Data { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Load();
        Enemy.AnyDied += OnEnemyDied;
    }

    void OnDestroy()
    {
        Enemy.AnyDied -= OnEnemyDied;
    }

    public void ResetProgress()
    {
        Data = new SaveData(levelCount);
        Save();
    }

    public void RecordLevelComplete(int levelIndex, int torch)
    {
        Data.levels[levelIndex].completed = true;
        Data.levels[levelIndex].torchLeft = torch;
        Data.torch = torch;

        Save();
    }

    void OnEnemyDied(Enemy enemy)
    {
        Data.enemiesDefeated++;
    }

    void Save()
    {
        string json = Data.ToJson();

#if UNITY_WEBGL
        // WebGL can't write files, so the save goes into browser storage.
        PlayerPrefs.SetString("save", json);
        PlayerPrefs.Save();
#else
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "save.json"), json);
#endif
    }

    void Load()
    {
        SaveData loaded = null;

        try
        {
            string json = ReadJson();
            if (!string.IsNullOrEmpty(json))
            {
                loaded = SaveData.FromJson(json);
            }
        }
        // A broken save file should start fresh, not crash the game.
        catch (System.Exception)
        {
            loaded = null;
        }

        // A save from a build with a different Silo count is thrown away.
        if (loaded == null || loaded.levels == null || loaded.levels.Count != levelCount)
        {
            Data = new SaveData(levelCount);
        }
        else
        {
            Data = loaded;
        }
    }

    string ReadJson()
    {
#if UNITY_WEBGL
        return PlayerPrefs.GetString("save", "");
#else
        string path = Path.Combine(Application.persistentDataPath, "save.json");
        if (!File.Exists(path))
        {
            return "";
        }

        return File.ReadAllText(path);
#endif
    }
}
