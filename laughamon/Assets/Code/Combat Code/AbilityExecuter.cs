using System.Collections.Generic;
using UnityEngine;

public interface IAbilityExecutionHandler
{
    void OnBeforeAbilityExecuted(Ability ability);
    void OnAfterAbilityExecuted(Ability ability);
}

public class AbilityExecuter : MonoBehaviour
{
    public List<Ability> AbilitiesUsedThisCombat;
    public List<Ability> SpellsUsedThisCombat => AbilitiesUsedThisCombat;
    public CharacterInventoryManager InventoryManager;

    public void Start()
    {
        CombatManager.Instance.OnCombatStarted += OnCombatStarted;
    }

    public void Init(CharacterInventoryManager inventoryManager)
    {
        InventoryManager = inventoryManager;
        AbilitiesUsedThisCombat.Clear();
    }

    private void OnCombatStarted()
    {
        AbilitiesUsedThisCombat.Clear();
        SpellsUsedThisCombat.Clear();
    }

    public void ExecuteAbility(Ability ability, CharacterControllerLaugh source, CharacterControllerLaugh target, IAbilityExecutionHandler executionHandler)
    {
        int abilityIndex = InventoryManager.Equipped.IndexOf(ability);
        ExecuteAbility(abilityIndex, source, target, executionHandler);
    }

    public void ExecuteAbility(int index, CharacterControllerLaugh source, CharacterControllerLaugh target, IAbilityExecutionHandler executionHandler)
    {
        var ability = InventoryManager.Equipped[index];
        AbilitiesUsedThisCombat.Add(ability);
        ability.ExecuteAbility(source, target, executionHandler);
    }

    public void ExecuteSpell(Ability ability, CharacterControllerLaugh source, CharacterControllerLaugh target, IAbilityExecutionHandler executionHandler)
    {
        int abilityIndex = InventoryManager.Equipped.IndexOf(ability);
        ExecuteSpell(abilityIndex, source, target, executionHandler);
    }

    public void ExecuteSpell(int index, CharacterControllerLaugh source, CharacterControllerLaugh target, IAbilityExecutionHandler executionHandler)
    {
        var spell = InventoryManager.Equipped[index];
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
