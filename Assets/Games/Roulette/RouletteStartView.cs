// Created by LunarEclipse on 2024-6-21 1:53.

using System;
using Luna.UI.Navigation;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace USEN.MiniGames.Roulette
{
    public class RouletteStartView : MonoBehaviour
    {
        public Button _startButton;
        
        private void Start()
        {
            
        }

        private void OnEnable()
        {
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