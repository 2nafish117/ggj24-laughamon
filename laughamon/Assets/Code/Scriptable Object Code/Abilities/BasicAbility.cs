using DG.Tweening;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Ability", menuName = "Custom/Ability")]
public class BasicAbility : Ability, IJokeResultHandler
{
    [Header("Animation Delays")]
    public float ExecutionTime = 3f;

    public override void ExecuteSucceeded()
    {
        //Announcer.Instance.Say($"{source.name} used an ability on {target.name}", 2f);

        if (IsAJoke)
        {
            JokeData joke = JokeDataArray[Random.Range(0, JokeDataArray.Length)];

            CombatHUDManager.Instance.ShowJokeQTE(joke, this);
            return;
        }

        StartAbilityExecution();
        if (!IsSelfTargeting)
        {
            ApplyLaugh();
        }
        else
        {
            ApplyToSelf();
        }

        if (BuffsList.Length > 0)
        {
            source.AddBuffs(BuffsList);
        }

        if (HasDamageModifier)
        {
            source.AddDamageModifiers(DamageModifiers, this);
        }

        if (MultiHits > 0)
        {
            source.MultiHits = MultiHits;
        }

        AddQueuedAbilities();
    }

    private void AddQueuedAbilities()
    {
        for (int i = 0; i < ExtraTurns; i++)
        {
            source.QueueAbility(ChargeAbility);
        }
    }

    private void ApplyLaugh()
    {
        int consecutiveMuliplier = source.ActionExecuter.GetConsecutiveCount(this);
        var effectiveness = target.CharacterProfile.GetEffectiveness(this);
        ReactionMultiplierPairs reactionMultiplier = GetReactionMultiplier(effectiveness);
        float multiplier = 1; ;
        if (reactionMultiplier == null)
        {
            multiplier = 1;
        }
        else
        {
            multiplier = reactionMultiplier.GetMultiplier(consecutiveMuliplier);
        }

        float damage = LaughPoint * multiplier;

        CombatLogger.Instance.AddLog(UsageText);

        //Buff calculations

        Buff targetDefensiveBuff = target.GetDefensiveBuff(damageType);
        if (targetDefensiveBuff != null)
        {
            damage = Mathf.Clamp(damage - targetDefensiveBuff.value, 0, 1000f);

            AbilityReactionEffectiveness buffEffectiveness = AbilityReactionEffectiveness.SuperEffective;
            if (damage > 1f) buffEffectiveness = AbilityReactionEffectiveness.NotVeryEffective;

            ReactionTextPairs buffReaction = targetDefensiveBuff.GetReactionTextFor(buffEffectiveness);
            if (buffReaction != null)
            {
                CombatLogger.Instance.AddLog(buffReaction.GetRandomReaction());
            }
        }
        else
        {
            ReactionTextPairs reaction = GetReactionTextFor(effectiveness);
            if (reaction != null)
            {
                CombatLogger.Instance.AddLog(reaction.GetRandomReaction());
            }
        }

        if (target.HasActiveDamageModifiers)
        {
            target.ActiveDamageModifiers.ModifyDamageApplication(source, target, damage);
            target.RemoveExpiredDamageModifiers();
        }
        else
        {
            target.LaughterPoints.Laugh(damage);
        }

        DOVirtual.DelayedCall(ExecutionTime, EndAbilityExecution);
    }

    private void ApplyToSelf()
    {
        CombatLogger.Instance.AddLog(UsageText);

        source.LaughterPoints.Laugh(LaughPoint);

        if (target.HasActiveDamageModifiers)
        {
            target.RemoveExpiredDamageModifiers();
        }

        DOVirtual.DelayedCall(ExecutionTime, EndAbilityExecution);
    }

    public override void ExecuteFailed()
    {
        StartAbilityExecution();
        DOVirtual.DelayedCall(ExecutionTime, EndAbilityExecution);
    }

    public void HandleJokeResult(bool success)
    {
        if (success)
        {
            StartAbilityExecution();
        }
        else
        {
            ExecuteFailed();
        }

        switch (CombatManager.Instance.IsPlayerTurn, success)
        {
            case (true, true):
                ApplyLaugh();
                return;

            case (false, true):
                ApplyLaugh();
                return;

            case (false, false):
                ApplyLaugh();
                return;
        }
    }
}
