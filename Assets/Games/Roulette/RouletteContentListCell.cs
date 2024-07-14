using LeTai;
using Luna.UI.Navigation;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace USEN.Games.Roulette
{
    public class RouletteContentListCell : ListViewCell<RouletteSector>
    {
        public TextMeshProUGUI text;
        private Button _button;
    
        private RouletteSector _data;
        public override RouletteSector Data
        {
            get => _data;
            set
            {
                _data = value;
                if (_button != null)
                    _button.colors = new ColorBlock
                    {
                        normalColor = value.color,
                        highlightedColor =  _button.colors.highlightedColor,
                        pressedColor =  _button.colors.pressedColor,
                        selectedColor = value.color.WithA(0.5f),
                        disabledColor = _button.colors.disabledColor,
                        colorMultiplier = _button.colors.colorMultiplier,
                        fadeDuration = _button.colors.fadeDuration
                    };
                if (text != null)
                    text.text = value.content;
            }
        }

        void Awake()
        {
            _button = GetComponent<Button>();
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
    }
}
