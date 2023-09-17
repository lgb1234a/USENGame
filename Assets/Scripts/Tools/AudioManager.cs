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

    private bool fadeIn = false;
    private bool fadeOut = false;
    private bool isFaded = false;
    private float fadeCostTime = 0f;
    private float fadeInterval = 0f;
    private float currentVolume = 0f;

    public static void InitVolume() 
    {
        var bgmVolume = AppConfig.Instance.BGMVolume;
        AudioManager.Instance.SetBgmVolume(bgmVolume);
        var effectVolume = AppConfig.Instance.EffectVolume;
        AudioManager.Instance.SetEffectVolume(effectVolume);
    }

    void Update()
    {
        if (fadeIn)
        {
            fadeInterval += Time.deltaTime;
            if (fadeInterval >= fadeCostTime)
            {
                fadeInterval = 0;
                audioSource.volume = 1;
                fadeIn = false;
            }else
            {
                audioSource.volume = fadeInterval / fadeCostTime;
            }
        }
        else if (fadeOut)
        {
            fadeInterval += Time.deltaTime;
            if (fadeInterval >= fadeCostTime)
            {
                fadeInterval = 0;
                audioSource.volume = 0;
                fadeOut = false;
                PauseBgm();
            }else
            {
                audioSource.volume =  1 - fadeInterval / fadeCostTime;
            }
        }
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

    public void PauseBgm()
    {
        if (audioSource.isPlaying)
            audioSource.Pause();
    }

    public void UnPauseBgm()
    {
        if (!audioSource.isPlaying)
            audioSource.UnPause();
    }

    public void PlayBgm(float delay = 0f) {
        if (!audioSource.isPlaying)
            audioSource.PlayDelayed(delay);
    }


    public void SetEffectVolume(int volume)
    {
        effectAudioSource.volume = volume * 0.1f + 0.5f;
    }

    public void PlayBingoEffect() {
        fadeCostTime = 1.0f;
        fadeOut = true;

        StartCoroutine(DelayPlayBingoEffect());
    }

    IEnumerator DelayPlayBingoEffect()
    {
        yield return new WaitForSeconds(fadeCostTime);
        effectAudioSource.clip = bingoEffect;
        effectAudioSource.Play();
        StartCoroutine(ResumePlayBgm());
    }

    IEnumerator ResumePlayBgm()
    {
        yield return new WaitForSeconds(3.0f);
        UnPauseBgm();
        fadeIn = true;
    }

    public void PlayNumberCheckEffect() {
        numberRotateAudioSource.clip = numberCheckEffect;
        numberRotateAudioSource.loop = false;
        numberRotateAudioSource.Play();
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
