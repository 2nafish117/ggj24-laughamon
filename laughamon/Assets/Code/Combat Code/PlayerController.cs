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
        CombatManager.Instance.EndPlayerTurn();
    }
}
