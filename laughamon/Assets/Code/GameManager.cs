using DG.Tweening;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action<int> OnCurrentLevelChanged;
    public int LevelIndex { get; private set; }
    public int MaxLevel;

    private void Awake()
    {
        Instance = this;
    }

    public CharacterProfile[] EnemyProfiles;

    private void Start()
    {
        ShowMenu();
    }

    public void ShowMenu()
    {
        UIMainMenuManager.Instace.ShowMenu(true);
        CombatHUDManager.Instance.ShowCombat(false);
    }

    public void ShowCombat()
    {
        UIMainMenuManager.Instace.ShowMenu(false);
        CombatHUDManager.Instance.ShowCombat(true);
    }

    public void OnCombatFinished(bool playerWon)
    {
        if (playerWon)
        {
            LevelIndex++;
        }
        //else
        //{
        //    LevelIndex--;
        //}

        LevelIndex = Mathf.Min(MaxLevel, Mathf.Max(LevelIndex++, 0));

        OnCurrentLevelChanged(LevelIndex);
        ShowMenu();
    }

    public void StartCombatAtLevel(int levelIndex)
    {
        StartCombat(levelIndex);
    }

    public void StartCombat(int enemyIndex)
    {
        enemyIndex = enemyIndex % EnemyProfiles.Length;

        ShowCombat();
        CombatManager.Instance.SetUpCombat(EnemyProfiles[enemyIndex]);
    }
}
