using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour, IAbilityEquipHandler
{
    public CharacterInventoryManager InventoryManager { get; private set; }
    public List<UIAbilitySlot> AbilitySlots;
    public List<UIAbilitySlot> EquippedAbilitiesSlots;

    public void Initialize(CharacterInventoryManager inventoryManager)
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
                bool isEquipped = InventoryManager.Equipped.Contains(InventoryManager.Inventory[i]);
                AbilitySlots[i].SetAbility(InventoryManager.Inventory[i], isEquipped, this);
                AbilitySlots[i].gameObject.SetActive(true);
            }
            else
            {
                AbilitySlots[i].gameObject.SetActive(false);
            }
        }

        SetEquippedAbilites(InventoryManager.Equipped);
    }

    public void OnAbilitySlotClicked(UIAbilitySlot slot)
    {


        if (InventoryManager.Equipped.Contains(slot.Ability))
        {
            if (InventoryManager.Equipped.Count == 1)
            {
                return;
            }

            InventoryManager.UnEquipAbility(slot.Ability);
            PopulateInventory();
            return;
        }

        int equippedCount = InventoryManager.Equipped.Count;
        if (equippedCount >= InventoryManager.MaxEquippedSlots)
        {
            return;
        }

        InventoryManager.EquipAbility(slot.Ability);
        PopulateInventory();
    }

    public void SetEquippedAbilites(List<Ability> abilities)
    {
        int ablitiyCount = abilities.Count;
        for (int i = 0; i < EquippedAbilitiesSlots.Count; i++)
        {
            if (i < ablitiyCount)
            {
                EquippedAbilitiesSlots[i].SetAbility(abilities[i], true, this);
                EquippedAbilitiesSlots[i].gameObject.SetActive(true);
            }
            else
            {
                EquippedAbilitiesSlots[i].gameObject.SetActive(false);
            }
        }
    }
}
