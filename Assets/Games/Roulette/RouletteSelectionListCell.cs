using Luna.UI.Navigation;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class RouletteSelectionListCell : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    public TextMeshProUGUI text;
    
    public void OnSelect(BaseEventData eventData)
    {
        text.color = Color.black;
        
        // Emit event
        ExecuteEvents.ExecuteHierarchy<RouletteThemeSelectionList>(gameObject, null, (target, data) =>
        {
            target.SnapTo(transform as RectTransform);
        });
    }

    public void OnDeselect(BaseEventData eventData)
    {
        text.color = Color.white;
    }

    public void OnSubmit(BaseEventData eventData)
    {
        Navigator.Push<USEN.MiniGames.Roulette.RouletteGameView>();
    }
}


