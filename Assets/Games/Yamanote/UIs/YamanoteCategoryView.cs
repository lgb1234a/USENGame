using System;
using System.Collections.Generic;
using System.Linq;
using Luna.UI;
using Luna.UI.Navigation;
using UnityEngine;
using USEN.Games.Common;

namespace USEN.Games.Yamanote
{
    public class YamanoteCategoryView : Widget
    {
        public YamanoteCategoryList listView;
        public BottomPanel bottomPanel;
        public TextAsset categoriesJson;

        private YamanoteDAO _dao;
        private List<YamanoteCategory> _categories;

        void Start()
        {
            _dao = new();
            if (_dao.IsEmpty())
                _dao.InsertFromJsonList(categoriesJson.text);
            _categories = _dao.GetCategories();
            listView.Data = _categories;
            
            listView.FocusOnCell(0);
            
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) ||
                Input.GetButtonDown("Cancel")) 
                Navigator.Pop();
        }

    }
}
