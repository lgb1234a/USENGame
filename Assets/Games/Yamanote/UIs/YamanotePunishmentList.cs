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
using USEN.Games.Roulette;

namespace USEN.Games.Yamanote
{
    
    public class YamanotePunishmentList : ListView<YamanotePunishmentListCell, RouletteData>, IEventSystemHandler
    {
        protected override void OnCellSubmitted(int index, YamanotePunishmentListCell listViewCell)
        {
            
        }

        protected override void OnCellDeselected(int index, YamanotePunishmentListCell listViewCell)
        {
            listViewCell.text.color = Color.white;
        }
        
        protected override void OnCellSelected(int index, YamanotePunishmentListCell listViewCell)
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
    }

}
