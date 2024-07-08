// Created by LunarEclipse on 2024-7-6 21:4.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Luna.Extensions;
using Modules.UI.Widgets;
using UnityEngine;
using UnityEngine.EventSystems;

namespace USEN.MiniGames.Roulette
{
    public class RouletteContentList : ListView<RouletteContentListCell, string>
    {
        async Task OnEnable()
        {
            // Select first cell
            var firstCell = _cells.First().gameObject;
            if (firstCell != null)
            {
                await UniTask.NextFrame();
                EventSystem.current.SetSelectedGameObject(firstCell);
            }
        }
        
        protected override void OnCellSubmitted(int index, ListViewCell<string> listViewCell)
        {
            Debug.Log($"Cell {index} submitted.");
        }

        protected override void OnCellDeselected(int index, ListViewCell<string> listViewCell)
        {
            Debug.Log($"Cell {index} deselected.");
        }

        protected override void OnCellSelected(int index, ListViewCell<string> listViewCell)
        {
            Debug.Log($"Cell {index} selected.");
        }
        
        public void FocusOnCell(int index)
        {
            if (_cells.Count == 0) return;
            
            var cell = _cells[index.Mod(_cells.Count)];
            cell.OnSelect(null);
            EventSystem.current.SetSelectedGameObject(cell.gameObject);
        }
    }
}