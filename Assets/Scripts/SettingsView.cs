using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SettingsView : IViewOperater
{
    string m_prefabPath = "SettingsPanel";
    GameObject m_viewGameObject;

    public SettingsView() {
        var obj = Resources.Load<GameObject>(m_prefabPath);
        m_viewGameObject = GameObject.Instantiate<GameObject>(obj, Vector3.zero, Quaternion.identity, ViewManager.Instance.GetRootTransform());
        var position = m_viewGameObject.transform.localPosition;
        position.z = 0;
        m_viewGameObject.transform.localPosition = position;
    }


    public void Show() {
        m_viewGameObject.SetActive(true);
    }

    public void Hide() {
        m_viewGameObject.SetActive(false);
        ViewManager.Instance.Pop();
    }

    public void Update() {
        if (Input.GetButtonDown("Cancel")) {
            Hide();
        }
    }

    public void OnAndroidKeyDown(string keyName) {
        
    }

    public void OnClickBackHomeButton() {
        Hide();
    }
}
