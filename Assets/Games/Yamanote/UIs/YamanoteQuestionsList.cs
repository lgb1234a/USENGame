using Luna.UI;
using Luna.UI.Navigation;
using UnityEngine;
using UnityEngine.EventSystems;

namespace USEN.Games.Yamanote
{
    
    public class YamanoteQuestionsList : ListView<YamanoteQuestionsListCell, YamanoteTheme>, IEventSystemHandler
    {
        protected void Start()
        {
            var content = _scrollRect.content;
            var firstCell = content.GetChild(0).GetComponent<YamanoteQuestionsListCell>();
            firstCell.Focus();
        }

        protected override void OnCellClicked(int index, YamanoteQuestionsListCell listViewCell)
        {
            Navigator.Push<YamanoteGameView>((view) =>
            {
                
            });
        }
        
        protected override void OnCellSubmitted(int index, YamanoteQuestionsListCell listViewCell)
        {
            Navigator.Push<YamanoteGameView>((view) =>
            {
                
            });
        }

        protected override void OnCellDeselected(int index, YamanoteQuestionsListCell listViewCell)
        {
            listViewCell.text.color = Color.black;
        }

        protected override void OnCellSelected(int index, YamanoteQuestionsListCell listViewCell)
        {
            listViewCell.text.color = Color.HSVToRGB(148f / 360, 0.9f, 0.6f);
        }
    }

}
