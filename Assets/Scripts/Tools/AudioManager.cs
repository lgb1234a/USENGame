using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviourSingletonTemplate<AudioManager>
{
    public AudioSource audioSource;
    public AudioClip bgm;
    public AudioClip willReachBgm;


    public AudioSource effectAudioSource;

    public AudioClip bingoEffect;
    public AudioClip numberCheckEffect;
    public AudioClip numberRotateEffect;
    public AudioClip reachClickEffect;

    public AudioSource keydownAudioSource;
    public AudioSource numberRotateAudioSource;

    public static void InitVolume() 
    {
        var bgmVolume = AppConfig.Instance.BGMVolume;
        AudioManager.Instance.SetBgmVolume(bgmVolume);
        var effectVolume = AppConfig.Instance.EffectVolume;
        AudioManager.Instance.SetEffectVolume(effectVolume);
    }

    public void SetBgmVolume(int volume)
    {
        audioSource.volume = volume * 0.1f + 0.5f;
    }

    public void PlayWillReachBgm() {
        audioSource.clip = willReachBgm;
        PlayBgm();
    }

    public void PlayDefaultBgm() {
        audioSource.clip = bgm;
        PlayBgm();
    }

    public void StopBgm() {
        if (audioSource.isPlaying)
            audioSource.Stop();
    }

    public void PlayBgm() {
        if (!audioSource.isPlaying)
            audioSource.Play();
    }


    public void SetEffectVolume(int volume)
    {
        effectAudioSource.volume = volume * 0.1f + 0.5f;
    }

    public void PlayBingoEffect() {
        effectAudioSource.clip = bingoEffect;
        effectAudioSource.Play();
    }

    public void PlayNumberCheckEffect() {
        effectAudioSource.clip = numberCheckEffect;
        effectAudioSource.loop = false;
        effectAudioSource.Play();
    }

    public void PlayNumberRotateEffect() {
        numberRotateAudioSource.clip = numberRotateEffect;
        numberRotateAudioSource.loop = true;
        numberRotateAudioSource.Play();
    }

    public void PlayNumberRotateEffectWithoutLoop() {
        numberRotateAudioSource.clip = numberRotateEffect;
        numberRotateAudioSource.loop = false;
        numberRotateAudioSource.Play();
    }

    public void StopNumberRotateEffect() {
        numberRotateAudioSource.Stop();
    }

    public void PlayReachClickEffect() {
        effectAudioSource.clip = reachClickEffect;
        effectAudioSource.loop = false;
        effectAudioSource.Play();
    }

    public void PlayKeyDownEffect() {
        keydownAudioSource.Play();
    }
}
