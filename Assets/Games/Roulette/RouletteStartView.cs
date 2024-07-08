// Created by LunarEclipse on 2024-6-21 1:53.

using System;
using Luna.UI;
using Luna.UI.Navigation;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace USEN.MiniGames.Roulette
{
    public class RouletteStartView : Widget
    {
        public Button _startButton;
        
        private void Start()
        {
            Debug.Log("RouletteStartView started.");
        }

        private void OnEnable()
        {
            base.OnEnable();
            
            if (_startButton != null)
            {
                EventSystem.current.SetSelectedGameObject(_startButton.gameObject);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) ||
                Input.GetButtonDown("Cancel")) {
                SceneManager.LoadScene("GameEntries");
            }
        }
        
        protected  KeyEventResult OnKey(KeyControl key, KeyEvent keyEvent)
        {
            Debug.Log($"[RouletteStartView] Key pressed: {key.keyCode} with event: {keyEvent}");
            // switch (key.keyCode)
            // {
            //     case Key.Enter:
            //         var selected = EventSystem.current.currentSelectedGameObject;
            //         if (selected != null)
            //         {
            //             selected.GetComponent<Button>().onClick.Invoke();
            //             return KeyEventResult.Handled;
            //         }
            //         break;
            // }
            return KeyEventResult.Unhandled;
        }

        public void OnStartButtonClicked()
        {
            Debug.Log("Start button clicked.");
            Navigator.Push<RouletteThemeSelectionView>();
        }
        
        public void OnSettingsButtonClicked()
        {
            Debug.Log("Settings button clicked.");
        }
    }
}