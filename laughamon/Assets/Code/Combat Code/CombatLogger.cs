using System.Text;
using TMPro;
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
        Scroll.verticalNormalizedPosition = 1;
    }
}
