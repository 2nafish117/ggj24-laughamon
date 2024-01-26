using System;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public string Name;
    public bool IsSelfTargeting;
    public ReactionTextPairs[] Reactions;

    [Tooltip("Reaction multipler are only used if listed otherwise the default value will be zero")]
    public ReactionMultiplierPairs[] ReactionMultipliers;

    protected IAbilityExecutionHandler executionHandler;
    protected LaughTaleCharacterController source;
    protected LaughTaleCharacterController target;

    public void ExecuteAbility(LaughTaleCharacterController source, LaughTaleCharacterController target, IAbilityExecutionHandler executionHandler)
    {
        this.executionHandler = executionHandler;
        this.source = source;
        this.target = target;
        Execute();
    }

    public abstract void Execute();

    public virtual void StartAbilityExecution()
    {
        executionHandler.OnBeforeAbilityExecuted(this);
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
}

[System.Serializable]
public class ReactionTextPairs
{
    public AbilityReactionEffectiveness Effectiveness;
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
        consecutiveUsage = Mathf.Min(Multiplier.Length, consecutiveUsage);
        return Multiplier[consecutiveUsage];
    }
}