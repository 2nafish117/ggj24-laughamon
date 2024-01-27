using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerLaugh : MonoBehaviour, IAbilityExecutionHandler
{
    public string Name => CharacterProfile.Name;
    public AbilityExecuter ActionExecuter;
    public LaughterPoints LaughterPoints;
    public CharacterProfile CharacterProfile;
    public CharacterInventoryManager CharacterInventoryManager;
    public AbilityEffectHandler EffectHandler;

    protected IAbilityExecutionHandler executionHandler;

    public override string ToString()
    {
        return Name;
    }

    private void Start()
    {
        LaughterPoints.Init(CharacterProfile.MaxHealth);

        CombatManager.Instance.OnTurnChanged += HandleTurnChanged;
        LaughterPoints.OnLaughPointsChanged += HandleLaughterChanged;
        OnStart();
    }

    protected virtual void OnStart()
    {

    }

    protected virtual void HandleTurnChanged(bool isPlayerTurn)
    {

    }

    public void UseAbilityAtIndex(int index, IAbilityExecutionHandler executionHandler, CharacterControllerLaugh target)
    {
        this.executionHandler = executionHandler;
        ActionExecuter.ExecuteAbility(index, this, target, this);
    }

    public void UseSpellAtIndex(int index, IAbilityExecutionHandler executionHandler, CharacterControllerLaugh target)
    {
        this.executionHandler = executionHandler;
        ActionExecuter.ExecuteSpell(index, this, target, this);
    }

    public virtual void OnBeforeAbilityExecuted(Ability ability)
    {
        executionHandler?.OnBeforeAbilityExecuted(ability);
    }

    public virtual void OnAfterAbilityExecuted(Ability ability)
    {
        executionHandler?.OnAfterAbilityExecuted(ability);
    }

    public void AnimateLaugh()
    {
        transform.DOPunchScale(Vector3.one * 0.2f, 0.4f, 1).SetEase(Ease.OutBounce);
    }

    public void HandleLaughterChanged(float currentPoints, float changed)
    {
        if (LaughterPoints.IsDead)
        {
            LaughterPoints.OnLaughPointsChanged -= HandleLaughterChanged;
            OnDead();
        }
    }

    public void OnDead()
    {
        CombatManager.Instance.EndCombat();
    }
}
