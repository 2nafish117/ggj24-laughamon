using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityEffectHandler : MonoBehaviour
{
    public LaughTaleCharacterController CharacterController;

    public List<AbilityDOTEffectExecuter> DeBuffs;

    public void AddDeBuff(CharacterController source, CharacterController target, AbilityDOT deBuff)
    {
        var deBuffExecuter = new AbilityDOTEffectExecuter();
        deBuffExecuter.InitDot(source, target, deBuff);
        DeBuffs.Add(deBuffExecuter);
    }

    public void TickAbility()
    {
        foreach (var deBuff in DeBuffs)
        {
            deBuff.TickDOT();
        }

        for (var i = 0; i < DeBuffs.Count; i++)
        {

        }
    }
}
