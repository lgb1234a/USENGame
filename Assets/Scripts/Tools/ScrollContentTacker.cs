using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;

class ScrollContentTacker : MonoBehaviour
{
    public RectTransform ViewPort;
    Canvas m_canvas;
    void Start() {
        m_canvas = GetComponent<Canvas>();
    }

    void LateUpdate() 
    {
        var selectedObj = EventSystem.current.currentSelectedGameObject;
        var screenPosition = m_canvas.worldCamera.WorldToScreenPoint(selectedObj.transform.position);
        
        if ((transform as RectTransform).anchoredPosition.y < 100 && screenPosition.y < 200) {
            var position = (transform as RectTransform).anchoredPosition;
            position.y = 750;
            (transform as RectTransform).anchoredPosition = position;
        }
        
        if ((transform as RectTransform).anchoredPosition.y > 700 && screenPosition.y > 900) {
            var position = (transform as RectTransform).anchoredPosition;
            position.y = 0;
            (transform as RectTransform).anchoredPosition = position;
        }
    }
}