using System.Collections.Generic;
using Luna.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace USEN.Games.Yamanote
{
    
    public class YamanoteCategoryList : FixedListView<YamanoteCategoryListCell, YamanoteCategory>, IEventSystemHandler
    {
        protected void Start()
        {
            var content = _scrollRect.content;
            var firstCell = content.GetChild(0).GetComponent<YamanoteCategoryListCell>();
            firstCell.Focus();

            for (int i = 0; i < content.childCount; i++)
            {
                var cell = content.GetChild(i).GetComponent<YamanoteCategoryListCell>();
                cell.Data = new YamanoteCategory
                {
                    title = $"Category {i + 1}",
                    questions = new List<string>
                    {
                        "芸人（トリオ）の名前",
                        "日本の球団の名前",
                        "アニメの主題歌",
                        "演歌歌手の名",
                        "前力士の名",
                        "都道府県23区",
                        "山手線駅名",
                        "中央線駅名",
                        "日本の車メーカー",
                    },
                }; 
            }
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
