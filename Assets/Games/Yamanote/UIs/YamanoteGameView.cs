// Created by LunarEclipse on 2024-7-21 19:44.

using System;
using Games.Yamanote;
using Luna.UI;
using Luna.UI.Navigation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using USEN.Games.Common;

namespace USEN.Games.Yamanote
{
    public class YamanoteGameView : Widget
    {
        public ImageShaderController cloudController;
        public ImageShaderController buildingsController;
        public Button startButton;
        public BottomPanel bottomPanel;
        
        private YamanoteTheme _theme;
        public YamanoteTheme Theme
        {
            get => _theme;
            set
            {
                _theme = value;
            }
        }

        private void Start()
        {
            Debug.Log("YamanoteGameView started.");
            cloudController.speed = new Vector2(-0.05f, 0f);
            buildingsController.speed = new Vector2(-0.5f, 0f);
            startButton.onClick.AddListener(OnStartButtonClicked);
            
            EventSystem.current.SetSelectedGameObject(startButton.gameObject);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) ||
                Input.GetButtonDown("Cancel")) {
                Navigator.Pop();
            }
        }

        private void OnStartButtonClicked()
        {
            
        }
    }
}