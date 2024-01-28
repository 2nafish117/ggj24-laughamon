using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Vfx/VfxContainer")]
public class VfxContainer : ScriptableObject
{
	[SerializeField] private GameObject laughFx;
	[SerializeField] private GameObject powerUpFx;
	[SerializeField] private GameObject powerDownFx;
	[SerializeField] private GameObject fireFx;

	public void PlayLaugh()
	{
		CombatManager.Instance.SpawnVFX(laughFx);
	}

	public void PlayPowerUp()
	{
		CombatManager.Instance.SpawnVFX(powerUpFx);
	}

	public void PlayPowerDown()
	{
		CombatManager.Instance.SpawnVFX(powerDownFx);
	}

	public void PlayFire()
	{
		CombatManager.Instance.SpawnVFX(fireFx);
	}
}
