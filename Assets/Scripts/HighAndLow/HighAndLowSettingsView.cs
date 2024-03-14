using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Spine.Unity;

public class HighAndLowSettingsView : AbstractView, IViewOperater
{
    string m_prefabPath = "HighAndLowSettingsPanel";
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
    }

    public void OnClickStartButton()
    {

    }
}