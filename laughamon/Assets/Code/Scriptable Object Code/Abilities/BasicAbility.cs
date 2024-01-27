using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Custom/Ability")]
public class BasicAbility : Ability
{
    [Header("Animation Delays")]
    public float ExecutionTime = 3f;

    public override void ExecuteSucceeded()
    {
        Announcer.Instance.Say($"{source.name} used an ability on {target.name}", 2f);
        StartAbilityExecution();
        DOVirtual.DelayedCall(2.5f, ApplyLaugh);
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
        CombatLogger.Instance.AddLog($"{source} used {name} on {target}." +
            $"\n{GetReactionTextFor(effectiveness).GetRandomReaction().Replace("[target]", $"{target}")}" +
            $"\n{target} laughed for {LaughPoint * multiplier}");


CombatLogger.Instance.AddLog(UsageText);
        CombatLogger.Instance.AddLog(GetReactionTextFor(effectiveness).GetRandomReaction());

        target.LaughterPoints.Laugh(LaughPoint * multiplier);
        DOVirtual.DelayedCall(ExecutionTime, EndAbilityExecution);
    }

    public override void ExecuteFailed()
    {
        StartAbilityExecution();
        DOVirtual.DelayedCall(ExecutionTime, EndAbilityExecution);
    }
}
