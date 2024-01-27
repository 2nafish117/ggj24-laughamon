using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAbilityPanel : MonoBehaviour, IAbilityClickHandler
{
    public List<UIAbilityActionsButtons> actions;
    public CharacterInventoryManager InventoryManager;

    public void Initialize(CharacterInventoryManager inventoryManager)
    {
        InventoryManager = inventoryManager;
        inventoryManager.OnAbilityChanged -= UpdateUI;
        inventoryManager.OnAbilityChanged += UpdateUI;

        UpdateUI(null, false);
    }

    public void OnAbilityClicked(UIAbilityActionsButtons abilityButton)
    {
        CombatHUDManager.Instance.OnAbilityUsed(abilityButton.Index);
    }

    public void UpdateUI(Ability _, bool __)
    {
        int equippedCount = InventoryManager.Equipped.Count;
        for (int i = 0; i < actions.Count; i++)
        {
            if (i < equippedCount)
            {
                actions[i].Initialize(InventoryManager.Equipped[i], i, this);
                actions[i].gameObject.SetActive(true);
            }
            else
            {
                actions[i].gameObject.SetActive(false);
            }
        }
    }
}
