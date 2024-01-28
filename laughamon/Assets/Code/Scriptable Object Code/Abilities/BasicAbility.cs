using DG.Tweening;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Ability", menuName = "Custom/Ability")]
public class BasicAbility : Ability, IJokeResultHandler
{
    [Header("Animation Delays")]
    public float ExecutionTime = 3f;

    private JokeData currentJoke; 

    public override void ExecuteSucceeded()
    {
        //Announcer.Instance.Say($"{source.name} used an ability on {target.name}", 2f);
        StartAbilityExecution();

        if (IsAJoke)
        {
            CombatLogger.Instance.AddLog(UsageText);

            //Buff targetDefensiveBuff = target.GetDefensiveBuff(DamageType.Joke);

            //if(targetDefensiveBuff != null)
            //{
            //    CombatLogger.Instance.AddLog(JokeFailedText);
            //    HandleJokeResult(false);
            //    return;
            //}

            currentJoke = JokeDataArray[Random.Range(0, JokeDataArray.Length)];

            CombatLogger.OnLogEmptied += ShowJoke;

            return;
        }

        CombatLogger.Instance.AddLog(UsageText);

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

    private void ShowJoke()
    {
        CombatLogger.OnLogEmptied -= ShowJoke;
        CombatHUDManager.Instance.ShowJokeQTE(currentJoke, this);
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
            //StartAbilityExecution();
        }
        else
        {
            //ExecuteFailed();
        }

        switch (CombatManager.Instance.IsPlayerTurn, success)
        {
            case (true, true):
                ApplyLaugh();
                JokeReactionCall();
                break;

            case (false, true):
                //ApplyLaugh();
                DOVirtual.DelayedCall(ExecutionTime, EndAbilityExecution);
                break;

            case(false, false):
                ApplyLaugh();
                break;

            case(true, false):
                DOVirtual.DelayedCall(ExecutionTime, EndAbilityExecution);
                break;
        }
    }

    private void JokeReactionCall()
    {
        DOVirtual.DelayedCall(JokeReactionDelay, TriggerTargetJokeAnimation);
    }
}
