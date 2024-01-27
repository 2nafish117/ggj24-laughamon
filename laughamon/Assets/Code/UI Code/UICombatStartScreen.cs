using TMPro;
using UnityEngine;

public class UICombatStartScreen : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI headingLabel;

    [SerializeField]
    private Camera enemyCamera;

    public void Init(string player, string enemy)
    {
        headingLabel.SetText($"{player} VS {enemy}");
        enemyCamera.gameObject.SetActive(true);

        gameObject.SetActive(true);
    }

    public void OnReadyClicked()
    {
        enemyCamera.gameObject.SetActive(false);
        gameObject.SetActive(false);
        CombatManager.Instance.StartCombat();
    }
}
