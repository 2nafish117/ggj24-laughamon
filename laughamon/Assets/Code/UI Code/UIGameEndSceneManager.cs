using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameEndSceneManager : MonoBehaviour
{
    public static UIGameEndSceneManager Instance;
    public AudioSource BGMPlayer;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        BGMPlayer.volume = 0;
        BGMPlayer.DOFade(1, 2f).SetEase(Ease.InSine);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Combat Demo");
    }
}
