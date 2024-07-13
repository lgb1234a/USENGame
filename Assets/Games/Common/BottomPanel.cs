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
        
        void Awake()
        {
            exitButton.onClick.AddListener(() => OnExitButtonClicked());
            selectButton.onClick.AddListener(() => OnSelectButtonClicked());
            confirmButton.onClick.AddListener(() => OnConfirmButtonClicked());
            redButton.onClick.AddListener(() => OnRedButtonClicked());
            blueButton.onClick.AddListener(() => OnBlueButtonClicked());
            greenButton.onClick.AddListener(() => OnGreenButtonClicked());
            yellowButton.onClick.AddListener(() => OnYellowButtonClicked());
        }

        private void OnEnable()
        {
            Debug.Log("BottomPanel enabled");
        }
        
        private void OnDisable()
        {
            Debug.Log("BottomPanel disabled");
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