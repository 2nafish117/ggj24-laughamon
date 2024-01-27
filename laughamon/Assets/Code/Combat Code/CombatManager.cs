using DG.Tweening;
using System;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    public event Action<bool> OnTurnChanged;
    public event Action OnCombatStarted;
    public event Action OnCombatEnded;

    public bool IsCombatOver { get; private set; } = false;

    [field: SerializeField]
    public bool IsPlayerTurn { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        OnTurnChanged += AnnounceTurn;
        DOVirtual.DelayedCall(2, StartCombat);
    }

    public void StartCombat()
    {
        print("Starting combat");
        IsCombatOver = false;
        OnCombatStarted?.Invoke();
        OnTurnChanged?.Invoke(IsPlayerTurn);

    }

    public void StartPlayerTurn()
    {
        if (IsCombatOver)
        {
            return;
        }

        IsPlayerTurn = true;
        OnTurnChanged?.Invoke(IsPlayerTurn);
    }

    public void EndPlayerTurn()
    {
        if (IsCombatOver)
        {
            return;
        }
        IsPlayerTurn = false;
        OnTurnChanged?.Invoke(IsPlayerTurn);
    }

    private void AnnounceTurn(bool _)
    {
        Announcer.Instance.Say(IsPlayerTurn ? "Your Turn" : "Enemy's Turn", -1);
    }

    public void EndCombat()
    {
        IsCombatOver = true;
        OnCombatEnded?.Invoke();
        string resultCombatLog = IsPlayerTurn ? "You killed him, how pathetic." : "Someone should go home and cry in a corner.";
        string announcerResult = AIController.Instance.LaughterPoints.IsDead ? "You Win,\nyay...." : "You lost, as fated";
        Announcer.Instance.Say(announcerResult, 5f);
        CombatLogger.Instance.AddLog(resultCombatLog);
        CombatLogger.Instance.AddLog("Combat Over!");
    }
}
