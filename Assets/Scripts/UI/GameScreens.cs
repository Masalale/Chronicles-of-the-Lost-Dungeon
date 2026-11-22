using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class GameScreens : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject howToPlayPanel;
    [SerializeField] GameObject levelCompletePanel;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Health playerHealth;
    [SerializeField] TMP_Text rewardText;
    [SerializeField] GameObject victoryPanel;
    [SerializeField] Image[] victoryShards;

    bool levelOver;

    void Start()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        levelCompletePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);

        GameManager.Instance.PauseChanged += OnPauseChanged;
        GameManager.Instance.LevelCompleted += ShowLevelComplete;
        GameManager.Instance.GameOver += ShowGameOver;
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PauseChanged -= OnPauseChanged;
            GameManager.Instance.LevelCompleted -= ShowLevelComplete;
            GameManager.Instance.GameOver -= ShowGameOver;
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TryToggle();
        }
    }

    public void PauseButton()
    {
        AudioManager.Instance.PlayClick();
        TryToggle();
    }

    public void Resume()
    {
        AudioManager.Instance.PlayClick();
        GameManager.Instance.SetPaused(false);
    }

    public void Retry()
    {
        AudioManager.Instance.PlayClick();
        GameManager.Instance.StartLevel(GameManager.Instance.CurrentLevelIndex);
    }

    public void Next()
    {
        AudioManager.Instance.PlayClick();
        GameManager.Instance.NextLevel();
    }

    public void Menu()
    {
        AudioManager.Instance.PlayClick();
        GameManager.Instance.GoToMenu();
    }

    public void ShowSettings()
    {
        AudioManager.Instance.PlayClick();
        ShowPausePage(settingsPanel);
    }

    public void ShowHowToPlay()
    {
        AudioManager.Instance.PlayClick();
        ShowPausePage(howToPlayPanel);
    }

    public void BackToPause()
    {
        AudioManager.Instance.PlayClick();
        ShowPausePage(pausePanel);
    }

    void TryToggle()
    {
        // Frozen but not paused means the riddle panel stopped time.
        // Pausing there, or after the level ends, would unfreeze the game.
        bool otherPanelOpen = !GameManager.Instance.IsPaused && Time.timeScale == 0f;

        if (!levelOver && !otherPanelOpen)
        {
            GameManager.Instance.TogglePause();
        }
    }

    void OnPauseChanged(bool paused)
    {
        settingsPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        pausePanel.SetActive(paused);
    }

    void ShowPausePage(GameObject page)
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        page.SetActive(true);
    }

    void ShowLevelComplete()
    {
        levelOver = true;

        if (!GameManager.Instance.HasNextLevel())
        {
            victoryPanel.SetActive(true);
            ShowVictoryShards();
            return;
        }

        levelCompletePanel.SetActive(true);
        rewardText.text = "Shard recovered: " + GameManager.Instance.Inventory.ShardCount + " of " + GameManager.Instance.LevelCount;
    }

    void ShowVictoryShards()
    {
        List<SiloResult> results = new List<SiloResult>();
        for (int i = 0; i < GameManager.Instance.LevelCount; i++)
        {
            results.Add(new SiloResult { silo = i, torch = SaveManager.Instance.Data.levels[i].torchLeft });
        }

        InsertionSorter.SortBrightestFirst(results);

        for (int i = 0; i < results.Count; i++)
        {
            Color c = GameManager.Instance.GetLevel(results[i].silo).color;
            float glow = (float)results[i].torch / playerHealth.Max;
            victoryShards[i].color = new Color(c.r * glow, c.g * glow, c.b * glow, 1f);
        }
    }

    void ShowGameOver()
    {
        levelOver = true;
        gameOverPanel.SetActive(true);
    }
}
