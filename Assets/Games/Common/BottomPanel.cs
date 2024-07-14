// Created by LunarEclipse on 2024-7-6 21:58.

using System;
using Luna.UI;
using Luna.UI.Navigation;
using UnityEngine;
using UnityEngine.UI;

namespace USEN.Games.Common
{
    public class BottomPanel: MonoBehaviour
    {
        public Button exitButton;
        public Button selectButton;
        public Button confirmButton;
        public Button redButton;
        public Button blueButton;
        public Button greenButton;
        public Button yellowButton;
        
        public bool clickExitToPop = false;
        
        public event Action onExitButtonClicked;
        public event Action onSelectButtonClicked;
        public event Action onConfirmButtonClicked;
        public event Action onRedButtonClicked;
        public event Action onBlueButtonClicked;
        public event Action onGreenButtonClicked;
        public event Action onYellowButtonClicked;
        
        private EventHandler<EventArgs> _redButtonEventHandler;
        private EventHandler<EventArgs> _blueButtonEventHandler;
        private EventHandler<EventArgs> _greenButtonEventHandler;
        private EventHandler<EventArgs> _yellowButtonEventHandler;
        
        void Awake()
        {
            exitButton.onClick.AddListener(OnExitButtonClicked);
            selectButton.onClick.AddListener(OnSelectButtonClicked);
            confirmButton.onClick.AddListener(OnConfirmButtonClicked);
            redButton.onClick.AddListener(OnRedButtonClicked);
            blueButton.onClick.AddListener(OnBlueButtonClicked);
            greenButton.onClick.AddListener(OnGreenButtonClicked);
            yellowButton.onClick.AddListener(OnYellowButtonClicked);
            
            _redButtonEventHandler = (sender, args) => OnRedButtonClicked();
            _blueButtonEventHandler = (sender, args) => OnBlueButtonClicked();
            _greenButtonEventHandler = (sender, args) => OnGreenButtonClicked();
            _yellowButtonEventHandler = (sender, args) => OnYellowButtonClicked();
        }

        private void OnEnable()
        {
            UsenEvents.OnRemoconRedButtonClicked += _redButtonEventHandler;
            UsenEvents.OnRemoconBlueButtonClicked += _blueButtonEventHandler;
            UsenEvents.OnRemoconGreenButtonClicked += _greenButtonEventHandler;
            UsenEvents.OnRemoconYellowButtonClicked += _yellowButtonEventHandler;
        }
        
        private void OnDisable()
        {
            UsenEvents.OnRemoconRedButtonClicked -= _redButtonEventHandler;
            UsenEvents.OnRemoconBlueButtonClicked -= _blueButtonEventHandler;
            UsenEvents.OnRemoconGreenButtonClicked -= _greenButtonEventHandler;
            UsenEvents.OnRemoconYellowButtonClicked -= _yellowButtonEventHandler;
        }

        private void Update()
        {
#if DEBUG
            if (Input.GetKeyDown(KeyCode.Keypad0))
                OnExitButtonClicked();
            else if (Input.GetKeyDown(KeyCode.KeypadPeriod))
                OnSelectButtonClicked();
            else if (Input.GetKeyDown(KeyCode.KeypadEnter))
                OnConfirmButtonClicked();
            
            if (Input.GetKeyDown(KeyCode.Keypad1))
                OnBlueButtonClicked();
            else if (Input.GetKeyDown(KeyCode.Keypad2))
                OnRedButtonClicked();
            else if (Input.GetKeyDown(KeyCode.Keypad3))
                OnGreenButtonClicked();
            else if (Input.GetKeyDown(KeyCode.Keypad4))
                OnYellowButtonClicked();
#endif
        }

        private void OnExitButtonClicked()
        {
            if (clickExitToPop)
                Navigator.Pop();
            onExitButtonClicked?.Invoke();
        }

        private void OnSelectButtonClicked()
        {
            onSelectButtonClicked?.Invoke();
        }

        private void OnConfirmButtonClicked()
        {
            onConfirmButtonClicked?.Invoke();
        }

        private void OnRedButtonClicked()
        {
            onRedButtonClicked?.Invoke();
        }

        private void OnBlueButtonClicked()
        {
            onBlueButtonClicked?.Invoke();
        }

        private void OnGreenButtonClicked()
        {
            onGreenButtonClicked?.Invoke();
        }

        private void OnYellowButtonClicked()
        {
            onYellowButtonClicked?.Invoke();
        }
    }
}