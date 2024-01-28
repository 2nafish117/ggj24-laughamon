using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public AudioSource AudioSourcePrefab;

    [SerializeField]
    private List<AudioSource> AudioSources;

    [SerializeField]
    private AudioSource BGMPlayer;

    [SerializeField]
    private AudioClip[] Tracks;

    public void PlayBGM(int trackIndex)
    {
        if (Tracks.Length == 0)
            return;

        BGMPlayer.DOFade(0, 2f).SetEase(Ease.OutSine).onComplete = () =>
        {
            trackIndex = trackIndex % Tracks.Length;
            BGMPlayer.clip = Tracks[trackIndex];
            BGMPlayer.loop = true;
            BGMPlayer.DOFade(1, 2f).SetEase(Ease.InSine);
        };
    }

    public AudioSource PlayAudioClip(AudioClip clip, bool loop = false)
    {
        var source = GetFreeSource();
        source.clip = clip;
        source.loop = loop;
        source.Play();
        source.gameObject.SetActive(true);
        return source;
    }

    AudioSource GetFreeSource()
    {
        foreach (var audioSource in AudioSources)
        {
            if (audioSource.isPlaying == false)
            {
                return audioSource;
            }
        }

        var newSource = Instantiate(AudioSourcePrefab, transform);
        AudioSources.Add(newSource);
        return newSource;
    }
}
