using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameEndSceneManager : MonoBehaviour
{
    public static UIGameEndSceneManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Combat Demo");
    }
}
