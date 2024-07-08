using Luna.UI.Navigation;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace USEN.MiniGames.Roulette
{
    public class RouletteContentListCell : ListViewCell<string>
    {
        public TextMeshProUGUI text;
        private Button _button;
    
        public override string Data
        {
            get => text.text;
            set => text.text = value;
        }
        
        void Awake()
        {
            _button = GetComponent<Button>();
            _button.colors = new ColorBlock
            {
                normalColor = Color.white,
                highlightedColor = Color.white,
                pressedColor = Color.white,
                selectedColor = Color.white,
                disabledColor = Color.white,
                colorMultiplier = 1,
                fadeDuration = 0.1f
            };
        }
    
        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            
            text.color = Color.black;
        
            // Emit event
            ExecuteEvents.ExecuteHierarchy<RouletteSelectionList>(gameObject, null, (target, data) =>
            {
                target.SnapTo(transform as RectTransform);
            });
        }

        public void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            
            text.color = Color.white;
        }

        public void OnSubmit(BaseEventData eventData)
        {
            base.OnSubmit(eventData);
            
            // Navigator.Push<RouletteGameSelectionView>();
        }
    
        public void OnClick()
        {
            Navigator.Push<RouletteGameSelectionView>();
        }
    
        public void Test()
        {
            Debug.Log("Test");
        }
    }
}
