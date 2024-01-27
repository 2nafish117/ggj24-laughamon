using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationKey
{
	// looping anims
    IdleLoop = 0,
	DefenseLoop,

	// one shot anims
	Heal,
	TakeHotSauceDamage,
	ListenMusic,
	DumbDance,
	Death,
	IdleTwitch,
	TakeDamage,
	Attack,
	Break4thWall,
	Tickle,

}

public class CharacterAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    //Set the default state in this function
    public void Init()
    {

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
			case AnimationKey.Heal:
				animator.SetTrigger("heal");
				break;
			case AnimationKey.TakeHotSauceDamage:
				animator.SetTrigger("hot_sauce_damage");
				break;
			case AnimationKey.ListenMusic:
				animator.SetTrigger("listen_music");
				break;
			case AnimationKey.DumbDance:
				animator.SetTrigger("dumb_dance");
				break;
			case AnimationKey.Death:
				animator.SetTrigger("death");
				break;
			case AnimationKey.IdleTwitch:
				animator.SetTrigger("idle_twitch");
				break;
			case AnimationKey.TakeDamage:
				animator.SetTrigger("damage");
				break;
			case AnimationKey.Attack:
				animator.SetTrigger("attack");
				break;
			case AnimationKey.Break4thWall:
				animator.SetTrigger("break_4th_wall");
				break;
			case AnimationKey.Tickle:
				animator.SetTrigger("tickle");
				break;
			default:
				Debug.Assert(false, "BRUH");
				break;
		}
    }

	#region debug
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
}
