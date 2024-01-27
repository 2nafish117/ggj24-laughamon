using UnityEngine;

[CreateAssetMenu(fileName = "Ability DeBuff", menuName = "Custom/Ability DOT")]
public class AbilityDeBuff : ScriptableObject
{
    public string Name;
    [Range(1, 10)]
    public int Turns;
    [Range(0, 1)]
    public float Chance = 1f;
    public float LaughPoint;

    [Space]
    public ReactionMultiplierPairs[] Multiplier;

    public void Execute(CharacterControllerLaugh source,CharacterControllerLaugh target)
    {

    }
}

public class AbilityDOTEffectExecuter
{
    protected CharacterControllerLaugh source;
    protected CharacterControllerLaugh target;
    protected AbilityDeBuff abilityDOT;

    public int RemainingTurns { get; private set; }

    public bool IsActive => RemainingTurns > 0;

    public void InitDot(CharacterControllerLaugh source, CharacterControllerLaugh target, AbilityDeBuff abilityDOT)
    {
        this.source = source;
        this.target = target;
        this.abilityDOT = abilityDOT;

        RemainingTurns = abilityDOT.Turns;
    }

    public void TickDOT()
    {
        RemainingTurns--;
        abilityDOT.Execute(source, target);
    }
}
