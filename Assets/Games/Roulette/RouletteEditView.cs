// Created by LunarEclipse on 2024-7-11 23:55.

using System.Collections.Generic;
using Luna.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using USEN.Games.Common;

namespace USEN.Games.Roulette
{
    public class RouletteEditView : Widget
    {
        public TextMeshProUGUI title;
        public TMP_InputField gameTitle;
        public Button sectorCounterButton;
        public TextMeshProUGUI sectorCounter;
        public RouletteEditList listView;
        public BottomPanel bottomPanel;
        
        private RouletteData _data;
        
        public RouletteData Data
        {
            get => _data;
            set
            {
                if (title.text == "")
                {
                    if (value.sectors.Count > 0)
                        title.text = "編集";
                    else title.text = "新規作成";
                }
                gameTitle.text = value.title;
                listView.Data = new List<RouletteSector>(value.sectors);
                sectorCounter.text = $"{value.sectors.Count}";
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