// Created by LunarEclipse on 2024-7-12 22:2.

using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace USEN.MiniGames.Roulette
{
    public class RouletteEditListCell : ListViewCell<RouletteSector>
    {
        public TextMeshProUGUI indexText;
        [FormerlySerializedAs("InputField")] public TMP_InputField inputField;
        
        
        private RouletteSector _data;
        public override RouletteSector Data
        {
            get => _data;
            set
            {
                _data = value;
                inputField.text = value.content;
            }
        }

        void Start()
        {
            indexText.text = Index.ToString();
            
            this.colors = new ColorBlock
            {
                normalColor = Color.clear,
                highlightedColor = new Color(0.8f, 0.8f, 0.8f, 0.3f),
                pressedColor = new Color(0.6f, 0.6f, 0.6f, 0.3f),
                selectedColor = new Color(0.8f, 0.8f, 0.8f, 0.3f),
                disabledColor = new Color(0f, 0f, 0f, 0.3f),
                colorMultiplier = 1,
                fadeDuration = 0.1f
            };
        }
        
        public override async void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            Debug.Log($"[RouletteEditListCell] OnSelect: {Index}");
            await UniTask.NextFrame();
            inputField.Select();
        }
    }
}