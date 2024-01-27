using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff", menuName = "Custom/Buff")]
public class Buff : ScriptableObject
{
    public DamageType damageType;
    public float value;
    public BuffType buffType;
    public int turnsLeft;
    public List<ReactionTextPairs> Reactions;

    public ReactionTextPairs GetReactionTextFor(AbilityReactionEffectiveness effectiveness)
    {
        for (int i = 0; i < Reactions.Count; i++)
        {
            if (effectiveness == Reactions[i].Effectiveness)
                return Reactions[i];
        }

        return null;
    }
}

public enum BuffType
{
   Offense,
   Defense
}
