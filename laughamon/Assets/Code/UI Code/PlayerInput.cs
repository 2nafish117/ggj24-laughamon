using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Instance;

    public static event System.Action OnProgressPrompt;
    //public static event System.Action<int> OnPlayerActionButtonPress;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.anyKeyDown) OnProgressPrompt?.Invoke();
    }
}
