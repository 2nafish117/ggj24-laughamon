using DG.Tweening;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Ability", menuName = "Custom/Ability")]
public class BasicAbility : Ability
{
    [Header("Animation Delays")]
    public float ExecutionTime = 3f;

    public override void ExecuteSucceeded()
    {
        //Announcer.Instance.Say($"{source.name} used an ability on {target.name}", 2f);
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

        AddQueuedAbilities();
        //DOVirtual.DelayedCall(2.5f, ApplyLaugh);
    }

    private void AddQueuedAbilities()
    {
        for(int i = 0; i < ExtraTurns; i++)
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

        Buff targetDefensiveBuff = target.GetDefensiveBuff();
        if (targetDefensiveBuff != null && (damageType == targetDefensiveBuff.damageType || targetDefensiveBuff.damageType == DamageType.Universal))
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

        target.LaughterPoints.Laugh(damage);

        DOVirtual.DelayedCall(ExecutionTime, EndAbilityExecution);
    }


    private void ApplyToSelf()
    {
        CombatLogger.Instance.AddLog(UsageText);

        source.LaughterPoints.Laugh(LaughPoint);

        DOVirtual.DelayedCall(ExecutionTime, EndAbilityExecution);
    }

    public override void ExecuteFailed()
    {
        StartAbilityExecution();
        DOVirtual.DelayedCall(ExecutionTime, EndAbilityExecution);
    }
}
