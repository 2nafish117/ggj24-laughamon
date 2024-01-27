using System.Collections.Generic;
using UnityEngine;

public interface IAbilityExecutionHandler
{
    void OnBeforeAbilityExecuted(Ability ability);
    void OnAfterAbilityExecuted(Ability ability);
}

public class AbilityExecuter : MonoBehaviour
{
    public List<Ability> Abilities;

    public List<Ability> Spells;

    public List<Ability> AbilitiesUsedThisCombat;
    public List<Ability> SpellsUsedThisCombat => AbilitiesUsedThisCombat;

    public void Start()
    {
        CombatManager.Instance.OnCombatStarted += OnCombatStarted;
    }

    private void OnCombatStarted()
    {
        AbilitiesUsedThisCombat.Clear();
        SpellsUsedThisCombat.Clear();
    }

    public void ExecuteAbility(Ability ability, CharacterControllerLaugh source, CharacterControllerLaugh target, IAbilityExecutionHandler executionHandler)
    {
        int abilityIndex = Abilities.IndexOf(ability);
        ExecuteAbility(abilityIndex, source, target, executionHandler);
    }

    public void ExecuteAbility(int index, CharacterControllerLaugh source, CharacterControllerLaugh target, IAbilityExecutionHandler executionHandler)
    {
        var ability = Abilities[index];
        AbilitiesUsedThisCombat.Add(ability);
        ability.ExecuteAbility(source, target, executionHandler);
    }

    public void ExecuteSpell(Ability ability, CharacterControllerLaugh source, CharacterControllerLaugh target, IAbilityExecutionHandler executionHandler)
    {
        int abilityIndex = Spells.IndexOf(ability);
        ExecuteSpell(abilityIndex, source, target, executionHandler);
    }

    public void ExecuteSpell(int index, CharacterControllerLaugh source, CharacterControllerLaugh target, IAbilityExecutionHandler executionHandler)
    {
        var spell = Spells[index];
        SpellsUsedThisCombat.Add(spell);
        spell.ExecuteAbility(source, target, executionHandler);
    }

    public int GetConsecutiveCount(Ability ability)
    {
        int counter = 0;
        for (int i = AbilitiesUsedThisCombat.Count - 1; i >= 0; i--)
        {
            if (AbilitiesUsedThisCombat[i] == ability)
            {
                counter++;
                continue;
            }
            break;
        }

        return counter;
    }
}
