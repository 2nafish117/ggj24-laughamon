using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    public event Action<bool> OnTurnChanged;
    public event Action OnCombatPrep;
    public event Action OnCombatStarted;
    public event Action OnCombatEnded;

    public bool IsCombatOver { get; private set; } = false;

    [field: SerializeField]
    public bool IsPlayerTurn { get; private set; }

    [SerializeField]
    private Transform VfxRoot;

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
        OnCombatPrep?.Invoke();
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
        bool playerVictory = AIController.Instance.LaughterPoints.IsDead;
        OnCombatEnded?.Invoke();
        Announcer.Instance.Say(playerVictory ? "You Won!" : "You Lost..", 5f);
        CombatHUDManager.Instance.ShowCombatEndScreen(playerVictory);
        GameManager.Instance.OnCombatFinished(playerVictory);
    }

    public GameObject SpawnVFX(GameObject prefab, Vector3 worldPos)
    {
        var pos = worldPos + prefab.transform.position;
        return Instantiate(prefab, pos, prefab.transform.rotation, VfxRoot);
    }

}
