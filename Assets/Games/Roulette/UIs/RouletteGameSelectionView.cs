// Created by LunarEclipse on 2024-6-21 1:45.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Luna.UI;
using Luna.UI.Navigation;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using USEN.Games.Common;

namespace USEN.Games.Roulette
{
    public class RouletteGameSelectionView : Widget, IEventSystemHandler
    {
        public TextMeshProUGUI titleText;
        public RouletteGameSelectionList rouletteGameSelectionList;
        public RouletteContentList rouletteContentList;
        public RouletteWheel rouletteWheel;
        public BottomPanel bottomPanel;
        
        private RouletteDAO _dao;
        private RouletteCategory _category;
        public RouletteCategory Category
        {
            get => _category;
            set
            {
                _category = value;
                rouletteGameSelectionList.Data = value.roulettes;

                if (value.title == "オリジナル")
                {
                    bottomPanel.redButton.gameObject.SetActive(true);
                    bottomPanel.yellowButton.gameObject.SetActive(true);
                }
                else
                {
                    bottomPanel.redButton.gameObject.SetActive(false);
                    bottomPanel.yellowButton.gameObject.SetActive(false);
                }

                titleText.text = value.title;
            }
        }
        
        private bool IsOriginal => Category.title == "オリジナル";

        async void Awake()
        {
            rouletteGameSelectionList.onCellSelected += (index, cell) => rouletteWheel.RouletteData = cell.Data;
            rouletteGameSelectionList.onCellSubmitted += (index, cell) => OnConfirmButtonClicked();
            rouletteContentList.onCellSubmitted += (index, cell) => OnConfirmButtonClicked();
            
            bottomPanel.onYellowButtonClicked += () =>
            {
                if (rouletteGameSelectionList.gameObject.activeSelf && 
                    rouletteGameSelectionList.Data.Count > 0)
                    rouletteGameSelectionList.Remove(rouletteGameSelectionList.SelectedIndex);
                
                if (rouletteContentList.gameObject.activeSelf && 
                    rouletteContentList.Data.Count > 2)
                    rouletteContentList.Remove(rouletteContentList.SelectedIndex);
                
                rouletteWheel.DrawRouletteWheel();
                _dao?.SaveToFile();
            };
            
            _dao = await RouletteDAO.Instance;
        }

        private void OnEnable()
        {
            HideContentView();
            bottomPanel.onRedButtonClicked += OnRedButtonClicked;
            bottomPanel.onBlueButtonClicked += OnBlueButtonClicked;
        }

        private void OnDisable()
        {
            bottomPanel.onRedButtonClicked -= OnRedButtonClicked;
            bottomPanel.onBlueButtonClicked -= OnBlueButtonClicked;
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

        public async void OnBlueButtonClicked()
        {
            // Edit roulette
            var result = await Navigator.Push<RouletteEditView>((view) => {
                view.Data = rouletteGameSelectionList.SelectedData;
            }) as RouletteData;
            
            // Add to category and save
            if (result != null)
            {
                if (IsOriginal)
                {
                    Category.roulettes[rouletteGameSelectionList.SelectedIndex] = result;
                    rouletteWheel.RouletteData = result;
                    rouletteGameSelectionList.Reload();
                }
                else
                {
                    _dao?.Data?.categories?.Last()?.roulettes?.Add(result);
                }
                
                _dao?.SaveToFile();
            }
        }

        public async void OnRedButtonClicked()
        {
            // Create new roulette
            var roulette = new RouletteData();
            roulette.title = "新規ルーレット";
            roulette.sectors = new List<RouletteSector>();
            for (int i = 0; i < 8; i++)
            {
                roulette.sectors.Add(new RouletteSector()
                {
                    content = $"",
                    weight = 1,
                    color = Color.HSVToRGB(1.0f / 8 * i, 0.5f, 1f),
                });
            }
            
            // Open edit view
            var result = await Navigator.Push<RouletteEditView>((view) => {
                view.Data = roulette;
            }) as RouletteData;
            
            // Add to category and save
            if (result != null)
            {
                if (IsOriginal)
                {
                    Category.roulettes.Add(result);
                    // Category = Category;
                    rouletteGameSelectionList.Reload();
                }
                else
                {
                    _dao?.Data?.categories?.Last()?.roulettes?.Add(result);
                }
                
                _dao?.SaveToFile();
            }
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
            rouletteGameSelectionList.Select(rouletteGameSelectionList.SelectedIndex);
        }
    }
}