using TMPro;
using UnityEngine;

public interface IAbilityClickHandler
{
    public void OnAbilityClicked(UIAbilityActionsButtons abilityButton);
}

public class UIAbilityActionsButtons : MonoBehaviour
{
    public Ability Ability;
    public int Index;
    public IAbilityClickHandler ClickHandler;

    [SerializeField]
    private TextMeshProUGUI abilityNameLabel;
    public void Initialize(Ability ability,int index,IAbilityClickHandler clickHandler)
    {
        Ability = ability;
        Index = index;
        ClickHandler = clickHandler;

        abilityNameLabel.SetText(ability.name);
    }

    public void OnClicked()
    {
        ClickHandler?.OnAbilityClicked(this);
    }
}
