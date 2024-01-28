using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface IJokeResultHandler
{
    public void HandleJokeResult(bool success);
}

public class UIJokePanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI jokeStartText;

    [SerializeField]
    private TextMeshProUGUI[] jokeOptions;

    [SerializeField]
    private Image timerFill;

    [SerializeField]
    private GameObject optionPanel;

    [SerializeField]
    private GameObject successMessage;

    [SerializeField]
    private TextMeshProUGUI successText;

    [SerializeField]
    private GameObject failureMessage;

    [SerializeField]
    private TextMeshProUGUI failureText;

    [SerializeField]
    private GameObject resultPanel;

    private int correctIndex;
    private JokeData jokeData;
    private IJokeResultHandler resultHandler;
    private bool success;

    public void SetJoke(JokeData jokeData, IJokeResultHandler resultHandler)
    {
        this.jokeData = jokeData;
        this.resultHandler = resultHandler;
        success = false;

        jokeStartText.SetText(jokeData.JokeStart);
        successText.SetText(jokeData.CorrectReaction);
        failureText.SetText(jokeData.WrongReaction);

        List<string> allOptions = new List<string>(jokeData.WrongAnswers)
        {
            jokeData.CorrectAnswer
        };

        for (int i = 0; i < jokeOptions.Length; i++)
        {
            var value = allOptions.GetRandom(out var index);
            jokeOptions[i].SetText(value);

            allOptions.RemoveAtSwapBack(index);
            if (jokeData.IsCorrectAnswer(value))
            {
                correctIndex = i;
            }
        }

        optionPanel.SetActive(true);
        resultPanel.SetActive(false);
        gameObject.SetActive(true);

        StartTimer(jokeData.AnswerTime);
    }

    private void StartTimer(float duration)
    {
        StartCoroutine(TimerTick(duration));
    }

    IEnumerator TimerTick(float duration)
    {
        float timer = 0;
        while (timer < duration)
        {
            yield return null;
            timer += Time.deltaTime;
            timerFill.fillAmount = 1 - Mathf.Clamp01(timer / duration);
        }

        yield return null;
        timerFill.fillAmount = 0;
        OnQTEFailed();
    }

    public void OnOptionClicked(int index)
    {
        StopAllCoroutines();
        success = index == correctIndex;
        if (success)
        {
            OnQTESuccess();
        }
        else
        {
            OnQTEFailed();
        }
    }

    public void OnQTEFailed()
    {
        CompleteJoke();
        ShowResult(false);
    }

    public void OnQTESuccess()
    {
        CompleteJoke();
        ShowResult(true);
    }

    private void CompleteJoke()
    {
        optionPanel.SetActive(false);
        jokeStartText.SetText(jokeData.FullJoke);
    }

    private void ShowResult(bool success)
    {
        successMessage.SetActive(success);
        failureMessage.SetActive(!success);
        resultPanel.SetActive(true);
    }

    public void ResumeCombat()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
        resultHandler?.HandleJokeResult(success);
    }
}
