using Cysharp.Threading.Tasks;
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
    
        public RouletteDataset dataset;
    
        void Start()
        {
            // Load the roulette data
            RouletteDAO.Instance.Data.ContinueWith(async task =>
            {
                var data = task.Result;
                Debug.Log($"[RouletteThemeSelectionView] Data loaded: {data.categories.Count} categories.");
                listView.Data = data.categories;
                await UniTask.DelayFrame(2);
                listView.FocusOnCell(0);
            });
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
