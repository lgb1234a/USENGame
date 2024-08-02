// Created by LunarEclipse on 2024-6-30 18:50.

using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Luna.UI;
using Luna.UI.Audio;
using Luna.UI.Navigation;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using USEN.Assets;
using USEN.Games.Common;

namespace USEN.Games.Roulette
{
    public class RouletteGameView : Widget
    {
        public RouletteWheel rouletteWheel;
        public Button startButton;
        public BottomPanel bottomPanel;
        
        private AsyncOperationHandle<AudioClip>? _audioClipHandle;
        
        public RouletteData RouletteData { 
            get => rouletteWheel.RouletteData;
            set => rouletteWheel.RouletteData = value;
        }
        
        void OnEnable()
        {
            base.OnKey += OnKey;
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
            base.OnKey -= OnKey;
            startButton.onClick.RemoveListener(OnStartButtonClicked);
            bottomPanel.onExitButtonClicked -= OnExitButtonClicked;
            bottomPanel.onSelectButtonClicked -= OnStartButtonClicked;
            bottomPanel.onConfirmButtonClicked -= OnConfirmButtonClicked;
            bottomPanel.onRedButtonClicked -= OnRedButtonClicked;
            bottomPanel.onBlueButtonClicked -= OnBlueButtonClicked;
            bottomPanel.onYellowButtonClicked -= OnYellowButtonClicked;
        }

        private void Start()
        {
            EventSystem.current.SetSelectedGameObject(startButton.gameObject);
            AssetUtils.LoadAsync<CommendView>().ContinueWith(task => {
                var go = task.Result;
                var commendView = go.GetComponent<CommendView>();
                if (commendView != null) 
                    _audioClipHandle = commendView.PreloadAudio();
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) ||
                Input.GetButtonDown("Cancel")) {
                Navigator.Pop();
            }
        }

        private void OnDestroy()
        {
            AssetUtils.Unload<CommendView>();
            if (_audioClipHandle != null)
                Addressables.Release(_audioClipHandle.Value);
        }

        private KeyEventResult OnKey(KeyControl key, KeyEvent @event)
        {
            if (@event == KeyEvent.Down)
            {
                switch(key.keyCode)
                {
                    case Key.Enter:
                    case Key.Space:
                        OnStartButtonClicked();
                        break;
                }
            }
            return KeyEventResult.Unhandled;
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
            Navigator.Pop();
        }

        private void OnRedButtonClicked()
        {
            Navigator.Pop();
            Navigator.Pop();
        }
        
        private async void OnYellowButtonClicked()
        {
            BgmManager.Pause();
            await Navigator.Push<CommendView>();
            BgmManager.Resume();
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
            
            // Show buttons
            await UniTask.Delay(2 * 1000);
            bottomPanel.yellowButton.gameObject.SetActive(true);
        }
    }
}