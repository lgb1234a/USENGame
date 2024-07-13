using System.Linq;
using Luna.UI.Navigation;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using USEN.Games.Roulette;

public class RouletteCategoryListCell : ListViewCell<RouletteCategory>, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    public TextMeshProUGUI text;
    
    private RouletteCategory _rouletteCategory;
    
    public override RouletteCategory Data
    {
        get => _rouletteCategory;
        set
        {
            _rouletteCategory = value;
            text.text = value.title;
        }
    }
    
    public void OnClick()
    {
        Navigator.Push<RouletteGameSelectionView>();
        
        // Emit event
        // ExecuteEvents.ExecuteHierarchy<RouletteGameSelectionView>(gameObject, null, (target, data) =>
        // {
        //     // target.rouletteGameSelectionList.gameObject.SetActive(false);
        //     // target.rouletteContentList.gameObject.SetActive(true);
        //     
        //     // Change roulette content list data
        //     target.rouletteContentList.Data = _rouletteData.objects;
        //     // target.rouletteContentList.FocusOnCell(0);
        // });
    }
    
    public void Test()
    {
        Debug.Log("Test");
    }
}


