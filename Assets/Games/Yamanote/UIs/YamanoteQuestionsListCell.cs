// Created by LunarEclipse on 2024-7-29 20:59.

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace USEN.Games.Yamanote
{
    public class YamanoteQuestionsListCell : FixedListViewCell<string>
    {
        public TextMeshProUGUI text;
        public Image background;
        public CircleCollider2D ringCollider;

        public override string Data
        {
            get => text.text;
            set => text.text = value;
        }
    }
}