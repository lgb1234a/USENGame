// Created by LunarEclipse on 2024-7-21 19:44.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Games.Yamanote;
using Luna.Extensions;
using Luna.UI;
using Luna.UI.Audio;
using Luna.UI.Navigation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using USEN.Games.Common;
using USEN.Games.Roulette;

namespace USEN.Games.Yamanote
{
    public class YamanoteGameView : Widget
    {
        public ImageShaderController cloudController;
        public ImageShaderController buildingsController;
        public Button startButton;
        public GameObject questionsView;
        public YamanoteQuestionsPicker questionsPicker;
        public BottomPanel bottomPanel;
        
        public Sprite rouletteBackground;
        
        private List<string> _questions;
        public List<string> Questions
        {
            get => _questions;
            set
            {
                _questions = value;
                questionsPicker.itemCount = Int32.MaxValue;
                questionsPicker.ItemBuilder = (index) =>
                {
                    return _questions[index.Mod(_questions.Count)];
                };
            }
        }

        private void Start()
        {
            Debug.Log("YamanoteGameView started.");
            cloudController.speed = new Vector2(-0.05f, 0f);
            buildingsController.speed = new Vector2(-0.5f, 0f);
            startButton.onClick.AddListener(OnStartButtonClicked);
            
            EventSystem.current.SetSelectedGameObject(startButton.gameObject);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) ||
                Input.GetButtonDown("Cancel")) {
                OnExitButtonClicked();
            }
        }

        private void OnEnable()
        {
            bottomPanel.onExitButtonClicked += OnExitButtonClicked;
            bottomPanel.onSelectButtonClicked += OnStartButtonClicked;
            bottomPanel.onConfirmButtonClicked += OnConfirmButtonClicked;
            bottomPanel.onRedButtonClicked += OnRedButtonClicked;
            bottomPanel.onBlueButtonClicked += OnBlueButtonClicked;
            bottomPanel.onGreenButtonClicked += OnGreenButtonClicked;
            bottomPanel.onYellowButtonClicked += OnYellowButtonClicked;
        }

        private void OnDisable()
        {
            bottomPanel.onExitButtonClicked -= OnExitButtonClicked;
            bottomPanel.onSelectButtonClicked -= OnStartButtonClicked;
            bottomPanel.onConfirmButtonClicked -= OnConfirmButtonClicked;
            bottomPanel.onRedButtonClicked -= OnRedButtonClicked;
            bottomPanel.onBlueButtonClicked -= OnBlueButtonClicked;
            bottomPanel.onGreenButtonClicked -= OnGreenButtonClicked;
            bottomPanel.onYellowButtonClicked -= OnYellowButtonClicked;
        }

        public async void OnStartButtonClicked()
        {
            startButton.gameObject.SetActive(false);
            questionsView.SetActive(true);
            
            await PickNextRandomQuestion();
            ShowControlButtons();
        }
        
        private void OnExitButtonClicked()
        {
            PopupConfirmView();
        }

        private void OnConfirmButtonClicked()
        {
            
        }

        private void OnRedButtonClicked()
        {
            Navigator.Push<YamanoteGameOverView>();
        }

        private void OnBlueButtonClicked()
        {
            PickNextRandomQuestion();
        }
        
        private async void OnGreenButtonClicked()
        {
            await Navigator.Push<USEN.Games.Roulette.RouletteGameView>(async (view) => {
                var dao = await RouletteDAO.Instance;
                view.RouletteData = dao.GetRandomRoulette();
                view.backgroundImage.sprite = rouletteBackground;
            });
            await UniTask.NextFrame();
        }

        private async void OnYellowButtonClicked()
        {
            BgmManager.Pause();
            await Navigator.Push<CommendView>();
            BgmManager.Resume();
        }
        
        public async Task PickNextRandomQuestion()
        {
            var questionsCount = _questions.Count;
            var randomIndex = UnityEngine.Random.Range(0, questionsCount);
            await questionsPicker.ScrollTo(questionsPicker.FirstVisibleIndex + questionsCount + randomIndex, 2);
        }
        
        private void ShowControlButtons()
        {
            bottomPanel.blueButton.gameObject.SetActive(true);
            bottomPanel.redButton.gameObject.SetActive(true);
            bottomPanel.greenButton.gameObject.SetActive(true);
            bottomPanel.yellowButton.gameObject.SetActive(true);
        }
        
        private void PopupConfirmView()
        {
            Navigator.ShowModal<PopupOptionsView>(
                builder: (popup) =>
                {
                    popup.onOption1 = () => Navigator.Pop();
                    popup.onOption2 = () => Navigator.PopToRoot();
                    popup.onOption3 = () => SceneManager.LoadScene("GameEntries");
                });
        }
    }
}