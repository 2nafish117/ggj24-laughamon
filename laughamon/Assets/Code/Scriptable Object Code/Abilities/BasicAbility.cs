using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Custom/Ability")]
public class BasicAbility : Ability
{
    public override void Execute()
    {
        target.AnimateLaugh();
        DOVirtual.DelayedCall(3, EndAbilityExecution);
    }

}
