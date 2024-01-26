using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Announcer : MonoBehaviour
{
    public static Announcer Instance { get; private set; }

    [SerializeField]
    private TextMeshProUGUI messageText;

    [SerializeField]
    private RectTransform messageContainer;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Announce a message for duration in seconds.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="duration">Secs the message should be on screen. Negative number to stay forever</param>
    public void Say(string message, float duration)
    {
        messageText.SetText(message);

        messageContainer.DOComplete();

        messageContainer.gameObject.SetActive(true);
        messageContainer.DOScale(Vector3.one, 0.3f).SetEase(Ease.InBounce);
        HideAfter(duration);

    }

    private void HideAfter(float duration)
    {
        DOVirtual.DelayedCall(duration, HideAnim);
    }

    private void HideAnim()
    {
        messageContainer.DOScale(Vector3.zero, 0.3f).SetEase(Ease.OutExpo);
    }

}
