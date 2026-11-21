using UnityEngine;
using UnityEngine.Networking;

// Field names must match the API's JSON keys for JsonUtility.
[System.Serializable]
public class TriviaResponse
{
    public int response_code;
    public TriviaResult[] results;
}

[System.Serializable]
public class TriviaResult
{
    public string question;
    public string correct_answer;
}

public class TriviaGate : MonoBehaviour
{
    // encode=url3986 keeps quotes and symbols safe; Parse decodes them.
    [SerializeField] string url = "https://opentdb.com/api.php?amount=5&category=9&difficulty=easy&type=boolean&encode=url3986";
    [SerializeField] int timeoutSeconds = 5;
    [SerializeField] TriviaResult[] fallbackQuestions;

    UnityWebRequest request;
    TriviaResult[] questions;
    int next;

    public bool IsReady
    {
        get
        {
            return questions != null;
        }
    }

    // No coroutine: just check every frame if the request is done.
    void Update()
    {
        if (request == null || !request.isDone)
        {
            return;
        }

        questions = Parse(request);
        request.Dispose();
        request = null;
    }

    void OnDestroy()
    {
        if (request != null)
        {
            request.Dispose();
        }
    }

    public void Fetch()
    {
        request = UnityWebRequest.Get(url);
        request.timeout = timeoutSeconds;
        request.SendWebRequest();
    }

    public TriviaResult Next()
    {
        TriviaResult question = questions[next % questions.Length];
        next++;
        return question;
    }

    TriviaResult[] Parse(UnityWebRequest finished)
    {
        if (finished.result != UnityWebRequest.Result.Success)
        {
            return Fallback();
        }

        TriviaResponse response;

        try
        {
            response = JsonUtility.FromJson<TriviaResponse>(finished.downloadHandler.text);
        }
        catch (System.Exception)
        {
            return Fallback();
        }

        if (response == null || response.response_code != 0 || response.results == null || response.results.Length == 0)
        {
            return Fallback();
        }

        for (int i = 0; i < response.results.Length; i++)
        {
            response.results[i].question = UnityWebRequest.UnEscapeURL(response.results[i].question);
            response.results[i].correct_answer = UnityWebRequest.UnEscapeURL(response.results[i].correct_answer);
        }
        return response.results;
    }

    TriviaResult[] Fallback()
    {
        // Random start, so offline players don't always get the same question.
        next = Random.Range(0, fallbackQuestions.Length);
        return fallbackQuestions;
    }
}
