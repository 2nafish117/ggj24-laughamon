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

    private void Start()
    {
        forestScene.LoadScene(combatContainer);
    }

    public void UnloadAll()
    {
        desertScene.UnLoadScene();
        lavaScene.UnLoadScene();
        forestScene.UnLoadScene();
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
        SceneManager.UnloadSceneAsync(scene);
    }
}
