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

    private bool playerInputPrompted = false;

    Coroutine ActiveLogDisplay = null;

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

    public void AddLog(string rawString, float minDelay = 0f)
    {
        string[] loglines = ParseRawStringToLogs(rawString);

        logQueue.Enqueue(new LogLineInfo(loglines[0], minDelay));
        for(int i=1;i < loglines.Length; i++)
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
        if (ActiveLogDisplay == null && logQueue.Count > 0)
        {
            ActiveLogDisplay = StartCoroutine(DisplayLogQueueRoutine());
        }

        else if(logQueue.Count == 0)
        {
            HideBlinkingArrow();
            LogText.SetText("");
            OnLogEmptied?.Invoke();
        }
    }

    private void ShowBlinkingArrow()
    {
        if(ActiveLogDisplay == null && textDisplayHelper.isScrollingText == false)
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

        ActiveLogDisplay = null;
        ShowBlinkingArrow();

        OnLogProgressed?.Invoke();
    }

    private string[] ParseRawStringToLogs(string raw)
    {
        string polished = raw.Replace("[target]", AIController.Instance.CharacterProfile.Name);
        polished = polished.Replace("[user]", PlayerController.Instance.CharacterProfile.Name);

        string[] broken = polished.Split("\n", System.StringSplitOptions.RemoveEmptyEntries);

        return broken;
    }
}
