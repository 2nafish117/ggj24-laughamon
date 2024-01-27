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

    private void Start()
    {
        logBuilder = new StringBuilder(32_762);
        //AddLog("Combat Started.");
    }

    public void AddLog(string rawString, float minDelay = 0f)
    {
        string[] loglines = ParseRawStringToLogs(rawString);

        logQueue.Enqueue(new LogLineInfo(loglines[0], minDelay));
        for(int i=1;i < loglines.Length; i++)
        {
            logQueue.Enqueue(new LogLineInfo(loglines[i], 0f));
        }

        if(ActiveLogDisplay == null)
        {
            ActiveLogDisplay = StartCoroutine(DisplayLogQueueRoutine());
        }
    }

    private void DisplayInLog(string line)
    {
        //logBuilder.AppendLine(line);
        //LogText.SetText(logBuilder.ToString());
        textDisplayHelper.DisplayText(line);


        StartCoroutine(ScrollAfter());
    }


    IEnumerator ScrollAfter()
    {
        yield return null;
        Scroll.verticalNormalizedPosition = 0;
    }

    IEnumerator DisplayLogQueueRoutine()
    {
        while(logQueue.Count > 0)
        {
            LogLineInfo first = logQueue.Dequeue();
            DisplayInLog(first.content);

            yield return new WaitForSeconds(first.minDelay);
            playerInputPrompted = false;

            while (!playerInputPrompted)
            {
                //replace depending on Input system
                if (Input.anyKey)
                {
                    playerInputPrompted = true;
                    break;
                }
                yield return null;
            }
        }

        ActiveLogDisplay = null;
    }

    private string[] ParseRawStringToLogs(string raw)
    {
        string polished = raw.Replace("[target]", AIController.Instance.CharacterProfile.Name);
        polished = polished.Replace("[user]", PlayerController.Instance.CharacterProfile.Name);

        string[] broken = polished.Split("\n");

        return broken;
    }
}
