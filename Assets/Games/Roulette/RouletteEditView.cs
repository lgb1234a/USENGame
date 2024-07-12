// Created by LunarEclipse on 2024-7-11 23:55.

using System;
using System.Collections.Generic;
using System.Linq;
using Luna.UI;
using Modules.UI.Widgets;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using USEN.Games.Common;

namespace USEN.MiniGames.Roulette
{
    public class RouletteEditView : Widget
    {
        public TextMeshProUGUI title;
        public TMP_InputField gameTitle;
        public Button sectorCounterButton;
        public TextMeshProUGUI sectorCounter;
        public RouletteEditList listView;
        public BottomPanel bottomPanel;
        
        private RouletteSectors _data;
        
        public RouletteSectors Data
        {
            get => _data;
            set
            {
                if (title.text == "")
                {
                    if (value.objects.Count > 0)
                        title.text = "編集";
                    else title.text = "新規作成";
                }
                gameTitle.text = value.name;
                listView.Data = new List<RouletteSector>(value.objects);
                sectorCounter.text = $"{value.objects.Count}";
            }
        }
        
        private void OnEnable()
        {
            Debug.Log("[RouletteEditView] OnEnable");
            EventSystem.current.SetSelectedGameObject(gameTitle.gameObject);
        }

        private void Start()
        {
            SetNavigation();
        }

        private void SetNavigation()
        {
            if (listView.cells.Count == 0) return;
            
            Navigation navigation1 = new Navigation
            {
                mode = Navigation.Mode.Explicit,
                selectOnUp = gameTitle,
                selectOnDown = listView.cells[0],
            };
            sectorCounterButton.navigation = navigation1;
            
            Navigation navigation2 = new Navigation
            {
                mode = Navigation.Mode.Explicit,
                selectOnUp = sectorCounterButton,
                selectOnDown = listView.cells.Count > 1 ? listView.cells[1] : null,
            };
            listView.cells[0].navigation = navigation2;
            listView.cells[0].inputField.navigation = navigation2;
        }
    }
}