using UnityEngine;
using DG.Tweening;

public abstract class Ability : ScriptableObject
{
    public float LaughPoint;
    public bool IsSelfTargeting;
    public Sprite AbilityIcon;
    public int ShopCost;
    [Range(0f, 1f)]
    public float SuccessChance = 1;
    public DamageType damageType;
    public AnimationKey AnimationKey;
    public bool TargetReact;
    public AnimationKey TargetAnimationKey;
    public float TargetAnimationDelay;
    public int ExtraTurns = 0;
    public Ability ChargeAbility;
    public Buff addsBuff;

    [Space]
    [TextArea]
    public string UsageText;

    [Space]
    public ReactionTextPairs[] Reactions;

    [Tooltip("Reaction multipler are only used if listed otherwise the default value will be one")]
    public ReactionMultiplierPairs[] ReactionMultipliers;

    [Header("DOT's")]
    public AbilityDOT[] DOTs;

    [Header("Cusotm Behaviours")]
    public AbilityCombatEffects[] CombatEffects;

    protected IAbilityExecutionHandler executionHandler;
    protected CharacterControllerLaugh source;
    protected CharacterControllerLaugh target;

    public void ExecuteAbility(CharacterControllerLaugh source, CharacterControllerLaugh target, IAbilityExecutionHandler executionHandler)
    {
        this.executionHandler = executionHandler;
        this.source = source;
        this.target = target;
        float roll = Random.Range(0f, 1f);
        if (roll < SuccessChance)
        {
            ExecuteSucceeded();
        }
        else
        {
            ExecuteFailed();
        }
    }

    /// <summary>
    /// Used to do things like apply damage and all
    /// </summary>
    public abstract void ExecuteSucceeded();
    /// <summary>
    /// Used to do things like you failed announcement and all
    /// </summary>
    public abstract void ExecuteFailed();

    public virtual void ExecuteDOT()
    {
        foreach (var dot in DOTs)
        {
            target.EffectHandler.AddDeBuff(source, target, dot);
        }
    }

    public virtual void ExecuteCombatEffects()
    {
        foreach (var effect in CombatEffects)
        {
            effect.ExecuteCustomEffect(source, target);
        }
    }

    public virtual void StartAbilityExecution()
    {
        executionHandler.OnBeforeAbilityExecuted(this);
        source.AnimationController.PlayAnimation(AnimationKey);
        if (TargetReact)
        {
            DOVirtual.DelayedCall(TargetAnimationDelay, TriggerTargetAnimation);
        }
    }

    private void TriggerTargetAnimation()
    {
        target.AnimationController.PlayAnimation(TargetAnimationKey);
    }

    public virtual void ExecuteOther()
    {
        ExecuteCombatEffects();
        ExecuteDOT();
    }

    public virtual void EndAbilityExecution()
    {
        executionHandler.OnAfterAbilityExecuted(this);
    }

    public ReactionTextPairs GetReactionTextFor(AbilityReactionEffectiveness effectiveness)
    {
        for (int i = 0; i < Reactions.Length; i++)
        {
            if (effectiveness == Reactions[i].Effectiveness)
                return Reactions[i];
        }

        return null;
    }

    public ReactionMultiplierPairs GetReactionMultiplier(AbilityReactionEffectiveness effectiveness)
    {
        for (int i = 0; i < ReactionMultipliers.Length; i++)
        {
            if (effectiveness == ReactionMultipliers[i].Effectiveness)
                return ReactionMultipliers[i];
        }

        return null;
    }
}

[System.Serializable]
public enum AbilityReactionEffectiveness
{
    SuperEffective,
    NotVeryEffective,
    Failure,
    Critical,
    DefenseSuccessful,
    DefenseFailed,
    Normal
}

[System.Serializable]
public class ReactionTextPairs
{
    public AbilityReactionEffectiveness Effectiveness;
    [TextArea]
    public string[] Reactions;

    public string GetRandomReaction()
    {
        return Reactions.GetRandom(out _);
    }
}

[System.Serializable]
public class ReactionMultiplierPairs
{
    public AbilityReactionEffectiveness Effectiveness;
    public float[] Multiplier;

    public float GetMultiplier(int consecutiveUsage)
    {
        consecutiveUsage = Mathf.Min(Multiplier.Length - 1, consecutiveUsage);
        return Multiplier[consecutiveUsage];
    }
}

public enum DamageType
{
    Touch,
    Joke,
    Comic,
    Universal
}