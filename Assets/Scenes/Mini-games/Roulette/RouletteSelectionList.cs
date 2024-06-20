using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RouletteSelectionList : MonoBehaviour, IEventSystemHandler
{
    protected ScrollRect scrollRect;

    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    public void SnapTo(RectTransform target)
    {
        var y = -target.anchoredPosition.y - ((RectTransform)scrollRect.transform).sizeDelta.y;
        y = Mathf.Clamp(y, 0, scrollRect.content.sizeDelta.y);
        var pos = new Vector2(scrollRect.content.anchoredPosition.x, y);
        DOTween.To(() => scrollRect.content.anchoredPosition, v => scrollRect.content.anchoredPosition = v, pos, 0.5f);
    }
}
