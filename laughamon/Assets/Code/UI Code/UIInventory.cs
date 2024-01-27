using System;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour, IAbilityEquipHandler
{
    public CharacterInventoryManager InventoryManager { get; private set; }
    public List<UIAbilitySlot> AbilitySlots;

    public void Init(CharacterInventoryManager inventoryManager)
    {
        InventoryManager = inventoryManager;
        PopulateInventory();
    }

    public void PopulateInventory()
    {
        int ablitiyCount = InventoryManager.Inventory.Count;
        for (int i = 0; i < AbilitySlots.Count; i++)
        {
            if (i < ablitiyCount)
            {
                AbilitySlots[i].SetAbility(InventoryManager.Inventory[i], this);
                AbilitySlots[i].gameObject.SetActive(true);
            }
            else
            {
                AbilitySlots[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnAbilitySlotClicked(UIAbilitySlot slot)
    {
        if (InventoryManager.Equipped.Contains(slot.Ability))
        {
            InventoryManager.UnEquipAbility(slot.Ability);
            slot.SetEquippedState(false);
            return;
        }

        int equippedCount = InventoryManager.Equipped.Count;
        if (equippedCount >= InventoryManager.MaxEquippedSlots)
        {
            return;
        }

        InventoryManager.EquipAbility(slot.Ability);
        slot.SetEquippedState(true);
    }
}
