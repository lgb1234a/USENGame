// Created by LunarEclipse on 2024-6-21 1:45.

using System;
using Luna.UI;
using Luna.UI.Navigation;
using UnityEngine;
using UnityEngine.EventSystems;

namespace USEN.MiniGames.Roulette
{
    public class RouletteGameSelectionView : Widget, IEventSystemHandler
    {
        public RouletteGameSelectionList rouletteGameSelectionList;
        public RouletteContentList rouletteContentList;
        public RouletteWheel rouletteWheel;
        public UsenBottomPanel bottomPanel;
        
        void Awake()
        {
            // rouletteGameSelectionList.OnCellSelected += OnCellSelected;
            // rouletteGameSelectionList.OnCellDeselected += OnCellDeselected;
            // rouletteGameSelectionList.OnCellSubmitted += OnCellSubmitted;
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
            if (rouletteGameSelectionList.gameObject.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Escape) ||
                    Input.GetButtonDown("Cancel")) {
                    Navigator.Pop();
                }
            }
            else if (rouletteContentList.gameObject.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Escape) ||
                    Input.GetButtonDown("Cancel")) {
                    rouletteContentList.gameObject.SetActive(false);
                    rouletteGameSelectionList.gameObject.SetActive(true);
                }
            }
        }
    }
}