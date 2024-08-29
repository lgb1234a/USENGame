using Luna.UI;
using Luna.UI.Navigation;
using UnityEngine;
using UnityEngine.EventSystems;

namespace USEN.Games.Yamanote
{
    
    public class YamanoteQuestionsList : FixedListView<YamanoteQuestionsListCell, string>, IEventSystemHandler
    {
        protected override void OnCellClicked(int index, YamanoteQuestionsListCell listViewCell)
        {
            Navigator.Push<YamanoteGameView>((view) =>
            {
                view.Questions = Data;
            });
        }
        
        protected override void OnCellSubmitted(int index, YamanoteQuestionsListCell listViewCell)
        {
            Navigator.Push<YamanoteGameView>((view) =>
            {
                view.Questions = Data;
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
