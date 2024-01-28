using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenuManager : MonoBehaviour
{
    public static UIMainMenuManager Instace { get; private set; }

    [SerializeField]
    private GameObject menuContainer;

    private void Awake()
    {
        Instace = this;
    }

    public void ShowMenu(bool show)
    {
        menuContainer.SetActive(show);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#endif
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("Desert_EndScene");
    }
}
