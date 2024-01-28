using System.Collections.Generic;
using UnityEngine;

public class UICombatResultScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject successResult;

    [SerializeField]
    private GameObject failureResult;

    private bool playerVictory;

    [SerializeField]
    private Camera deathCamera;

    public void SetResult(bool playerVictory)
    {
        this.playerVictory = playerVictory;
        successResult.SetActive(playerVictory);
        failureResult.SetActive(!playerVictory);
        deathCamera.gameObject.SetActive(true);

        if (playerVictory)
        {
            PlayerController.Instance.AnimationController.LoopDance();
        }
        else
        {
            AIController.Instance.AnimationController.LoopDance();
        }

        gameObject.SetActive(true);
    }

    public void OnMainMenuButtonClicked()
    {
        deathCamera.gameObject.SetActive(false);
        GameManager.Instance.ShowMenu();
    }
}
