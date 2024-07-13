using Cysharp.Threading.Tasks;
using Luna.UI;
using Luna.UI.Navigation;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using USEN.Games.Common;
using USEN.Games.Roulette;

public class RouletteCategoryView : Widget
{
    public RouletteCategoryList listView;
    public BottomPanel bottomPanel;
    
    public RouletteDataset dataset;
    
    void OnEnable()
    {
        listView.FocusOnCell(0);
    }
    
    void Start()
    {
        Debug.Log("RouletteThemeSelectionView started.");
        
        // var json = JsonConvert.SerializeObject(dataset);
        // Debug.Log($"[RouletteThemeSelectionView] Dataset JSON: {json}");
        
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

    protected KeyEventResult OnKey(KeyControl key, KeyEvent keyEvent)
    {
        Debug.Log($"[RouletteThemeSelectionView] Key pressed: {key.keyCode} with event: {keyEvent}");
        switch (key.keyCode)
        {
            // case Key.UpArrow:
            //     listView.verticalNormalizedPosition += 0.1f;
            //     return KeyEventResult.Handled;
            // case Key.DownArrow:
            //     listView.verticalNormalizedPosition -= 0.1f;
            //     return KeyEventResult.Handled;
            // case Key.LeftArrow:
            //     listView.horizontalNormalizedPosition -= 0.1f;
            //     return KeyEventResult.Handled;
            // case Key.RightArrow:
            //     listView.horizontalNormalizedPosition += 0.1f;
            //     return KeyEventResult.Handled;
            // case Key.Enter:
            //     var selected = EventSystem.current.currentSelectedGameObject;
            //     if (selected != null)
            //     {
            //         selected.GetComponent<Button>().onClick.Invoke();
            //         return KeyEventResult.Handled;
            //     }
            //     break;
        }
        
        return KeyEventResult.Unhandled;
    }
    
}
