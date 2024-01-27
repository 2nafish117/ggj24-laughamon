using DG.Tweening;
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

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CombatManager.Instance.OnCombatStarted += InitUI;
        CombatManager.Instance.OnTurnChanged += SwitchTurn;
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

    public void SkipTurn()
    {
        CombatLogger.Instance.AddLog($" {PlayerController.Instance} decides to do nothing.");
        CombatManager.Instance.EndPlayerTurn();
    }
}
