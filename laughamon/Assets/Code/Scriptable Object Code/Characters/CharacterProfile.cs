using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Profile", menuName = "Custom/Character Profile")]
public class CharacterProfile : ScriptableObject
{
    public string Name;
    public GameObject Prefab;
    public float MaxHealth;
    public Sprite ProfilePicture;

    [Tooltip("Non listed abilities are treated as normal by default")]
    public ReactionAbilityPairs[] AbilityEffectiveness;

    [Header("Starting Inventory")]
    public List<Ability> Inventory;

    public AbilityReactionEffectiveness GetEffectiveness(Ability ability)
    {
        AbilityReactionEffectiveness effectiveness = AbilityReactionEffectiveness.Normal;
        for (int i = 0; i < AbilityEffectiveness.Length; i++)
        {
            if (AbilityEffectiveness[i].HasReaction(ability))
            {
                effectiveness = AbilityEffectiveness[i].Effectiveness;
                break;
            }
        }

        return effectiveness;
    }
}

[System.Serializable]
public class ReactionAbilityPairs
{
    public AbilityReactionEffectiveness Effectiveness;
    public Ability[] Reactions;

    public bool HasReaction(Ability ability)
    {
        for (int i = 0; i < Reactions.Length; i++)
        {
            if (ReferenceEquals(Reactions[i], ability))
                return true;
        }

        return false;
    }
}
