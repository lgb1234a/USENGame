using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Spine.Unity;

public class HightAndLowGameView : AbstractView, IViewOperater
{
    string m_prefabPath = "HighAndLow/HighAndLowGamePanel";
    HighAndLowHistoryView m_historyView;
    public void Build()
    {
        m_mainViewGameObject = LoadViewGameObject(m_prefabPath, ViewManager.Instance.GetRootTransform());

    }

    public void Hide()
    {
        m_mainViewGameObject.SetActive(false);
    }

    public void OnAndroidKeyDown(string keyName)
    {
        if (keyName == "blue")
        {
            if (m_historyView == null) {
                m_historyView = new HighAndLowHistoryView();
            }
            ViewManager.Instance.Push(m_historyView);
        }
    }

    public void OnThemeTypeChanged()
    {
        
    }

    public void Show()
    {
        m_mainViewGameObject.SetActive(true);
    }

    public void Update()
    {
        if (Input.GetButtonDown("Cancel")) {
            Hide();
            ViewManager.Instance.Hided(this);
        }

        if (Input.GetKeyDown(KeyCode.A)) {
            if (m_historyView == null) {
                m_historyView = new HighAndLowHistoryView();
            }
            ViewManager.Instance.Push(m_historyView);
        }
    }

    public void OnClickStartButton()
    {

    }
}