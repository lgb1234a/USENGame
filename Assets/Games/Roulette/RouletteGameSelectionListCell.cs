using System.Linq;
using Luna.UI.Navigation;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using USEN.MiniGames.Roulette;

public class RouletteGameSelectionListCell : ListViewCell<RouletteSectors>, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    public RouletteWheel rouletteWheel;
    public TextMeshProUGUI text;
    
    private RouletteSectors _rouletteData;
    public override RouletteSectors Data
    {
        get => _rouletteData;
        set
        {
            _rouletteData = value;
            text.text = value.name;
        }
    }
    
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        text.color = Color.black;
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        text.color = Color.white;
    }

    public override void OnSubmit(BaseEventData eventData)
    {
        base.OnSubmit(eventData);
        // Navigator.Push<RouletteGameSelectionView>();
    }
    
    public void OnClick()
    {
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


