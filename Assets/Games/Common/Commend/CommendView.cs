// Created by LunarEclipse on 2024-7-19 14:12.

using System;
using System.Collections.Generic;
using Luna.UI;
using Luna.UI.Navigation;
using UnityEngine;
using UnityEngine.Video;

namespace USEN.Games.Common.Commend
{
    public class CommendView : Widget
    {
        public VideoPlayer videoPlayer;
        public List<VideoClip> videoClips;

        // public int index;

        private void Start()
        {
            var index = AppConfig.Instance.CommendationVideoOption;
            
            videoPlayer.targetCamera = Camera.main;
            
            if (index < videoClips.Count)
            {
                videoPlayer.clip = videoClips[index];
                videoPlayer.Prepare();
            }
            
            videoPlayer.prepareCompleted += OnVideoPrepared;
            videoPlayer.loopPointReached += OnVideoEnd;
        }

        private void OnVideoPrepared(VideoPlayer source)
        {
            videoPlayer.Play();
        }

        private void OnVideoEnd(VideoPlayer source)
        {
            // Navigator.Pop();
        }
    }
}