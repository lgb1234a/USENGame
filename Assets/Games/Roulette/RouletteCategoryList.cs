using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Luna.Core.Pool;
using Luna.UI;
using Luna.UI.Navigation;
using Modules.UI.Widgets;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace USEN.Games.Roulette
{
    
    public class RouletteCategoryList : ListView<RouletteCategoryListCell, RouletteCategory>, IEventSystemHandler
    {
        public void OnCellClicked()
        {
            Navigator.Push<RouletteGameSelectionView>((view) =>
            {
                view.Category = SelectedData;
            });
        }
        
        protected override void OnCellDeselected(int index, RouletteCategoryListCell listViewCell)
        {
            listViewCell.text.color = Color.white;
        }

        protected override void OnCellSelected(int index, RouletteCategoryListCell listViewCell)
        {
            listViewCell.text.color = Color.black;
        }
    }

}
