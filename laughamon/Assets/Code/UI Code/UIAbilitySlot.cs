using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IAbilityEquipHandler
{
    public void OnAbilitySlotClicked(UIAbilitySlot slot);
}

public class UIAbilitySlot : MonoBehaviour
{
    public Color UnEquippedColor = Color.white;
    public Color EquippedColor = Color.blue;

    public Image Bg;
    public Image AbilityIcon;
    public IAbilityEquipHandler equipHandler;

    public Ability Ability { get; set; }

    public void SetAbility(Ability ability, IAbilityEquipHandler equipHandler)
    {
        Ability = ability;
        this.equipHandler = equipHandler;
        AbilityIcon.sprite = ability.AbilityIcon;
    }

    public void OnButtonClicked()
    {
        equipHandler?.OnAbilitySlotClicked(this);
    }

    public void SetEquippedState(bool isEquipped)
    {
        Bg.color = isEquipped ? EquippedColor : UnEquippedColor;
    }

}
