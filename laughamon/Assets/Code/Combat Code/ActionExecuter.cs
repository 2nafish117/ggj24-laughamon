using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbilityExecutionHandler
{
    void OnBeforeAbilityExecuted(Ability ability);
    void OnAfterAbilityExecuted(Ability ability);
}

public class ActionExecuter : MonoBehaviour
{
    public List<Ability> Abilities;

    public List<Ability> Spells;

    public void ExecuteAbility(Ability ability, LaughTaleCharacterController source, LaughTaleCharacterController target, IAbilityExecutionHandler executionHandler)
    {
        int abilityIndex = Abilities.IndexOf(ability);
        ExecuteAbility(abilityIndex, source, target, executionHandler);
    }

    public void ExecuteAbility(int index, LaughTaleCharacterController source, LaughTaleCharacterController target, IAbilityExecutionHandler executionHandler)
    {
        var ability = Abilities[index];
        ability.ExecuteAbility(source, target, executionHandler);
    }

    public void ExecuteSpell(Ability ability, LaughTaleCharacterController source, LaughTaleCharacterController target, IAbilityExecutionHandler executionHandler)
    {
        int abilityIndex = Spells.IndexOf(ability);
        ExecuteSpell(abilityIndex, source, target, executionHandler);
    }

    public void ExecuteSpell(int index, LaughTaleCharacterController source, LaughTaleCharacterController target, IAbilityExecutionHandler executionHandler)
    {
        var ability = Spells[index];
        ability.ExecuteAbility(source, target, executionHandler);
    }
}
