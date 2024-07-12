// Created by LunarEclipse on 2024-6-21 1:45.

using System;
using Luna.UI;
using Luna.UI.Navigation;
using UnityEngine;
using UnityEngine.EventSystems;
using USEN.Games.Common;

namespace USEN.MiniGames.Roulette
{
    public class RouletteGameSelectionView : Widget, IEventSystemHandler
    {
        public RouletteGameSelectionList rouletteGameSelectionList;
        public RouletteContentList rouletteContentList;
        public RouletteWheel rouletteWheel;
        public BottomPanel bottomPanel;

        void Awake()
        {
            rouletteGameSelectionList.onCellSubmitted += (index, cell) => ShowContentView();
        }
        
        private void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
        
        void Start()
        {
            
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
                Navigator.Push<RouletteGameView>();
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
            rouletteContentList.Data = rouletteGameSelectionList.SelectedData.objects;
            rouletteContentList.gameObject.SetActive(true);
        }
        
        private void HideContentView()
        {
            rouletteContentList.gameObject.SetActive(false);
            rouletteGameSelectionList.gameObject.SetActive(true);
        }
    }
}