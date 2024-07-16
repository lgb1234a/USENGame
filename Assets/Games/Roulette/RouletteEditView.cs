// Created by LunarEclipse on 2024-7-11 23:55.

using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Luna.UI;
using Luna.UI.Navigation;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
using USEN.Games.Common;
using Random = UnityEngine.Random;

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

        private bool _isEditing = false;
        private bool IsEditing => EventSystem.current.currentSelectedGameObject?.GetComponent<TMP_InputField>()?.isFocused ?? false;
        
        public RouletteData Data
        {
            get => _data;
            set
            {
                _data = value;
                
                if (title.text == "")
                {
                    if (value.sectors.Count > 0)
                        title.text = "編集";
                    else title.text = "新規作成";
                }
                gameTitle.text = value.title;
                listView.Data = value.sectors; // reference type, modify this will modify the data in database
                sectorCounter.text = $"{value.sectors.Count}";
            }
        }

        protected void Awake()
        {
            gameTitle.onValueChanged.AddListener((value) =>
            {
                Data.title = value;
            });
            
            listView.onCellCreated += (index, cell) =>
            {
                cell.onInputValueChanged += (i, cell, value) =>
                {
                    Data.sectors[i].content = value;
                };
            };
            
            listView.onCellSubmitted += async (index, cell) =>
            {
                await UniTask.NextFrame();
                cell.inputField.Select();
            };
            
            bottomPanel.onBlueButtonClicked += async () =>
            {
                Navigator.Pop(Data);
                await UniTask.NextFrame();
                Navigator.Push<RouletteGameView>(async (view) =>
                {
                    await UniTask.NextFrame();
                    view.RouletteData = Data;
                });
            };
            
            bottomPanel.onRedButtonClicked += () =>
            {
                Navigator.Pop(Data);
                RouletteDAO.Instance.SaveToFile();
            };
        }

        private void OnEnable()
        {
            Debug.Log("[RouletteEditView] OnEnable");
            EventSystem.current.SetSelectedGameObject(gameTitle.gameObject);
            base.OnKey += OnKey;
        }

        private void OnDisable()
        {
            Debug.Log("[RouletteEditView] OnDisable");
            base.OnKey -= OnKey;
        }

        private void Start()
        {
            SetNavigation();
        }
        
        private void Update()
        {
            Debug.Log($"[RouletteEditView] Update: {IsEditing}");
            
            if (Input.GetKeyDown(KeyCode.Escape) ||
                Input.GetButtonDown("Cancel")) {
                if (!_isEditing) Navigator.Pop(Data);
            }

            if (EventSystem.current.currentSelectedGameObject == sectorCounterButton.gameObject)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow))
                    AddSector();
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    RemoveSector();
            }
        }

        private KeyEventResult OnKey(KeyControl key, KeyEvent keyEvent)
        {
            // Debug.Log($"[RouletteEditView] Key pressed: {key.keyCode} with event: {keyEvent}");
            // Debug.Log($"[RouletteEditView] Current selected: {EventSystem.current.currentSelectedGameObject}");
            
            // if (keyEvent == KeyEvent.KeyDown &&
            //     EventSystem.current.currentSelectedGameObject == sectorCounterButton.gameObject)
            // {
            //     switch (key.keyCode)
            //     {
            //         case Key.RightArrow:
            //             AddSector();
            //             break;
            //         case Key.LeftArrow:
            //             RemoveSector();
            //             break;
            //     }
            // }
            // if (!IsEditing) Navigator.Pop();
            
            
            Debug.Log($"[RouletteEditView] Key pressed: {IsEditing} with event: {keyEvent}");
            if (keyEvent == KeyEvent.Down)
            {
                _isEditing = EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>()?.isFocused ?? false;
            }
            
            return KeyEventResult.Unhandled;
        }

        public void AddSector()
        {
            var newSector = new RouletteSector();
            newSector.color = RandomColor(0.5f);
            Data.sectors.Insert(0, newSector);
            listView.Data = Data.sectors;
            sectorCounter.text = $"{Data.sectors.Count}";
        }
        
        public void RemoveSector()
        {
            if (Data.sectors.Count == 0) return;
            Data.sectors.RemoveAt(0);
            listView.Data = Data.sectors;
            sectorCounter.text = $"{Data.sectors.Count}";
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
        
        private Color RandomColor(float saturation = 1, float brightness = 1)
        {
            return Color.HSVToRGB(Random.value, saturation, brightness);
        }
    }
}