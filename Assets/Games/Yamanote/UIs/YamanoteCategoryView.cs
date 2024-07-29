using System.Collections.Generic;
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
    
        // public YamanoteDataset dataset;


        void Start()
        {
            Debug.Log("YamanoteThemeSelectionView started.");

            listView.FocusOnCell(0);

            // var json = JsonConvert.SerializeObject(dataset);
            // Debug.Log($"[YamanoteThemeSelectionView] Dataset JSON: {json}");

            // YamanoteDAO.Instance.Data.ContinueWith(async task =>
            // {
            //     var data = task.Result;
            //     Debug.Log($"[YamanoteThemeSelectionView] Data loaded: {data.categories.Count} categories.");
            //     listView.Data = data.categories;
            //     await UniTask.DelayFrame(2);
            //     listView.FocusOnCell(0);
            // });
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
