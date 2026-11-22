using UnityEngine;
using TMPro;

public class LoadingOverlay : MonoBehaviour
{
    [SerializeField] RoomLoader roomLoader;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] GameObject errorPanel;
    [SerializeField] TMP_Text errorText;

    void Awake()
    {
        roomLoader.LoadFinished += OnLoadFinished;
        roomLoader.LoadFailed += OnLoadFailed;

        loadingPanel.SetActive(true);
        errorPanel.SetActive(false);
    }

    void OnDestroy()
    {
        if (roomLoader != null)
        {
            roomLoader.LoadFinished -= OnLoadFinished;
            roomLoader.LoadFailed -= OnLoadFailed;
        }
    }

    // The only way out when a room fails to load.
    public void BackToMenu()
    {
        AudioManager.Instance.PlayClick();
        GameManager.Instance.GoToMenu();
    }

    void OnLoadFinished()
    {
        loadingPanel.SetActive(false);
    }

    void OnLoadFailed(string message)
    {
        loadingPanel.SetActive(false);
        errorPanel.SetActive(true);
        errorText.text = message;
    }
}
