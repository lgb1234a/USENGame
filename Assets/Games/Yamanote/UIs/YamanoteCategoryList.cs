using System;
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

namespace USEN.Games.Yamanote
{
    
    public class YamanoteCategoryList : ListView<YamanoteCategoryListCell, YamanoteCategory>, IEventSystemHandler
    {
        protected void Start()
        {
            var content = _scrollRect.content;
            var firstCell = content.GetChild(0).GetComponent<YamanoteCategoryListCell>();
            firstCell.Focus();
        }

        // protected override void OnCellClicked(int index, YamanoteCategoryListCell listViewCell)
        // {
        //     Navigator.Push<YamanoteGameSelectionView>((view) =>
        //     {
        //         view.Category = SelectedData;
        //     });
        // }
        //
        // protected override void OnCellSubmitted(int index, YamanoteCategoryListCell listViewCell)
        // {
        //     Navigator.Push<YamanoteGameSelectionView>((view) =>
        //     {
        //         view.Category = SelectedData;
        //     });
        // }

        protected override void OnCellDeselected(int index, YamanoteCategoryListCell listViewCell)
        {
            listViewCell.text.color = Color.white;
        }

        protected override void OnCellSelected(int index, YamanoteCategoryListCell listViewCell)
        {
            listViewCell.text.color = Color.black;
        }
    }

}
