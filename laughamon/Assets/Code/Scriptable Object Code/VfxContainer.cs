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

    public void PlayLaugh(Vector3 worldPos)
    {
        CombatManager.Instance.SpawnVFX(laughFx, worldPos);
    }

    public void PlayPowerUp(Vector3 worldPos)
    {
        CombatManager.Instance.SpawnVFX(powerUpFx, worldPos);
    }

    public void PlayPowerDown(Vector3 worldPos)
    {
        CombatManager.Instance.SpawnVFX(powerDownFx, worldPos);
    }

    public void PlayFire(Vector3 worldPos)
    {
        CombatManager.Instance.SpawnVFX(fireFx, worldPos);
    }
}
