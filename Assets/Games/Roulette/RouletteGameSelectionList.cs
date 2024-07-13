using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Luna.Core.Pool;
using Luna.UI;
using Modules.UI.Widgets;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace USEN.Games.Roulette
{
    
    public class RouletteGameSelectionList : ListView<RouletteGameSelectionListCell, RouletteData>, IEventSystemHandler
    {
        protected override void OnCellSubmitted(int index, RouletteGameSelectionListCell listViewCell)
        {
            
        }

        protected override void OnCellDeselected(int index, RouletteGameSelectionListCell listViewCell)
        {
            listViewCell.text.color = Color.white;
        }

        protected override void OnCellSelected(int index, RouletteGameSelectionListCell listViewCell)
        {
            listViewCell.text.color = Color.black;
            
            // Emit event
            ExecuteEvents.ExecuteHierarchy<RouletteGameSelectionView>(gameObject, null, (target, data) =>
            {
                this.SnapTo(transform as RectTransform);
                if (target.rouletteWheel != null)
                {
                    // Change roulette wheel data
                    target.rouletteWheel.RouletteData = SelectedData;
                }
            });
        }

        public void SnapTo(RectTransform target)
        {
            var y = -target.anchoredPosition.y - ((RectTransform)_scrollRect.transform).rect.height;
            y = Mathf.Clamp(y, 0, _scrollRect.content.rect.height);
            var pos = new Vector2(_scrollRect.content.anchoredPosition.x, y);
            DOTween.To(() => _scrollRect.content.anchoredPosition, v => _scrollRect.content.anchoredPosition = v, pos, 0.5f);
        }
    }

}
