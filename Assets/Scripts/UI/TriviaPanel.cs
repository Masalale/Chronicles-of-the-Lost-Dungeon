using UnityEngine;
using TMPro;

public class TriviaPanel : MonoBehaviour
{
    // This script sits outside the panel so it keeps listening while hidden.
    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text questionText;
    [SerializeField] TMP_Text resultText;
    [SerializeField] GameObject answerButtons;
    [SerializeField] GameObject closeButton;

    RiddleKeeper keeper;
    TriviaResult question;
    bool correct;

    void Awake()
    {
        panel.SetActive(false);
    }

    void OnEnable()
    {
        RiddleKeeper.Asked += Show;
    }

    void OnDisable()
    {
        RiddleKeeper.Asked -= Show;
    }

    public void Answer(bool choice)
    {
        AudioManager.Instance.PlayClick();
        // The API sends the answer as the text True or False.
        correct = choice == (question.correct_answer == "True");
        resultText.text = correct ? "Right! The exit opens" : "Wrong! Your torch is scorched (-1). Ask again";
        answerButtons.SetActive(false);
        closeButton.SetActive(true);
    }

    public void Close()
    {
        AudioManager.Instance.PlayClick();
        panel.SetActive(false);
        Time.timeScale = 1f;
        keeper.Answer(correct);
    }

    void Show(RiddleKeeper asker, TriviaResult asked)
    {
        keeper = asker;
        question = asked;
        panel.SetActive(true);
        questionText.text = asked.question;
        resultText.text = "";
        answerButtons.SetActive(true);
        closeButton.SetActive(false);
        Time.timeScale = 0f;
    }
}
