// Created by LunarEclipse on 2024-6-30 18:50.

using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Luna.UI;
using Luna.UI.Navigation;
using UnityEngine;
using UnityEngine.UI;
using USEN.Games.Common;

namespace USEN.Games.Roulette
{
    public class RouletteGameView : Widget
    {
        public RouletteWheel rouletteWheel;
        public Button startButton;
        public BottomPanel bottomPanel;
        
        public RouletteData RouletteData { 
            get => rouletteWheel.RouletteData;
            set => rouletteWheel.RouletteData = value;
        }
        
        void OnEnable()
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
            bottomPanel.onExitButtonClicked += OnExitButtonClicked;
            bottomPanel.onSelectButtonClicked += OnStartButtonClicked;
            bottomPanel.onConfirmButtonClicked += OnConfirmButtonClicked;
            bottomPanel.onRedButtonClicked += OnRedButtonClicked;
            bottomPanel.onBlueButtonClicked += OnBlueButtonClicked;
            bottomPanel.onYellowButtonClicked += OnYellowButtonClicked;
        }
        
        void OnDisable()
        {
            startButton.onClick.RemoveListener(OnStartButtonClicked);
            bottomPanel.onExitButtonClicked -= OnExitButtonClicked;
            bottomPanel.onSelectButtonClicked -= OnStartButtonClicked;
            bottomPanel.onConfirmButtonClicked -= OnConfirmButtonClicked;
            bottomPanel.onRedButtonClicked -= OnRedButtonClicked;
            bottomPanel.onBlueButtonClicked -= OnBlueButtonClicked;
            bottomPanel.onYellowButtonClicked -= OnYellowButtonClicked;
        }

        private void OnStartButtonClicked()
        { 
            SpinWheel();
        }

        private void OnConfirmButtonClicked()
        {
            SpinWheel();
        }

        private void OnExitButtonClicked()
        {
            Navigator.Pop();
        }

        private void OnBlueButtonClicked()
        {
            
        }

        private void OnRedButtonClicked()
        {
            
        }
        
        private void OnYellowButtonClicked()
        {
            
        }
        
        private async Task SpinWheel()
        {
            Debug.Log("Start button clicked.");
            
            // Hide buttons
            startButton.gameObject.SetActive(false);
            
            // Spin the wheel
            rouletteWheel.SpinWheel();
            await UniTask.Delay((int)((rouletteWheel.spinDuration - 2) * 1000));
            
            // Dotween move & scale
            rouletteWheel.transform.parent.DOLocalMoveX(960, 1f).SetEase(Ease.InOutSine);
            rouletteWheel.transform.parent.DOScale(3f, 1f).SetEase(Ease.InOutSine);
        }
    }
}