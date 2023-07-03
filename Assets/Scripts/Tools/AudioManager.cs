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

    public void SetBgmVolume(float volume)
    {
        audioSource.volume = volume;
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


    public void SetEffectVolume(float volume)
    {
        effectAudioSource.volume = volume;
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
        effectAudioSource.clip = numberRotateEffect;
        effectAudioSource.loop = true;
        effectAudioSource.Play();
    }

    public void StopNumberRotateEffect() {
        effectAudioSource.Stop();
    }

    public void PlayReachClickEffect() {
        effectAudioSource.clip = reachClickEffect;
        effectAudioSource.Play();
    }
}
