using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//NOTE: Fix issues with delay for full stops and question marks, etc, by adopting a more clear regex-based approach
/// <summary>
/// UI component that controls scrolling text functionality.
/// </summary>
public class TextBox : MonoBehaviour
{
    [SerializeField] public TMP_Text mainContent;

    [SerializeField] [Range(1, 120)] protected float scrollTextSpeed;
    [SerializeField] [Range(0, 5)] protected float fullStopPause;
    [SerializeField] [Range(1, 30)] protected float ellipsisSpeed;
    [SerializeField] [Range(0.2f, 3)] protected float scrollingVolumeFactor;
    [SerializeField] [Range(0.2f, 3)] protected float scrollingPitchFactor;
    [SerializeField] [Range(0f, 3f)] protected float initialdelay;

    [Header("Audio Babbling")]
    [SerializeField] List<AudioClip> audioClips;
    [SerializeField] AudioSource audioSource;
    [SerializeField] bool babbleOn = true;
    //[SerializeField] float delay;

    public string textString { get; private set; }
    public bool isScrollingText { get; private set; }
    public bool scrollTextFast { get; private set; }


    private Coroutine textScrollingCoroutine;
    private float currentScrollDelay; //the delay before printing the next character during text scroll

    public event System.Action onFinishedScrollingText;

    public virtual void DisplayText(string _textString)
    {
        Show();
        ClearText();
        textString = _textString;
        if (isScrollingText == false)
        {
            isScrollingText = true;
            scrollTextFast = false;
            textScrollingCoroutine = StartCoroutine(ScrollText());
        }
    }

    public virtual void DisplayTextNoScrolling(string _textString)
    {
        ClearText();
        mainContent.text = _textString;
    }

    public virtual void SpeedUpScrolling()
    {
        scrollTextFast = true;
    }

    public virtual void ClearText()
    {
        if (isScrollingText)
        {
            if (textScrollingCoroutine != null) StopCoroutine(textScrollingCoroutine);
        }
        isScrollingText = false;
        mainContent.text = "";
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator ScrollText()
    {
        yield return new WaitForSeconds(initialdelay);
        currentScrollDelay = 0f;

        mainContent.text = textString;
        mainContent.maxVisibleCharacters = 0;

        for (int i = 0; i < textString.Length; i++)
        {
            if(textString[i] == '<')
            {
                while (textString[i] != '>')
                {
                    i++;
                }
                continue;
            }

            if (scrollTextFast)
            {
                currentScrollDelay += 1f / (scrollTextSpeed * 2);
            }

            //parsing logic
            else if (textString[i] == '.')
            {
                //ellipsis check
                if (i > 0 && textString[i - 1] == '.' ||
                    i < textString.Length - 1 && textString[i + 1] == '.')
                {
                    currentScrollDelay += 1f / ellipsisSpeed;
                }
                else
                {
                    currentScrollDelay += fullStopPause;
                }
            }

            else currentScrollDelay += 1f / scrollTextSpeed;

            mainContent.maxVisibleCharacters++;

            if (currentScrollDelay >= Time.deltaTime)
            {
                float temp = currentScrollDelay;
                currentScrollDelay = 0f;

                //random sound selection
                if(babbleOn && audioSource.isPlaying == false)
                {
                    audioSource.clip = audioClips[Random.Range(0, audioClips.Count)];
                    audioSource.Play();
                }
                yield return new WaitForSeconds(temp);
            }
        }
        isScrollingText = false;
        onFinishedScrollingText?.Invoke();
    }

    private void OnDisable()
    {
        mainContent.text = "";
        isScrollingText = false;
        scrollTextFast = false;
    }

}
