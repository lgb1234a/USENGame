using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RouletteThemeSelectionList : MonoBehaviour, IEventSystemHandler
{
    protected ScrollRect scrollRect;

    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        
        // Set the selected game object to the first button
        EventSystem.current.SetSelectedGameObject(scrollRect.content.GetChild(0).gameObject);
    }

    public void SnapTo(RectTransform target)
    {
        var y = -target.anchoredPosition.y - ((RectTransform)scrollRect.transform).sizeDelta.y;
        y = Mathf.Clamp(y, 0, scrollRect.content.sizeDelta.y);
        var pos = new Vector2(scrollRect.content.anchoredPosition.x, y);
        DOTween.To(() => scrollRect.content.anchoredPosition, v => scrollRect.content.anchoredPosition = v, pos, 0.5f);
    }
}
