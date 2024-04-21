using UnityEngine;

public class BingoAboutView : AbstractView, IViewOperater
{
    string m_prefabPath = "Bingo/BingoAboutPanel";
    BingoSettingsView m_settingsView;

    public void Build() {
        m_mainViewGameObject = LoadViewGameObject(m_prefabPath, ViewManager.Instance.GetRootTransform());
        // m_mainViewGameObject = GameObject.Instantiate<GameObject>(obj, Vector3.zero, Quaternion.identity, ViewManager.Instance.GetRootTransform());
        var position = m_mainViewGameObject.transform.localPosition;
        position.z = 0;
        m_mainViewGameObject.transform.localPosition = position;
    }

    public void Show() {
        m_mainViewGameObject.SetActive(true);
    }

    public void Hide() {
        m_mainViewGameObject.SetActive(false);
    }

    public void Update() {
        if (Input.GetButtonDown("Cancel")) {
            OnClickBackHomeButton();
        }
    }

    public void OnAndroidKeyDown(string keyName) {
        
    }

    public void OnClickBackHomeButton() {
        if (m_settingsView == null) 
            m_settingsView = new BingoSettingsView();
        ViewManager.Instance.Push(m_settingsView);
    }

    public void OnThemeTypeChanged() {

    }
}
