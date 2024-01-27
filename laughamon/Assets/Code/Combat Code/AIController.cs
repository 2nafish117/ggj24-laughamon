using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : CharacterControllerLaugh
{
    public static AIController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    protected override void HandleTurnChanged(bool isPlayerTurn)
    {
        if (isPlayerTurn)
            return;

        DOVirtual.DelayedCall(2f, AnnounceAITurn);
    }

    private void AnnounceAITurn()
    {
        Announcer.Instance.Say("Enemy is Thinking", 2);
        DOVirtual.DelayedCall(2, PlayAIMove);
    }

    private void PlayAIMove()
    {
        ExecuteAnAbility(PlayerController.Instance);
    }

    private void ExecuteAnAbility(CharacterControllerLaugh target)
    {
        bool useSpell = Random.Range(0, 2) == 0;
        if (useSpell)
        {
            Announcer.Instance.Say("Enemy Casted a Spell", 2f);
            ActionExecuter.Spells.GetRandom(out var index);
            ActionExecuter.ExecuteSpell(index, this, target, this);
        }
        else
        {
            Announcer.Instance.Say("Enemy Used an Ability", 2f);
            ActionExecuter.Abilities.GetRandom(out var index);
            ActionExecuter.ExecuteAbility(index, this, target, this);
        }
    }

    public override void OnAfterAbilityExecuted(Ability ability)
    {
        base.OnAfterAbilityExecuted(ability);
        CombatManager.Instance.StartPlayerTurn();
    }

}
