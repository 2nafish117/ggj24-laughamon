using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class UILevelButton : MonoBehaviour
{
    public int LevelIndex;

    [SerializeField]
    private Transform activeState;

    [SerializeField]
    private Transform inActiveState;

    [SerializeField]
    private Transform defeatedState;

    [SerializeField]
    private GameObject buttonText;

    public bool IsInteractable;

    [SerializeField]
    CharacterAnimationController activeAnimator;
    [SerializeField]
    CharacterAnimationController deadAnimator;

    private enum State
    {
        Locked,
        Unlocked,
        Defeated
    }

    public void Start()
    {
        GameManager.Instance.OnCurrentLevelChanged += HandleLevelUnlocked;
        Initialize();
    }

    private void HandleLevelUnlocked(int index)
    {
        SetState(index);
    }

    public void Initialize()
    {
        SetState(GameManager.Instance.LevelIndex);
    }

    private void SetState(int index)
    {
        State state = index == LevelIndex ? State.Unlocked : index > LevelIndex ? State.Defeated : State.Locked;
        IsInteractable = state == State.Unlocked;
        activeState.gameObject.SetActive(state == State.Unlocked);
        buttonText.gameObject.SetActive(state == State.Unlocked);

        inActiveState.gameObject.SetActive(state == State.Locked);

        defeatedState.gameObject.SetActive(state == State.Defeated);

        if (state == State.Unlocked)
        {
            activeAnimator.LoopDance();
        }
        else
        {
            activeAnimator.StopDance();
        }

        if (state == State.Defeated)
        {
            deadAnimator.PlayAnimation(AnimationKey.Death1);
        }
    }

    public void OnButtonClicked()
    {
        if (IsInteractable == false)
        {
            return;
        }

        GameManager.Instance.StartCombatAtLevel(LevelIndex);
    }
}
