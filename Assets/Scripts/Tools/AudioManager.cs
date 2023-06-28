using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviourSingletonTemplate<AudioManager>
{
    public AudioSource audioSource;

    public AudioSource effectAudioSource;

    public void SetBgmVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void SetEffectVolume(float volume)
    {
        // effectAudioSource.volume = volume;
    }
}
