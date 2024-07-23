using Cysharp.Threading.Tasks;
using Luna.UI;
using Luna.UI.Navigation;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using USEN.Games.Common;
using USEN.Games.Yamanote;

public class YamanoteQuestionView : Widget
{
    public YamanoteCategoryList listView;
    public BottomPanel bottomPanel;
    
    // public YamanoteDataset dataset;
    
    void OnEnable()
    {
        listView.FocusOnCell(0);
    }
    
    void Start()
    {
        Debug.Log("YamanoteThemeSelectionView started.");
        
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
