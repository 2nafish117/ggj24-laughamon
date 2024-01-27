using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public CharacterProfile[] EnemyProfiles;

    private void Start()
    {
        DOVirtual.DelayedCall(2, StartGame);
    }
    public void StartGame()
    {
        StartCombat(0);
    }

    public void StartCombat(int enemyIndex)
    {
        enemyIndex = enemyIndex % EnemyProfiles.Length;
        CombatManager.Instance.SetUpCombat(EnemyProfiles[enemyIndex]);
    }

}
