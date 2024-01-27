using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityCombatEffects : ScriptableObject
{
    public abstract void ExecuteCustomEffect(CharacterControllerLaugh source, CharacterControllerLaugh target);
}