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

    public override void OnAfterAbilityExecuted(Ability ability)
    {
        base.OnAfterAbilityExecuted(ability);
        if (CombatLogger.Instance.IsLogEmpty) EndPlayerTurn();
        else 
            CombatLogger.OnLogEmptied += EndPlayerTurn;
    }

    private void EndPlayerTurn()
    {
        CombatLogger.OnLogEmptied -= EndPlayerTurn;
        CombatManager.Instance.EndPlayerTurn();
    }
}
