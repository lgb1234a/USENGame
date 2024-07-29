// Created by LunarEclipse on 2024-7-18 9:26.

using Luna.UI;
using Luna.UI.Navigation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using USEN.Games.Common;

namespace USEN.Games.Yamanote
{
    public class YamanoteSettingsView: Widget
    {
        public Button appInfoButton;
        public BottomPanel bottomPanel;

        private void Start()
        {
            appInfoButton.onClick.AddListener(OnClickAppInfoButton);
        }

        private void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(appInfoButton.gameObject);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) ||
                Input.GetButtonDown("Cancel")) {
                Navigator.Pop();
            }
        }
        
        void OnClickAppInfoButton() {
            Navigator.Push<AppInfoView>();
        }
    }
}