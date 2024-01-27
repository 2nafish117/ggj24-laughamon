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
    }

    public void SetUpCombat(CharacterProfile enemyProfile)
    {
        AIController.Instance.Init(enemyProfile);
        CombatHUDManager.Instance.ShowStartScreen(PlayerController.Instance.CharacterProfile.Name, enemyProfile.Name);
    }

    public void StartCombat()
    {
        IsCombatOver = false;
        //Can be determined by a random roll.
        IsPlayerTurn = true;
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
        bool playerVictory = AIController.Instance.LaughterPoints.IsDead;
        Announcer.Instance.Say(playerVictory ? "You Won!" : "You Lost..", 5f);
        CombatHUDManager.Instance.ShowCombatEndScreen(playerVictory);
        DOVirtual.DelayedCall(5f, ShowLevelScreen);
    }

    private void ShowLevelScreen()
    {
        GameManager.Instance.ShowLevelScreen(AIController.Instance.LaughterPoints.IsDead);
    }
}
