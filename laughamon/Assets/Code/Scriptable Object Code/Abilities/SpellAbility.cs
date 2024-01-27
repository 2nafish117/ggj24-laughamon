using DG.Tweening;
using UnityEngine;


[CreateAssetMenu(fileName = "Spell", menuName = "Custom/Spell")]
public class SpellAbility : Ability
{
    public override void ExecuteSucceeded()
    {
        //Announcer.Instance.Say($"{source.name} casted a spell in {target.name}",2f);
        //target.AnimateLaugh();
        DOVirtual.DelayedCall(3, EndAbilityExecution);
    }

    public override void ExecuteFailed()
    {

    }
}
