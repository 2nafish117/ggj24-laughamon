using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationIntTrigger : MonoBehaviour
{
	[SerializeField]
	private Animator m_Animator;

	[SerializeField]
	private string m_IntTrigger;

	[SerializeField]
	private int m_IntTriggerValue = 0;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			StartCoroutine(SetAnimatorIntTrigger(m_Animator, m_IntTrigger, m_IntTriggerValue));
		}
	}

	//[ContextMenu("Debug/TestAnimationIntTrigger")]
	//void TestAnimationIntTrigger()
	//{
	//	StartCoroutine(SetAnimatorIntTrigger(m_Animator, m_IntTrigger, m_IntTriggerValue));
	//}

	// use this to use int params on animaor as a trigger
	IEnumerator SetAnimatorIntTrigger(Animator animator, string name, int value)
	{
		animator.SetInteger(name, value);
		yield return null;
		//yield return new WaitForSeconds(0.1f);
		animator.SetInteger(name, 0);
	}
}
