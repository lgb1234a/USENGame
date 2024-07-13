// Created by LunarEclipse on 2024-7-6 21:4.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Luna.Extensions;
using Modules.UI.Widgets;
using UnityEngine;
using UnityEngine.EventSystems;

namespace USEN.Games.Roulette
{
    public class RouletteContentList : ListView<RouletteContentListCell, RouletteSector>
    {
        async Task OnEnable()
        {
            // Select first cell
            var firstCell = cells.First().gameObject;
            if (firstCell != null)
            {
                await UniTask.NextFrame();
                EventSystem.current.SetSelectedGameObject(firstCell);
            }
        }
        
        protected override void OnCellSubmitted(int index, RouletteContentListCell listViewCell)
        {
            Debug.Log($"Cell {index} submitted.");
        }

        protected override void OnCellDeselected(int index, RouletteContentListCell listViewCell)
        {
            Debug.Log($"Cell {index} deselected.");
        }

        protected override void OnCellSelected(int index, RouletteContentListCell listViewCell)
        {
            Debug.Log($"Cell {index} selected.");
            if (Input.GetButtonDown("Vertical"))
            {
                SnapTo(listViewCell.transform as RectTransform);
            }
        }
    }
}