// Created by LunarEclipse on 2024-6-21 1:53.

using System;
using DG.Tweening;
using Luna.UI;
using Luna.UI.Navigation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using USEN.Games.Common;

namespace USEN.Games.Yamanote
{
    public class YamanoteHomeView : Widget
    {
        public Button startButton;
        public Button settingsButton;
        public Image trainImage;
        public BottomPanel bottomPanel;
        
        private void Start()
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
            settingsButton.onClick.AddListener(OnSettingsButtonClicked);
            bottomPanel.exitButton.onClick.AddListener(OnExitButtonClicked);
            
            EventSystem.current.SetSelectedGameObject(startButton.gameObject);
        }

        private void OnEnable()
        {
            // Train tween animation
            trainImage.transform.DOLocalMoveX(-5000, 1.5f).SetEase(Ease.OutSine);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) ||
                Input.GetButtonDown("Cancel")) 
                OnExitButtonClicked();
        }

        public void OnStartButtonClicked()
        {
            Navigator.Push<YamanoteCategoryView>();
        }
        
        private void OnExitButtonClicked()
        {
            SceneManager.LoadScene("GameEntries");
        }
        
        public void OnSettingsButtonClicked()
        {
            Navigator.Push<YamanoteSettingsView>();
        }
    }
}