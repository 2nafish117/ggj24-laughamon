using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventoryManager : MonoBehaviour
{
    public List<Ability> Inventory;
    [Range(1, 10)]
    public int MaxEquippedSlots = 4;
    public List<Ability> Equipped;
    public event Action<Ability, bool> OnAbilityChanged;

    public void Init(List<Ability> startingAbilities, bool clear = false)
    {
        if (clear)
        {
            Inventory.Clear();
            Equipped.Clear();
        }

        Inventory.AddRange(startingAbilities);
    }

    public void EquipAbility(Ability ability)
    {
        Equipped.Add(ability);
        OnAbilityChanged?.Invoke(ability, true);
    }

    public void UnEquipAbility(Ability ability)
    {
        Equipped.Remove(ability);
        OnAbilityChanged?.Invoke(ability, false);
    }

    public void EquipDefaults()
    {
        Equipped.Clear();

        for (int i = 0; i < MaxEquippedSlots; i++)
        {
            if (i >= Inventory.Count)
            {
                break;
            }

            Equipped.Add(Inventory[i]);
        }
    }
}
