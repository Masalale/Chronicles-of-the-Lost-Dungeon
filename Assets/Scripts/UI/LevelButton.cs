using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] Image background;
    [SerializeField] GameObject closedPadlock;
    [SerializeField] GameObject openPadlock;
    [SerializeField] TMP_Text numberText;
    [SerializeField] Color lockedColor = new Color(0.35f, 0.35f, 0.4f);
    [SerializeField] Color availableColor = Color.white;

    int index;

    public void Setup(int levelIndex, LevelStatus status)
    {
        index = levelIndex;
        numberText.text = (levelIndex + 1).ToString();

        if (status == LevelStatus.Locked)
        {
            closedPadlock.SetActive(true);
            openPadlock.SetActive(false);
            background.color = lockedColor;
            button.interactable = false;
        }
        else if (status == LevelStatus.Available)
        {
            closedPadlock.SetActive(false);
            openPadlock.SetActive(true);
            background.color = availableColor;
            button.interactable = true;
        }
        else
        {
            closedPadlock.SetActive(false);
            openPadlock.SetActive(false);
            // Completed Silos stay pressable so they can be replayed.
            background.color = GameManager.Instance.GetLevel(index).color;
            button.interactable = true;
        }
    }

    public void Select()
    {
        AudioManager.Instance.PlayClick();
        GameManager.Instance.StartLevel(index);
    }
}
