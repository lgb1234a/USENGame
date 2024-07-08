using System.Linq;
using Luna.UI.Navigation;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using USEN.MiniGames.Roulette;

public class RouletteGameSelectionListCell : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    public RouletteWheel rouletteWheel;
    public TextMeshProUGUI text;
    
    [HideInInspector] public RouletteSectors rouletteData;
    
    public void OnSelect(BaseEventData eventData)
    {
        text.color = Color.black;
        
        // Emit event
        ExecuteEvents.ExecuteHierarchy<RouletteGameSelectionList>(gameObject, null, (target, data) =>
        {
            target.SnapTo(transform as RectTransform);
            if (rouletteWheel != null)
            {
                // Change roulette wheel data
                rouletteWheel.Sectors = rouletteData.objects;
            }
        });
    }

    public void OnDeselect(BaseEventData eventData)
    {
        text.color = Color.white;
    }

    public void OnSubmit(BaseEventData eventData)
    {
        // Navigator.Push<RouletteGameSelectionView>();
    }
    
    public void OnClick()
    {
        // Emit event
        ExecuteEvents.ExecuteHierarchy<RouletteGameSelectionView>(gameObject, null, (target, data) =>
        {
            target.rouletteGameSelectionList.gameObject.SetActive(false);
            target.rouletteContentList.gameObject.SetActive(true);
            
            // Change roulette content list data
            target.rouletteContentList.Data = rouletteData.objects.Select(x => x.content).ToList();
            target.rouletteContentList.FocusOnCell(0);
        });
    }
    
    public void Test()
    {
        Debug.Log("Test");
    }
}


