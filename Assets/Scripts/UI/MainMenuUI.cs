using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject levelSelectPanel;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject howToPlayPanel;
    [SerializeField] GameObject introPanel;
    [SerializeField] Button continueButton;
    [SerializeField] LevelButton[] levelButtons;

    void Start()
    {
        ShowPanel(mainPanel);

        bool[] flags = SaveManager.Instance.Data.CompletedFlags();
        bool anyCompleted = false;
        for (int i = 0; i < flags.Length; i++)
        {
            if (flags[i])
            {
                anyCompleted = true;
            }
        }

        continueButton.interactable = anyCompleted;

        AudioManager.Instance.PlayMenuMusic();
    }

    public void ShowIntro()
    {
        AudioManager.Instance.PlayClick();
        ShowPanel(introPanel);
    }

    public void StartGame()
    {
        AudioManager.Instance.PlayClick();
        GameManager.Instance.StartNewGame();
    }

    public void ContinueGame()
    {
        AudioManager.Instance.PlayClick();
        GameManager.Instance.ContinueGame();
    }

    public void ShowMain()
    {
        AudioManager.Instance.PlayClick();
        ShowPanel(mainPanel);
    }

    public void ShowLevelSelect()
    {
        AudioManager.Instance.PlayClick();
        ShowPanel(levelSelectPanel);

        bool[] flags = SaveManager.Instance.Data.CompletedFlags();
        // Refreshed every time, since finishing a Silo changes the buttons.
        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].Setup(i, Progression.GetStatus(i, flags));
        }
    }

    public void ShowSettings()
    {
        AudioManager.Instance.PlayClick();
        ShowPanel(settingsPanel);
    }

    public void ShowHowToPlay()
    {
        AudioManager.Instance.PlayClick();
        ShowPanel(howToPlayPanel);
    }

    public void ExitGame()
    {
        AudioManager.Instance.PlayClick();
        GameManager.Instance.QuitGame();
    }

    void ShowPanel(GameObject panel)
    {
        mainPanel.SetActive(false);
        levelSelectPanel.SetActive(false);
        settingsPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        introPanel.SetActive(false);
        panel.SetActive(true);
    }
}
