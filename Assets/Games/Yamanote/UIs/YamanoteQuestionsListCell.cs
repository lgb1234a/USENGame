// Created by LunarEclipse on 2024-7-29 20:59.

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace USEN.Games.Yamanote
{
    public class YamanoteQuestionsListCell : ListViewCell<YamanoteTheme>
    {
        public TextMeshProUGUI text;
        public Image background;
        public CircleCollider2D ringCollider;

        private YamanoteTheme _data;
        public override YamanoteTheme Data
        {
            get => _data;
            set
            {
                _data = value;
                text.text = value.title;
            }
        }
    }
}