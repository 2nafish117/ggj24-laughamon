using DG.Tweening;
using UnityEngine;


[CreateAssetMenu(fileName = "Spell", menuName = "Custom/Spell")]
public class SpellAbility : Ability
{
    public override void Execute()
    {
        Debug.Log("Used a spell");
        target.AnimateLaugh();
        DOVirtual.DelayedCall(1, EndAbilityExecution);
    }
}
