// Created by LunarEclipse on 2024-5-31 22:54.

using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

namespace HightAndLowGame
{
    public class ResultPlayerDirector : MonoBehaviour
    {
        private PlayableDirector _playableDirector;
        [FormerlySerializedAs("timelineAsset1")] public PlayableAsset high; 
        [FormerlySerializedAs("timelineAsset2")] public PlayableAsset low; 
        
        void Awake()
        {
            _playableDirector = GetComponent<PlayableDirector>();
        }

        public void PlayHighResult()
        {
            _playableDirector.Play(high);
        }
        
        public void PlayLowResult()
        {
            _playableDirector.Play(low);
        }
        
    }
}