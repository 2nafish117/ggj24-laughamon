using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    private GameObject textParentLabel;

    [SerializeField]
    private Image image; 
    public void Initialize(Ability ability,int index,IAbilityClickHandler clickHandler)
    {
        Ability = ability;
        Index = index;
        ClickHandler = clickHandler;

        image.sprite = ability.AbilityIcon;
        abilityNameLabel.SetText(ability.name);
        textParentLabel.SetActive(false);
    }

    public void OnClicked()
    {
        ClickHandler?.OnAbilityClicked(this);
    }

    public void OnHoverGained()
    {
        textParentLabel.SetActive(true);
    }

    public void OnHoverLost()
    {
        textParentLabel.SetActive(false);
    }
}
