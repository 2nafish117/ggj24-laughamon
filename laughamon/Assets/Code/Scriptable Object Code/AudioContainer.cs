using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/AudioContainer")]
public class AudioContainer : ScriptableObject
{
	[SerializeField] private AudioClip battleMusic1;
	[SerializeField] private AudioClip battleMusic2;
	[SerializeField] private AudioClip evilLaugh;
	[SerializeField] private AudioClip giggle;
	[SerializeField] private AudioClip hotSauceScreech;
	[SerializeField] private AudioClip kabaali;
	[SerializeField] private AudioClip laugh1;
	[SerializeField] private AudioClip laugh2;
	[SerializeField] private AudioClip laugh3;
	[SerializeField] private AudioClip mainMenu;

	//public void PlayBattleMusic1()
	//{
	//	//AudioManager.Instance.PlayBGM()
	//}

	//public void PlayBattleMusic2()
	//{
	//	//AudioManager.Instance.PlayAudioClip(powerUpFx);
	//}

	public void PlayEvilLaugh()
	{
		AudioManager.Instance.PlayAudioClip(evilLaugh, false);
	}

	public void PlayGiggle()
	{
		AudioManager.Instance.PlayAudioClip(giggle, false);
	}

	public void PlayHotSauceScreech()
	{
		AudioManager.Instance.PlayAudioClip(hotSauceScreech, false);
	}

	public void PlayKabaali()
	{
		AudioManager.Instance.PlayAudioClip(kabaali, false);
	}

	public void PlayLaugh1()
	{
		AudioManager.Instance.PlayAudioClip(laugh1, false);
	}

	public void PlayLaugh2()
	{
		AudioManager.Instance.PlayAudioClip(laugh2, false);
	}

	//public void PlayMainMenu()
	//{
	//	//AudioManager.Instance.PlayAudioClip(laugh1, false);
	//}
}
