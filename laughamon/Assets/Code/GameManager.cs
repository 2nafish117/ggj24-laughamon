using DG.Tweening;
using System;
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
        DOVirtual.DelayedCall(0.2f, StartGame);
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

    internal void ShowLevelScreen(bool playerWon)
    {
        throw new NotImplementedException();
    }
}
