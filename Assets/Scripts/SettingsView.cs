using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SettingsView : IViewOperater
{
    string m_prefabPath = "SettingsPanel";
    GameObject m_viewGameObject;
    Button m_backHomeButton;

    public SettingsView() {
        var obj = Resources.Load<GameObject>(m_prefabPath);
        m_viewGameObject = GameObject.Instantiate<GameObject>(obj, Vector3.zero, Quaternion.identity, ViewManager.Instance.GetRootTransform());
        var position = m_viewGameObject.transform.localPosition;
        position.z = 0;
        m_viewGameObject.transform.localPosition = position;
        
        m_backHomeButton = m_viewGameObject.transform.Find("PlayPanel/BackHomeButton").GetComponent<Button>();
        m_backHomeButton.onClick.AddListener(OnClickBackHomeButton);
    }


    public void Show() {
        m_viewGameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(m_backHomeButton.gameObject);
    }

    public void Hide() {
        m_viewGameObject.SetActive(false);
        ViewManager.Instance.Pop();
    }

    public void Update() {
        
    }

    public void OnAndroidKeyDown(string keyName) {
        
    }

    public void OnClickBackHomeButton() {
        Hide();
    }
}
