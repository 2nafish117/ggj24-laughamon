using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Combat Effect", menuName = "Custom/Combat Effect")]

public class DemoAbilityEffect : AbilityCombatEffects
{
    public override void ExecuteCustomEffect(CharacterControllerLaugh source, CharacterControllerLaugh target)
    {
        Announcer.Instance.Say("Executing custom effect", 2f);
    }
}
