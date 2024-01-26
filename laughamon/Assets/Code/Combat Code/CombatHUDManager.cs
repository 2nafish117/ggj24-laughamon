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

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CombatManager.Instance.OnTurnChanged += SwitchTurn;
        playerBar.Initialize(PlayerController.Instance.LaughterPoints);
        enemyBar.Initialize(AIController.Instance.LaughterPoints);
    }

    public void SwitchTurn(bool isPlayerTurn)
    {
        playerTurnActionsCanvas.interactable = isPlayerTurn;
        playerTurnActionsCanvas.DOFade(isPlayerTurn ? 1 : 0, 0.4f).SetEase(isPlayerTurn ? Ease.InExpo : Ease.OutExpo);
    }

    public void OnAbilityUsed(int index)
    {
        PlayerController.Instance.UseAbilityAtIndex(index, this, AIController.Instance);
    }

    public void OnSpellUsed(int index)
    {
        PlayerController.Instance.UseSpellAtIndex(index, this, AIController.Instance);
    }

    public void OnBeforeAbilityExecuted(Ability ability)
    {

    }

    public void OnAfterAbilityExecuted(Ability ability)
    {

    }
}
