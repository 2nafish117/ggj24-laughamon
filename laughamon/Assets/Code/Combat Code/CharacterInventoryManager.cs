using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventoryManager : MonoBehaviour
{
    public List<Ability> inventory;
    [Range(1, 10)]
    public int EquippedSlots = 4;
    public List<Ability> Equipped;

    public void Init(List<Ability> startingAbilities)
    {
        inventory.AddRange(startingAbilities);

    }

    public void EquipDefaults()
    {
        Equipped.Clear();

        for (int i = 0; i < EquippedSlots; i++)
        {
            if (i >= inventory.Count)
            {
                break;
            }

            Equipped.Add(inventory[i]);
        }
    }
}
