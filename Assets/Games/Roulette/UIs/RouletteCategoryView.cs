using System;
using System.Collections.Generic;
using Luna.UI;
using Luna.UI.Navigation;
using UnityEngine;
using USEN.Games.Common;

namespace USEN.Games.Roulette
{
    public class RouletteCategoryView : Widget
    {
        public RouletteCategoryList listView;
        public BottomPanel bottomPanel;

        public List<RouletteCategory> Categories
        {
            get => listView.Data;
            set => listView.Data = value;
        }

        void Start()
        {
            listView.FocusOnCell(0);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) ||
                Input.GetButtonDown("Cancel")) {
                Navigator.Pop();
            }
        }
    }
}
