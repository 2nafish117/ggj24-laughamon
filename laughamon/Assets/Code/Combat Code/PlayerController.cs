using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterControllerLaugh, IAbilityExecutionHandler
{
    public static PlayerController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    protected override void OnStart()
    {
        LaughterPoints.Init(CharacterProfile.MaxHealth);
        InventoryManager.Init(CharacterProfile.Inventory);
        EffectHandler.Init(this);
        AnimationController.Init();
    }

    public override void OnAfterAbilityExecuted(Ability ability)
    {
        base.OnAfterAbilityExecuted(ability);
        CombatLogger.OnLogEmptied += EndPlayerTurn;
    }

    private void EndPlayerTurn()
    {
        CombatLogger.OnLogEmptied -= EndPlayerTurn;
        CombatManager.Instance.EndPlayerTurn();
    }
}
