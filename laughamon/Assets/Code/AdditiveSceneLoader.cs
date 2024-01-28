using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveSceneLoader : MonoBehaviour
{
    public static AdditiveSceneLoader Instance;

    public Transform combatContainer;

    public SceneData desertScene;
    public SceneData lavaScene;
    public SceneData forestScene;

    private void Awake()
    {
        Instance = this;
    }

    public void UnloadAll()
    {
        desertScene.UnLoadScene();
        lavaScene.UnLoadScene();
        forestScene.UnLoadScene();
    }

    public void LoadScene(string mapName)
    {
        if (desertScene.scene.Equals(mapName))
        {
            desertScene.LoadScene(combatContainer);
            forestScene.UnLoadScene();
            lavaScene.UnLoadScene();
            return;
        }

        if (forestScene.scene.Equals(mapName))
        {
            forestScene.LoadScene(combatContainer);
            desertScene.UnLoadScene();
            lavaScene.UnLoadScene();
            return;
        }

        if (lavaScene.scene.Equals(mapName))
        {
            lavaScene.LoadScene(combatContainer);
            forestScene.UnLoadScene();
            desertScene.UnLoadScene();
            return;
        }
    }
}

[System.Serializable]
public class SceneData
{
    public Vector3 position;
    public Vector3 rotation;
    public string scene;

    public void LoadScene(Transform combatSetup)
    {
        combatSetup.SetPositionAndRotation(position, Quaternion.Euler(rotation));
        SceneManager.LoadScene(scene, LoadSceneMode.Additive);
    }

    public void UnLoadScene()
    {
        var allScenes = SceneManager.sceneCount;
        for (int i = 0; i < allScenes; i++)
        {
            if (SceneManager.GetSceneAt(i).name.Equals(scene))
            {
                SceneManager.UnloadSceneAsync(scene);
                return;
            }
        }
    }
}
