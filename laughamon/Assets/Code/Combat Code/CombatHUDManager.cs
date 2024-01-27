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

    [SerializeField]
    private Camera combatCamera;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CombatManager.Instance.OnCombatPrep += HandleCombatPrep;
        CombatManager.Instance.OnCombatStarted += HandleCombatStarted;
        CombatManager.Instance.OnTurnChanged += SwitchTurn;
    }

    private void HandleCombatPrep()
    {
        combatResultScreen.gameObject.SetActive(false);
        InitUI();
    }

    private void HandleCombatStarted()
    {
        SetCombatElements(true);
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
        if (isPlayerTurn)
        {
            ShowPlayerControls(true);
            CombatLogger.OnLogProgressed += DismissControlsOnFirstCombatLog;
        }
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

    private void ShowPlayerControls(bool value)
    {
        playerTurnActionsCanvas.interactable = value;
        playerTurnActionsCanvas.DOFade(value ? 1 : 0, 0.4f).SetEase(value ? Ease.InExpo : Ease.OutExpo);
    }

    private void DismissControlsOnFirstCombatLog()
    {
        CombatLogger.OnLogProgressed -= DismissControlsOnFirstCombatLog;
        ShowPlayerControls(false);
    }

    internal void ShowCombatEndScreen(bool playerVictory)
    {
        SetCombatElements(false);
        combatResultScreen.SetResult(playerVictory);
    }

    public void ShowCombat(bool active)
    {
        combatCamera.gameObject.SetActive(active);
        gameObject.SetActive(active);
    }
}
