// Created by LunarEclipse on 2024-7-14 7:54.

using System.Collections.Generic;
using UnityEngine;

namespace Luna.UI.Audio
{
    public static class SFXManager
    {
        private static Dictionary<string, AudioClip> soundEffects = new Dictionary<string, AudioClip>();
        private static AudioSource audioSource;

        [RuntimeInitializeOnLoadMethod]
        static void Initialize()
        {
            GameObject soundManagerObject = new GameObject("SFXManager");
            audioSource = soundManagerObject.AddComponent<AudioSource>();
            Object.DontDestroyOnLoad(soundManagerObject);
        }

        public static void AddSound(string clipName, AudioClip clip)
        {
            if (!soundEffects.ContainsKey(clipName))
            {
                soundEffects[clipName] = clip;
            }
            else
            {
                Debug.LogWarning($"SFXManager: Sound '{clipName}' already exists!");
            }
        }

        public static void PlaySound(string clipName)
        {
            if (soundEffects.ContainsKey(clipName))
            {
                audioSource.PlayOneShot(soundEffects[clipName]);
            }
            else
            {
                Debug.LogWarning($"SFXManager: Sound '{clipName}' not found!");
            }
        }
    }   
}