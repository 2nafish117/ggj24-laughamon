using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public enum AnimationKey
{
    // looping anims
    IdleLoop = 0,
    DefenseLoop,

    // one shot anims
    IdleTwitch1,
    Heal,
    ListenMusic,

    DumbDance1,
    DumbDance2,
    DumbDance3,

    Laughing1,
    Laughing2,

    Death1,
    Death2,

    TakeHotSauceDamage,
    TakeDamage1,
    TakeTickleDamage,

    Break4thWall1,
    Break4thWall2,

    Tickle1,
    Attack1,
    ActingPuppet,

    Taunt1,
    Taunt2,
    Taunt3,

    NumKeys,
    Meditate
}

public class CharacterAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [Header("Keys")]
    [SerializeField]
    private List<AnimationKey> danceKeys;
    [SerializeField]
    private List<AnimationKey> attackReaction;
    [SerializeField]
    private List<AnimationKey> healKeys;
    [SerializeField]
    private List<AnimationKey> laughKeys;
    [SerializeField]
    private List<AnimationKey> deathKeys;

    //Set the default state in this function
    public void Init()
    {
        PlayAnimation(AnimationKey.IdleLoop);
    }

    public void PlayAnimation(AnimationKey key)
    {
        switch (key)
        {
            case AnimationKey.IdleLoop:
                animator.SetTrigger("idle_loop");
                break;
            case AnimationKey.DefenseLoop:
                animator.SetTrigger("idle_defense");
                break;
            case AnimationKey.IdleTwitch1:
                animator.SetTrigger("idle_twitch1");
                break;
            case AnimationKey.Heal:
                animator.SetTrigger("heal");
                break;
            case AnimationKey.ListenMusic:
                animator.SetTrigger("listen_music");
                break;
            case AnimationKey.DumbDance1:
                animator.SetTrigger("dumb_dance1");
                break;
            case AnimationKey.DumbDance2:
                animator.SetTrigger("dumb_dance2");
                break;
            case AnimationKey.DumbDance3:
                animator.SetTrigger("dumb_dance3");
                break;
            case AnimationKey.Laughing1:
                animator.SetTrigger("laughing1");
                break;
            case AnimationKey.Laughing2:
                animator.SetTrigger("laughing2");
                break;
            case AnimationKey.Death1:
                animator.SetTrigger("death1");
                break;
            case AnimationKey.Death2:
                animator.SetTrigger("death2");
                break;
            case AnimationKey.TakeHotSauceDamage:
                animator.SetTrigger("hot_sauce_damage");
                break;
            case AnimationKey.TakeDamage1:
                animator.SetTrigger("damage1");
                break;
            //case AnimationKey.TakeDamage2:
            //	animator.SetTrigger("damage2");
            //	break;
            case AnimationKey.TakeTickleDamage:
                animator.SetTrigger("take_tickle_damage");
                break;

            case AnimationKey.Break4thWall1:
                animator.SetTrigger("break4thwall1");
                break;
            case AnimationKey.Break4thWall2:
                animator.SetTrigger("break4thwall2");
                break;
            case AnimationKey.Tickle1:
                animator.SetTrigger("tickle1");
                break;
            case AnimationKey.Attack1:
                animator.SetTrigger("attack1");
                break;
            case AnimationKey.ActingPuppet:
                animator.SetTrigger("acting_puppet");
                break;
            case AnimationKey.Taunt1:
                animator.SetTrigger("taunt1");
                break;
            case AnimationKey.Taunt2:
                animator.SetTrigger("taunt2");
                break;
            case AnimationKey.Taunt3:
                animator.SetTrigger("taunt3");
                break;
            case AnimationKey.Meditate:
                animator.SetTrigger("meditate");
                break;


            default:
                Debug.Assert(false, "BRUH");
                break;
        }
    }

    #region debug

    [Header("Testing")]
    [SerializeField]
    private GameObject characterPrefab;

    [SerializeField]
    private GameObject debugContainer;

    [ContextMenu("Debug/TestAnimKeys")]
    private void TestAnimKeys()
    {
        for (int i = 0; i < (int)AnimationKey.NumKeys; ++i)
        {
            var go = Instantiate(characterPrefab);
            go.transform.parent = debugContainer.transform;
            var controller = go.GetComponent<CharacterAnimationController>();
            controller.PlayAnimation((AnimationKey)i);
        }
    }

    [SerializeField]
    private string m_IntTrigger;

    [SerializeField]
    private int m_IntTriggerValue = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(SetAnimatorIntTrigger(animator, m_IntTrigger, m_IntTriggerValue));
        }
    }
    #endregion

    // use this to use int params on animaor as a trigger
    IEnumerator SetAnimatorIntTrigger(Animator animator, string name, int value)
    {
        animator.SetInteger(name, value);
        yield return null;
        animator.SetInteger(name, 0);
    }

    public void PlayDance()
    {
        if (danceKeys.Count == 0)
            return;

        PlayAnimation(danceKeys.GetRandom(out _));
    }

    public void PlayHeal()
    {
        if (healKeys.Count == 0)
            return;

        PlayAnimation(healKeys.GetRandom(out _));
    }

    public void PlayHitReaction()
    {
        if (attackReaction.Count == 0)
            return;

        PlayAnimation(attackReaction.GetRandom(out _));
    }

    public void PlayLaugh()
    {
        if (laughKeys.Count == 0)
            return;

        PlayAnimation(laughKeys.GetRandom(out _));
    }

    public void PlayDeath()
    {
        if (deathKeys.Count == 0)
            return;

        PlayAnimation(deathKeys.GetRandom(out _));
    }

    Coroutine danceRoutine;
    public void LoopDance()
    {
        if (gameObject.activeSelf == false || gameObject.activeInHierarchy == false)
        {
            return;
        }

        if (danceKeys.Count == 0)
        {
            PlayAnimation(AnimationKey.IdleLoop);
            return;
        }
        danceRoutine = StartCoroutine(LoopDanceKeys());
    }

    public void StopDance()
    {
        if (danceRoutine != null)
        {
            StopCoroutine(danceRoutine);
        }
    }

    IEnumerator LoopDanceKeys()
    {
        WaitForSeconds seconds = new WaitForSeconds(3);

        while (true)
        {
            var key = danceKeys.GetRandom(out _);
            PlayAnimation(key);
            yield return seconds;
        }
    }
}
