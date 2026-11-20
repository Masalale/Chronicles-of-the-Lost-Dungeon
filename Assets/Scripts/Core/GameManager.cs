using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] LevelData[] levels;
    [SerializeField] string gameScene = "Game";
    [SerializeField] string menuScene = "MainMenu";

    public static GameManager Instance;

    public int CurrentLevelIndex { get; private set; }
    public LevelData CurrentLevel { get { return levels[CurrentLevelIndex]; } }
    public int LevelCount { get { return levels.Length; } }
    public Inventory Inventory { get; private set; }
    public bool IsPaused { get; private set; }

    public event System.Action LevelCompleted;
    public event System.Action GameOver;
    public event System.Action<bool> PauseChanged;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartLevel(int index)
    {
        if (!Progression.IsUnlocked(index, SaveManager.Instance.Data.CompletedFlags()))
        {
            return;
        }

        CurrentLevelIndex = index;
        // Each completed Silo gave a shard, so the save rebuilds the shards.
        Inventory = new Inventory(SaveManager.Instance.Data.CompletedFlags());
        Time.timeScale = 1f;
        IsPaused = false;
        SceneManager.LoadScene(gameScene);
    }

    public void StartNewGame()
    {
        SaveManager.Instance.ResetProgress();
        StartLevel(0);
    }

    public void ContinueGame()
    {
        bool[] flags = SaveManager.Instance.Data.CompletedFlags();

        int highest = 0;
        for (int i = 0; i < flags.Length; i++)
        {
            if (Progression.IsUnlocked(i, flags))
            {
                highest = i;
            }
        }

        StartLevel(highest);
    }

    public bool HasNextLevel()
    {
        return CurrentLevelIndex + 1 < levels.Length;
    }

    public LevelData GetLevel(int index)
    {
        return levels[index];
    }

    public void NextLevel()
    {
        StartLevel(CurrentLevelIndex + 1);
    }

    public void CompleteLevel(int torch)
    {
        SaveManager.Instance.RecordLevelComplete(CurrentLevelIndex, torch);
        Time.timeScale = 0f;

        LevelCompleted?.Invoke();
    }

    public void PlayerDied()
    {
        Time.timeScale = 0f;

        GameOver?.Invoke();
    }

    public void SetPaused(bool paused)
    {
        IsPaused = paused;
        Time.timeScale = paused ? 0f : 1f;

        PauseChanged?.Invoke(paused);
    }

    public void TogglePause()
    {
        SetPaused(!IsPaused);
    }

    // timeScale survives scene loads, so unfreeze before leaving.
    public void GoToMenu()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        SceneManager.LoadScene(menuScene);
    }

    public void QuitGame()
    {
// Application.Quit does nothing in the editor.
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
