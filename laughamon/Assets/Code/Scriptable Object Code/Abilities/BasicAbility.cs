using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Custom/Ability")]
public class BasicAbility : Ability
{
    [Header("Animation Delays")]
    public float ExecutionTime = 3f;

    public override void ExecuteSucceeded()
    {
        //Announcer.Instance.Say($"{source.name} used an ability on {target.name}", 2f);
        StartAbilityExecution();
        ApplyLaugh();
        //DOVirtual.DelayedCall(2.5f, ApplyLaugh);
    }

    private void ApplyLaugh()
    {
        int consecutiveMuliplier = source.ActionExecuter.GetConsecutiveCount(this);
        var effectiveness = target.CharacterProfile.GetEffectiveness(this);
        ReactionMultiplierPairs reactionMultiplier = GetReactionMultiplier(effectiveness);
        float multiplier = 1; ;
        if (reactionMultiplier == null)
        {
            multiplier = 0;
        }
        else
        {
            multiplier = reactionMultiplier.GetMultiplier(consecutiveMuliplier);
        }

        CombatLogger.Instance.AddLog(UsageText);
        ReactionTextPairs reaction = GetReactionTextFor(effectiveness);
        if (reaction != null)
        {
            CombatLogger.Instance.AddLog(reaction.GetRandomReaction());
        }

        target.LaughterPoints.Laugh(LaughPoint * multiplier);
        DOVirtual.DelayedCall(ExecutionTime, EndAbilityExecution);
    }

    public override void ExecuteFailed()
    {
        StartAbilityExecution();
        DOVirtual.DelayedCall(ExecutionTime, EndAbilityExecution);
    }
}
