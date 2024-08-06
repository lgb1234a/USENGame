// Created by LunarEclipse on 2024-6-21 1:53.

using Cysharp.Threading.Tasks;
using Luna;
using Luna.UI;
using Luna.UI.Audio;
using Luna.UI.Navigation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace USEN.Games.Roulette
{
    public class RouletteStartView : Widget
    {
        public Button startButton;
        public Button settingsButton;
        
        public AudioClip bgmClip;
        
        private void Start()
        {
            EventSystem.current.SetSelectedGameObject(startButton.gameObject);
            BgmManager.Play(bgmClip);
            
            // Preload all roulette widgets
            // Widget.Load(GetType().Namespace);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) ||
                Input.GetButtonDown("Cancel")) {
                OnExitButtonClicked();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                BgmManager.Play(bgmClip);
            }
        }

        private void OnDestroy()
        {
            BgmManager.Stop();
            
            // Unload all roulette widgets
            // Widget.Unload(GetType().Namespace);
        }

        public void OnStartButtonClicked()
        {
            Navigator.Push<RouletteCategoryView>();
        }
        
        public void OnSettingsButtonClicked()
        {
            Navigator.Push<RouletteSettingsView>();
        }
        
        public void OnExitButtonClicked()
        {
            SceneManager.LoadScene("GameEntries");
        }
    }
}