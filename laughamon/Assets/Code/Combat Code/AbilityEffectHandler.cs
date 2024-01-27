using System.Collections.Generic;
using UnityEngine;

public class AbilityEffectHandler : MonoBehaviour
{
    public CharacterControllerLaugh CharacterController;

    public List<AbilityDOTEffectExecuter> DeBuffs;

    public void AddDeBuff(CharacterControllerLaugh source, CharacterControllerLaugh target, AbilityDOT deBuff)
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

        int len = DeBuffs.Count;
        for (var i = 0; i < len; i++)
        {
            if (DeBuffs[i].IsActive == false)
            {
                i--;
                len--;
                DeBuffs.RemoveAtSwapBack(i);
            }
        }
    }
}
