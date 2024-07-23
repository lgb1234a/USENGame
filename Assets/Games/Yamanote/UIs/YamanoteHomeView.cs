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
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace USEN.Games.Yamanote
{
    public class YamanoteHomeView : Widget
    {
        public Button startButton;
        public Button settingsButton;
        
        private void Start()
        {
            Debug.Log("YamanoteHomeView started.");
            startButton.onClick.AddListener(OnStartButtonClicked);
            settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        }

        private void OnEnable()
        {
            if (startButton != null)
            {
                EventSystem.current.SetSelectedGameObject(startButton.gameObject);
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
            Navigator.Push<YamanoteCategoryView>();
        }
        
        public void OnSettingsButtonClicked()
        {
            Navigator.Push<YamanoteSettingsView>();
        }
    }
}