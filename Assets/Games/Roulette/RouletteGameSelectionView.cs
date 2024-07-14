// Created by LunarEclipse on 2024-6-21 1:45.

using System;
using Cysharp.Threading.Tasks;
using Luna.UI;
using Luna.UI.Navigation;
using UnityEngine;
using UnityEngine.EventSystems;
using USEN.Games.Common;

namespace USEN.Games.Roulette
{
    public class RouletteGameSelectionView : Widget, IEventSystemHandler
    {
        public RouletteGameSelectionList rouletteGameSelectionList;
        public RouletteContentList rouletteContentList;
        public RouletteWheel rouletteWheel;
        public BottomPanel bottomPanel;
        
        private RouletteCategory _category;
        public RouletteCategory Category
        {
            get => _category;
            set
            {
                _category = value;
                rouletteGameSelectionList.Data = value.roulettes;
            }
        }

        void Awake()
        {
            rouletteGameSelectionList.onCellSubmitted += (index, cell) => OnConfirmButtonClicked();
            rouletteContentList.onCellSubmitted += (index, cell) => OnConfirmButtonClicked();
            
            bottomPanel.onRedButtonClicked += OnRedButtonClicked;
            bottomPanel.onBlueButtonClicked += OnBlueButtonClicked;
        }

        private void OnEnable()
        {
            HideContentView();
            Category = _category;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) ||
                Input.GetButtonDown("Cancel")) {
                OnExitButtonClicked();
            }
        }
        
        public void OnConfirmButtonClicked()
        {
            if (rouletteGameSelectionList.gameObject.activeSelf)
            {
                ShowContentView();
            }
            else if (rouletteContentList.gameObject.activeSelf)
            {
                Navigator.Push<RouletteGameView>(async (view) =>
                {
                    await UniTask.NextFrame();
                    view.RouletteData = rouletteGameSelectionList.SelectedData;
                });
            }
        }

        public void OnExitButtonClicked()
        {
            if (rouletteGameSelectionList.gameObject.activeSelf)
            {
                Navigator.Pop();
            }
            else if (rouletteContentList.gameObject.activeSelf)
            {
                HideContentView();
            }
        }

        public void OnBlueButtonClicked()
        {
            Navigator.Push<RouletteEditView>((view) =>
            {
                view.Data = rouletteGameSelectionList.SelectedData;
            });
        }

        public void OnRedButtonClicked()
        {
            
        }
        
        private void ShowContentView()
        {
            rouletteGameSelectionList.gameObject.SetActive(false);
            rouletteContentList.Data = rouletteGameSelectionList.SelectedData.sectors;
            rouletteContentList.gameObject.SetActive(true);
        }
        
        private void HideContentView()
        {
            rouletteContentList.gameObject.SetActive(false);
            rouletteGameSelectionList.gameObject.SetActive(true);
        }
    }
}