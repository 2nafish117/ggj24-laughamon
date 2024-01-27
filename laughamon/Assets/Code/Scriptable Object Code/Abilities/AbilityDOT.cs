using UnityEngine;

[CreateAssetMenu(fileName = "AbilityDOT", menuName = "Custom/Ability DOT")]
public class AbilityDOT : ScriptableObject
{
    public string Name;
    [Range(1, 10)]
    public int Turns;
    [Range(0, 1)]
    public float Chance = 1f;
    public float LaughPoint;

    [Space]
    public ReactionMultiplierPairs[] Multiplier;
}

public class AbilityDOTEffectExecuter
{
    protected CharacterController source;
    protected CharacterController target;
    protected AbilityDOT abilityDOT;

    public int RemainingTurns { get; private set; }

    public bool IsActive => RemainingTurns > 0;

    public void InitDot(CharacterController source, CharacterController target, AbilityDOT abilityDOT)
    {
        this.source = source;
        this.target = target;
        this.abilityDOT = abilityDOT;

        RemainingTurns = abilityDOT.Turns;
    }

    public void TickDOT()
    {
        RemainingTurns--;
    }
}
