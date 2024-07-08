using System;
using System.Collections;
using System.Collections.Generic;
using Luna.UI;
using Luna.UI.Navigation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class RouletteThemeSelectionView : Widget
{
    public ScrollRect listView;
    
    void Start()
    {
        Debug.Log("RouletteThemeSelectionView started.");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) ||
            Input.GetButtonDown("Cancel")) {
            Navigator.Pop();
        }
    }

    protected override KeyEventResult OnKey(KeyControl key, KeyEvent keyEvent)
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
