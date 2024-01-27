using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Custom/Ability")]
public class BasicAbility : Ability
{
    public override void ExecuteSucceeded()
    {
        Announcer.Instance.Say($"{source.name} used an ability on {target.name}", 2f);
        target.AnimateLaugh();
        DOVirtual.DelayedCall(3, EndAbilityExecution);
    }

    public override void ExecuteFailed()
    {
    }
}
