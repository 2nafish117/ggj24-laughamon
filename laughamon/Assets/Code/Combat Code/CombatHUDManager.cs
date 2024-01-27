using DG.Tweening;
using System;
using UnityEngine;

public class CombatHUDManager : MonoBehaviour, IAbilityExecutionHandler
{
    public static CombatHUDManager Instance { get; private set; }

    [SerializeField]
    private CanvasGroup playerTurnActionsCanvas;

    [SerializeField]
    private UILaughBar playerBar;

    [SerializeField]
    private UILaughBar enemyBar;

    [SerializeField]
    private UIInventory inventoryPanel;

    [SerializeField]
    private UIAbilityPanel abilityPanel;

    [SerializeField]
    private UICombatStartScreen combatStartScreen;

    [SerializeField]
    private UICombatResultScreen combatResultScreen;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CombatManager.Instance.OnCombatStarted += HandleCombatStarted;
        CombatManager.Instance.OnCombatStarted += InitUI;
        CombatManager.Instance.OnTurnChanged += SwitchTurn;
    }

    private void HandleCombatStarted()
    {
        SetCombatElements(true);
        combatResultScreen.gameObject.SetActive(false);
    }

    public void InitUI()
    {
        playerBar.Initialize(PlayerController.Instance.LaughterPoints);
        enemyBar.Initialize(AIController.Instance.LaughterPoints);
        inventoryPanel.Initialize(PlayerController.Instance.InventoryManager);
        abilityPanel.Initialize(PlayerController.Instance.InventoryManager);
    }

    public void SwitchTurn(bool isPlayerTurn)
    {
        playerTurnActionsCanvas.interactable = isPlayerTurn;
        playerTurnActionsCanvas.DOFade(isPlayerTurn ? 1 : 0, 0.4f).SetEase(isPlayerTurn ? Ease.InExpo : Ease.OutExpo);
    }

    public void OnAbilityUsed(int index)
    {
        if (CombatManager.Instance.IsPlayerTurn == false) return;

        playerTurnActionsCanvas.interactable = false;
        PlayerController.Instance.UseAbilityAtIndex(index, this, AIController.Instance);
    }

    public void OnSpellUsed(int index)
    {
        if (CombatManager.Instance.IsPlayerTurn == false) return;

        playerTurnActionsCanvas.interactable = false;
        PlayerController.Instance.UseSpellAtIndex(index, this, AIController.Instance);
    }

    public void OnBeforeAbilityExecuted(Ability ability)
    {

    }

    public void OnAfterAbilityExecuted(Ability ability)
    {

    }

    //public void SkipTurn()
    //{
    //    CombatLogger.Instance.AddLog($" {PlayerController.Instance} decides to do nothing.");
    //    CombatManager.Instance.EndPlayerTurn();
    //}

    public void ShowStartScreen(string playerName, string enemyName)
    {
        combatStartScreen.Init(playerName, enemyName);
        SetCombatElements(false);
    }

    public void SetCombatElements(bool active)
    {
        playerBar.gameObject.SetActive(active);
        enemyBar.gameObject.SetActive(active);
        abilityPanel.gameObject.SetActive(active);
        CombatLogger.Instance.gameObject.SetActive(active);
        Announcer.Instance.gameObject.SetActive(active);
    }

    internal void ShowCombatEndScreen(bool playerVictory)
    {
        SetCombatElements(false);
        combatResultScreen.SetResult(playerVictory);
    }
}
