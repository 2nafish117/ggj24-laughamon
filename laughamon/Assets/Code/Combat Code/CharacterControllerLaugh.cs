using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;

public class CharacterControllerLaugh : MonoBehaviour, IAbilityExecutionHandler
{
    public string Name => CharacterProfile.Name;
    public AbilityExecuter ActionExecuter;
    public LaughterPoints LaughterPoints;
    public CharacterProfile CharacterProfile;
    public CharacterInventoryManager InventoryManager;
    public AbilityEffectHandler EffectHandler;
    public CharacterAnimationController AnimationController;


    protected IAbilityExecutionHandler executionHandler;

    protected Queue<Ability> AbilityQueue = new Queue<Ability>();
    protected List<Buff> Buffs = new List<Buff>();

    public override string ToString()
    {
        return Name;
    }

    private void Start()
    {
        CombatManager.Instance.OnCombatStarted += OnStart;
        CombatManager.Instance.OnTurnChanged += HandleTurnChanged;
        LaughterPoints.OnLaughPointsChanged += HandleLaughterChanged;
    }

    protected virtual void OnStart()
    {
        Init(CharacterProfile);
    }

    public virtual void Init(CharacterProfile profile)
    {
        CharacterProfile = profile;
        AnimationController = SpawnCharacter(profile.Prefab);
        LaughterPoints.Init(CharacterProfile.MaxHealth);
        InventoryManager.Init(CharacterProfile.Inventory);
        InventoryManager.EquipDefaults();
        ActionExecuter.Init(InventoryManager);
        EffectHandler.Init(this);
        AnimationController.Init();
    }

    protected virtual CharacterAnimationController SpawnCharacter(GameObject characterPrefab)
    {
        if (AnimationController != null)
        {
            Destroy(AnimationController.gameObject);
        }
        return Instantiate(characterPrefab, transform).GetComponentInChildren<CharacterAnimationController>();
    }

    protected virtual void HandleTurnChanged(bool isPlayerTurn)
    {

    }
    private void ClearBuffs()
    {
        List<Buff> remainingBuffs = new List<Buff>();
        foreach (Buff buff in Buffs)
        {
            buff.turnsLeft--;
            if (buff.turnsLeft > 0) remainingBuffs.Add(buff);
        }
        Buffs = remainingBuffs;
    }

    public void UseAbilityAtIndex(int index, IAbilityExecutionHandler executionHandler, CharacterControllerLaugh target)
    {
        this.executionHandler = executionHandler;

        if (AbilityQueue.Count == 0)
        {
            ActionExecuter.ExecuteAbility(index, this, target, this);
        }

        else
        {
            ActionExecuter.ExecuteAbility(AbilityQueue.Dequeue(), this, target, this);
        }
    }

    public void UseSpellAtIndex(int index, IAbilityExecutionHandler executionHandler, CharacterControllerLaugh target)
    {
        this.executionHandler = executionHandler;
        ActionExecuter.ExecuteSpell(index, this, target, this);
    }

    public virtual void OnBeforeAbilityExecuted(Ability ability)
    {
        ClearBuffs();
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

    public void AnimateUnLaugh()
    {
        transform.DOPunchRotation(Vector3.one * 0.2f, 0.4f, 1).SetEase(Ease.OutBounce);
    }

    public void AnimateROFL()
    {
        AnimationController.PlayAnimation(AnimationKey.Death1);
    }

    public void HandleLaughterChanged(float currentPoints, float changed)
    {
        if (LaughterPoints.IsDead)
        {
            LaughterPoints.OnLaughPointsChanged -= HandleLaughterChanged;
            OnDead();
            return;
        }

        //if (changed < 0)
        //{
        //    AnimateLaugh();
        //}
        //else
        //{
        //    AnimateUnLaugh();
        //}
    }

    public void OnDead()
    {
        AnimateROFL();
        CombatManager.Instance.EndCombat();
    }

    public void QueueAbility(Ability ability)
    {
        AbilityQueue.Enqueue(ability);
    }

    public Buff GetDefensiveBuff()
    {
        if (Buffs.Count == 0) return null;
        else return Buffs[0]; //TODO search for defensive buff.
    }

    public void AddBuff(Buff buff)
    {
        Buffs.Add(buff);
    }
}
