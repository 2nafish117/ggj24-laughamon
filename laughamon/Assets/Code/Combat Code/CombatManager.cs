using DG.Tweening;
using System;
using UnityEngine;


public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    public event Action<bool> OnTurnChanged;
    public event Action OnCombatStarted;
    public event Action OnCombatEnded;

    [field: SerializeField]
    public bool IsPlayerTurn { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        OnTurnChanged += AnnounceTurn;
        DOVirtual.DelayedCall(0.2f, StartCombat);
    }

    public void StartCombat()
    {
        print("Starting combat");
        IsPlayerTurn = true;

        OnCombatStarted?.Invoke();
        OnTurnChanged?.Invoke(IsPlayerTurn);

    }

    public void StartPlayerTurn()
    {
        IsPlayerTurn = true;
        OnTurnChanged?.Invoke(IsPlayerTurn);
    }

    public void EndPlayerTurn()
    {
        IsPlayerTurn = false;
        OnTurnChanged?.Invoke(IsPlayerTurn);
    }

    private void AnnounceTurn(bool _)
    {
        Announcer.Instance.Say(IsPlayerTurn ? "Your Turn" : "Enemy's Turn", 2f);
    }

    public void EndCombat()
    {
        OnCombatEnded?.Invoke();
        Announcer.Instance.Say(IsPlayerTurn ? "Player Victory" : "Player Defeat", 5f);
    }
}
