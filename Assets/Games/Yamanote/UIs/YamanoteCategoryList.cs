using System.Collections.Generic;
using Modules.UI.Widgets;
using UnityEngine;
using UnityEngine.EventSystems;

namespace USEN.Games.Yamanote
{
    
    public class YamanoteCategoryList : ListView<YamanoteCategoryListCell, YamanoteCategory>, IEventSystemHandler
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
                    themes = new List<YamanoteTheme>
                    {
                        new YamanoteTheme
                        {
                            title = "Theme 1",
                            questions = new List<string>
                            {
                                "Question 1",
                                "Question 2",
                                "Question 3",
                            }
                        },
                        new YamanoteTheme
                        {
                            title = "Theme 2",
                            questions = new List<string>
                            {
                                "Question 1",
                                "Question 2",
                                "Question 3",
                            }
                        },
                        new YamanoteTheme
                        {
                            title = "Theme 3",
                            questions = new List<string>
                            {
                                "Question 1",
                                "Question 2",
                                "Question 3",
                            }
                        }
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
