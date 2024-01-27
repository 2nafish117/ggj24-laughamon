using System.Collections;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CombatLogger : MonoBehaviour
{
    public static CombatLogger Instance;

    private void Awake()
    {
        Instance = this;
    }

    public TextMeshProUGUI LogText;
    public ScrollRect Scroll;
    public StringBuilder logBuilder;

    private void Start()
    {
        logBuilder = new StringBuilder(32_762);
        AddLog("Combat Started.");
    }

    public void AddLog(string log)
    {
        logBuilder.AppendLine(log);
        LogText.SetText(logBuilder.ToString());

        StartCoroutine(ScrollAfter());
    }

    IEnumerator ScrollAfter()
    {
        yield return null;
        Scroll.verticalNormalizedPosition = 0;
    }

    private string[] ParseRawStringToLogs(string raw)
    {
        string polished = raw.Replace("[target]", AIController.Instance.CharacterProfile.Name);

        string[] broken = polished.Split("/n");

        return broken;
    }
}
