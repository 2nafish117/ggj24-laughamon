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
    public Buff[] BuffsList;

    [Space]
    [TextArea]
    public string UsageText;

    [Space]
    public ReactionTextPairs[] Reactions;

    [Tooltip("Reaction multipler are only used if listed otherwise the default value will be one")]
    public ReactionMultiplierPairs[] ReactionMultipliers;

    [Header("DOT's")]
    public AbilityDeBuff[] DOTs;

    [Header("Cusotm Behaviours")]
    public AbilityCombatEffects[] CombatEffects;

    protected IAbilityExecutionHandler executionHandler;
    protected CharacterControllerLaugh source;
    protected CharacterControllerLaugh target;

    [Header("Damage Modifiers")]
    public bool HasDamageModifier;
    public DamageModifiers DamageModifiers;

    [Header("Multi Hit")]
    [Range(0, 5)]
    public int MultiHits;

    [Header("Joke Ability")]
    public bool IsAJoke;
    //public JokeData JokeData;
    public JokeData[] JokeDataArray;
    public AnimationKey JokeReaction;
    public float JokeReactionDelay;

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

    public virtual void ApplyDOT()
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

    protected void TriggerTargetAnimation()
    {
        target.AnimationController.PlayAnimation(TargetAnimationKey);
    }

    protected void TriggerTargetJokeAnimation()
    {
        target.AnimationController.PlayAnimation(JokeReaction);
    }

    public virtual void ExecuteOther()
    {
        ExecuteCombatEffects();
        ApplyDOT();
    }

    public virtual void EndAbilityExecution()
    {
        if (source.MultiHits > 0)
        {
            source.MultiHits--;
            //
            if (CombatManager.Instance.IsPlayerTurn)
            {
                CombatManager.Instance.StartPlayerTurn();
            }
            else
            {
                CombatManager.Instance.EndPlayerTurn();
            }

            ClearReferences();
            return;
        }

        executionHandler.OnAfterAbilityExecuted(this);
        ClearReferences();
    }

    private void ClearReferences()
    {
        source = null;
        target = null;
        executionHandler = null;
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
public class DamageModifiers
{
    [Range(-1f, 1f)]
    public float DamageBlockMultiplier;
    [Range(0, 10)]
    public int DamageBlockInstance;
    [Range(-1f, 1f)]
    public float DamageReflectMultiplier;
    [Range(0, 10)]
    public int DamageReflectInstance;

    [HideInInspector]
    public Ability SourceAbility;

    public bool IsExpired => DamageBlockInstance <= 0 && DamageReflectInstance <= 0;

    public DamageModifiers() { }

    public DamageModifiers(Ability sourceAbility, float damageBlockMultiplier, int damageBlockInstance, float reflectionMultiplier, int reflectionInstances)
    {
        SourceAbility = sourceAbility;
        AddDamageReflectionBuff(reflectionMultiplier, reflectionInstances);
        AddDamageBlockBuff(damageBlockMultiplier, damageBlockInstance);
    }

    public void AddDamageReflectionBuff(float reflectionMultiplier, int instances)
    {
        DamageReflectMultiplier = reflectionMultiplier;
        DamageReflectInstance = instances;
    }

    public void AddDamageBlockBuff(float blockMultiplier, int instances)
    {
        DamageBlockMultiplier = blockMultiplier;
        DamageBlockInstance = instances;
    }

    public void ModifyDamageApplication(CharacterControllerLaugh source, CharacterControllerLaugh target, float damage)
    {
        var reflectedDamage = 0f;
        if (DamageReflectInstance > 0)
        {
            DamageBlockInstance--;
            reflectedDamage = damage * DamageReflectMultiplier;
        }

        var blockedDamage = 0f;
        if (DamageBlockInstance > 0)
        {
            DamageBlockInstance--;
            blockedDamage = damage * DamageBlockMultiplier;
        }


        target.LaughterPoints.Laugh(reflectedDamage);
        if (reflectedDamage > 0)
        {
            source.AnimationController.PlayHitReaction();
            //TODO : Add reflected damage combat log;
        }
        else
        {
            source.AnimationController.PlayHeal();
            //TODO : Add reflected heal combat log;
        }

        var reducedDamage = damage - blockedDamage;
        source.LaughterPoints.Laugh(reducedDamage);
        if (reducedDamage > 0)
        {
            target.AnimationController.PlayHitReaction();
            //TODO : Add reduced damage combat log;
        }
        else
        {
            target.AnimationController.PlayHeal();
            //TODO : Add amped damage combat log;
        }
    }
}

[System.Serializable]
public class JokeData
{
    public float AnswerTime = 4f;
    [TextArea]
    public string JokeStart;
    [TextArea]
    public string FullJoke;
    public string CorrectAnswer;
    public string[] WrongAnswers;
    [TextArea]
    public string CorrectReaction;
    [TextArea]
    public string WrongReaction;

    public bool IsCorrectAnswer(string answer) => CorrectAnswer.Equals(answer);
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