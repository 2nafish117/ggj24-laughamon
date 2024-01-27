using System;
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

    private void Start()
    {
        CombatManager.Instance.OnCombatStarted += OnStart;
        CombatManager.Instance.OnTurnChanged += HandleTurnChanged;
        LaughterPoints.OnLaughPointsChanged += HandleLaughterChanged;

        LaughterPoints.Init(CharacterProfile.MaxHealth);
        InventoryManager.Init(CharacterProfile.Inventory);
        InventoryManager.EquipDefaults();

        ActionExecuter.Init(InventoryManager);
        EffectHandler.Init(this);
        AnimationController.Init();
    }

    protected override void OnStart()
    {
    }

    public override void OnAfterAbilityExecuted(Ability ability)
    {
        base.OnAfterAbilityExecuted(ability);
        if (CombatLogger.Instance.IsLogEmpty()) EndPlayerTurn();
        else
            CombatLogger.OnLogEmptied += EndPlayerTurn;
    }

    private void EndPlayerTurn()
    {
        CombatLogger.OnLogEmptied -= EndPlayerTurn;
        CombatManager.Instance.EndPlayerTurn();
    }
}
