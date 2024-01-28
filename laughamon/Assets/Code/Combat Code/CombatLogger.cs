using System.Collections;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CombatLogger : MonoBehaviour
{
    public static CombatLogger Instance;

    private Queue<LogLineInfo> logQueue = new Queue<LogLineInfo>();

    Coroutine ActiveLogDisplayCoroutine = null;

    public static event System.Action OnLogProgressed;
    public static event System.Action OnLogEmptied;

    struct LogLineInfo
    {
        public string content;
        public float minDelay;

        public LogLineInfo(string _content, float _minDelay)
        {
            content = _content;
            minDelay = _minDelay;
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    public TextMeshProUGUI LogText;
    public ScrollRect Scroll;
    public StringBuilder logBuilder;
    public TextBox textDisplayHelper;
    public GameObject blinkingArrow;

    public bool IsLogEmpty()
    {
        return LogText.text == "" && textDisplayHelper.isScrollingText == false;
    }

    private void Start()
    {
        logBuilder = new StringBuilder(32_762);
        //AddLog("Combat Started.");
        LogText.SetText("");
    }

    private void OnEnable()
    {
        PlayerInput.OnProgressPrompt += ProgressLog;
        textDisplayHelper.onFinishedScrollingText += ShowBlinkingArrow;
    }

    private void OnDisable()
    {
        PlayerInput.OnProgressPrompt -= ProgressLog;
        textDisplayHelper.onFinishedScrollingText -= ShowBlinkingArrow;
        StopAllCoroutines();
        ActiveLogDisplayCoroutine = null;
    }

    public void AddLog(string rawString, float minDelay = 0f)
    {
        List<string> loglines = ParseRawStringToLogs(rawString);
        if (loglines.Count == 0)
            return;

        logQueue.Enqueue(new LogLineInfo(loglines[0], minDelay));
        for (int i = 1; i < loglines.Count; i++)
        {
            logQueue.Enqueue(new LogLineInfo(loglines[i], 0f));
        }

        ProgressLog();
    }

    private void DisplayInLog(string line)
    {
        //logBuilder.AppendLine(line);
        //LogText.SetText(logBuilder.ToString());

        HideBlinkingArrow();

        textDisplayHelper.DisplayText(line);

        StartCoroutine(ScrollAfter());
    }

    private void ProgressLog()
    {
        if (ActiveLogDisplayCoroutine == null && logQueue.Count > 0)
        {
            ActiveLogDisplayCoroutine = StartCoroutine(DisplayLogQueueRoutine());
        }

        else if (ActiveLogDisplayCoroutine == null && logQueue.Count == 0)
        {
            HideBlinkingArrow();
            LogText.SetText("");
            OnLogEmptied?.Invoke();
        }
    }

    private void ShowBlinkingArrow()
    {
        if (ActiveLogDisplayCoroutine == null && textDisplayHelper.isScrollingText == false)
        {
            blinkingArrow.SetActive(true);
        }
    }

    private void HideBlinkingArrow()
    {
        blinkingArrow.SetActive(false);
    }




    IEnumerator ScrollAfter()
    {
        yield return null;
        Scroll.verticalNormalizedPosition = 0;
    }

    IEnumerator DisplayLogQueueRoutine()
    {

        LogLineInfo first = logQueue.Dequeue();
        DisplayInLog(first.content);

        yield return new WaitForSeconds(first.minDelay);

        ActiveLogDisplayCoroutine = null;
        ShowBlinkingArrow();

        OnLogProgressed?.Invoke();
    }

    private List<string> ParseRawStringToLogs(string raw)
    {
        //string userName = CombatManager.Instance.IsPlayerTurn ?
        //    PlayerController.Instance.CharacterProfile.Name : AIController.Instance.CharacterProfile.Name;

        //string targetName = CombatManager.Instance.IsPlayerTurn ?
        //     AIController.Instance.CharacterProfile.Name : PlayerController.Instance.CharacterProfile.Name;

        //string polished = raw.Replace("[user]", userName);
        //polished = polished.Replace("[target]", targetName);

        string polished = raw.Replace("[enemy]", AIController.Instance.CharacterProfile.Name);

        List<string> broken = polished.Split("\n", System.StringSplitOptions.RemoveEmptyEntries).ToListPooled<string>();

        if (broken.Count > 0)
        {
            if (broken[broken.Count - 1].Length <= 1)
            {
                broken.RemoveAt(broken.Count - 1); //removes newline substrings
            }
        }


        return broken;
    }
}
