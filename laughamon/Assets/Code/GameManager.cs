using DG.Tweening;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action<int> OnCurrentLevelChanged;
    public int LevelIndex { get; private set; }
    public int MaxLevel;
    public int StartingLevelsUnlocked = 0;

    private void Awake()
    {
        Instance = this;
        LevelIndex = StartingLevelsUnlocked;
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
    }

    public void StartCombatAtLevel(int levelIndex,string mapName)
    {
        AdditiveSceneLoader.Instance.LoadScene(mapName);
        StartCombat(levelIndex);
    }

    public void StartCombat(int enemyIndex)
    {
        enemyIndex = enemyIndex % EnemyProfiles.Length;

        ShowCombat();
        CombatManager.Instance.SetUpCombat(EnemyProfiles[enemyIndex]);
    }
}
